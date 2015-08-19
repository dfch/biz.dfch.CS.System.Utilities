/**
 * Copyright 2015 Marc Rufer, d-fens GmbH
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

﻿using System;
﻿using System.Collections.Generic;
﻿using System.Net.Http;
﻿using System.Net.Http.Headers;
﻿using biz.dfch.CS.System.Utilities.Attributes;

namespace biz.dfch.CS.System.Utilities.Rest
{
    /// <summary>
    /// Provides invoke methods to execute REST calls
    /// </summary>
    public class RestCallExecutor
    {
        #region Constants and Properties
        private const int DEFAULT_TIMEOUT = 90;
        private const String DEFAULT_USER_AGENT = "RestCallExecutor";
        private const String CONTENT_TYPE_HEADER_KEY = "Content-Type";
        private const String USER_AGENT_HEADER_KEY = "User-Agent";

        // DFTODO add credential support

        private int _Timeout;
        public int Timeout
        {
            get { return _Timeout; }
            set { _Timeout = value; }
        }

        private ContentType _ContentType;
        public ContentType ContentType
        {
            get { return _ContentType; }
            set { _ContentType = value; }
        }

        private Boolean _EnsureSuccessCode;
        public Boolean EnsureSuccessCode 
        {
            get { return _EnsureSuccessCode; }
            set { _EnsureSuccessCode = value; }
        }
        #endregion

        #region Constructors
        public RestCallExecutor(Boolean ensureSuccessCode = true)
        {
            _EnsureSuccessCode = ensureSuccessCode;
            _ContentType = ContentType.ApplicationJson;
            _Timeout = DEFAULT_TIMEOUT;
        }
        #endregion

        #region Invoke
        public String Invoke(String uri)
        {
            return Invoke(HttpMethod.Get, uri, null, null);
        }

        public String Invoke(String uri, IDictionary<String, String> headers)
        {
            return Invoke(HttpMethod.Get, uri, headers, null);
        }

        // DFTODO C# doc
        public String Invoke(HttpMethod method, String uri, IDictionary<String, String> headers, String body)
        {
            ValidateUriParameter(uri);

            using (var httpClient = new HttpClient()) 
            {
                httpClient.BaseAddress = new Uri(uri);
                httpClient.Timeout = new TimeSpan(0, 0, _Timeout);

                if (null != headers && headers.ContainsKey(CONTENT_TYPE_HEADER_KEY))
                {
                    _ContentType = EnumUtil.Parse<ContentType>(headers[CONTENT_TYPE_HEADER_KEY]);
                    headers.Remove(CONTENT_TYPE_HEADER_KEY);
                }

                httpClient.DefaultRequestHeaders.Add(USER_AGENT_HEADER_KEY, ExtractUserAgentFromHeaders(headers));

                if (null != headers)
                {
                    foreach (var header in headers)
                    {
                        httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }

                HttpResponseMessage response;

                switch (method)
                {
                    case HttpMethod.Get:
                    case HttpMethod.Head:
                        response = httpClient.GetAsync(uri).Result;
                        break;
                    case HttpMethod.Post:
                        {
                            HttpContent content = new StringContent(body);
                            content.Headers.ContentType = new MediaTypeHeaderValue(_ContentType.GetStringValue());
                            response = httpClient.PostAsync(uri, content).Result;
                        }
                        break;
                    case HttpMethod.Put:
                        {
                            HttpContent content = new StringContent(body);
                            content.Headers.ContentType = new MediaTypeHeaderValue(_ContentType.ToString());
                            response = httpClient.PutAsync(uri, content).Result;
                        }
                        break;
                    case HttpMethod.Delete:
                        response = httpClient.DeleteAsync(uri).Result;
                        break;
                    default:
                        throw new NotImplementedException(String.Format("{0}: Method not implemented. " +
                                                                        "Currently only the following methods are implemented: 'GET', 'HEAD', 'POST', 'PUT', 'DELETE'.",
                                                                        method.GetStringValue()));
                }

                if (EnsureSuccessCode)
                {
                    response.EnsureSuccessStatusCode();
                }
                
                return response.Content.ReadAsStringAsync().Result;
            }
        }

        private String ExtractUserAgentFromHeaders(IDictionary<String, String> headers)
        {
            if (null != headers && headers.ContainsKey(USER_AGENT_HEADER_KEY))
            {
                headers.Remove(USER_AGENT_HEADER_KEY);
                return headers[USER_AGENT_HEADER_KEY];
            }
            return DEFAULT_USER_AGENT;
        }

        #endregion

        private void ValidateUriParameter(String uri)
        {
            if (String.IsNullOrWhiteSpace(uri)) throw new ArgumentException(String.Format("uri: Parameter validation FAILED. Parameter cannot be null or empty."), "uri");
        }
    }
}