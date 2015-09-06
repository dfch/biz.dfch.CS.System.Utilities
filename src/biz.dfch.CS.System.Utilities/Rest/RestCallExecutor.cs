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
﻿using biz.dfch.CS.Utilities.Attributes;

namespace biz.dfch.CS.Utilities.Rest
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
        private const String AUTHORIZATION_HEADER_KEY = "Authorization";
        private const String USER_AGENT_HEADER_KEY = "User-Agent";

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

        private Boolean _EnsureSuccessStatusCode;
        public Boolean EnsureSuccessStatusCode 
        {
            get { return _EnsureSuccessStatusCode; }
            set { _EnsureSuccessStatusCode = value; }
        }

        private String _AuthScheme;
        /// <summary>
        /// Sets the authentication scheme to use for creating the 'Authorization'
        /// header out of the header values passed to the invoke method.
        /// (i.e. Basic, Bearer, ...)
        /// </summary>
        public String AuthScheme
        {
            get { return _AuthScheme; }
            set { _AuthScheme = value; }
        }
        #endregion

        #region Constructors
        public RestCallExecutor(Boolean ensureSuccessStatusCode = true)
        {
            _EnsureSuccessStatusCode = ensureSuccessStatusCode;
            _ContentType = ContentType.ApplicationJson;
            _Timeout = DEFAULT_TIMEOUT;
        }
        #endregion

        /// <summary>
        /// Executes a HTTP GET request to the passed uri
        /// </summary>
        /// <param name="uri">A valid URI</param>
        /// <returns>The response body as String if succeded, otherwise an exception is thrown</returns>
        #region Invoke
        public String Invoke(String uri)
        {
            return Invoke(HttpMethod.Get, uri, null, null);
        }

        /// <summary>
        /// Executes a HTTP GET request to the passed uri with given headers
        /// </summary>
        /// <param name="uri">A valid URI</param>
        /// <param name="headers">A dictionary of headers (header key, header value)</param>
        /// <returns>The response body as String if succeded, otherwise an exception is thrown</returns>
        public String Invoke(String uri, IDictionary<String, String> headers)
        {
            return Invoke(HttpMethod.Get, uri, headers, null);
        }

        /// <summary>
        /// Executes a HTTP request based on the passed parameters
        /// </summary>
        /// <param name="method">HTTP Method (GET, PUT, POST, HEAD or DELETE)</param>
        /// <param name="uri">A valid URI</param>
        /// <param name="headers">A dictionary of headers (header key, header value)</param>
        /// <param name="body">The payload formatted according the ContentType property (default = application/json)</param>
        /// <returns>he response body as String if succeded, otherwise an exception is thrown</returns>
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

                if (null != _AuthScheme && null != headers && headers.ContainsKey(AUTHORIZATION_HEADER_KEY))
                {
                    httpClient.DefaultRequestHeaders.Authorization = 
                        new AuthenticationHeaderValue(_AuthScheme, headers[AUTHORIZATION_HEADER_KEY]);
                    headers.Remove(AUTHORIZATION_HEADER_KEY);
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
                        content.Headers.ContentType = new MediaTypeHeaderValue(_ContentType.GetStringValue());
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

                if (EnsureSuccessStatusCode)
                {
                    response.EnsureSuccessStatusCode();
                }

                return response.Content.ReadAsStringAsync().Result;
            }
        }

        /// <summary>
        /// Extracts the 'User-Agent' header from the header parameters
        /// and removes the value from the passed headers dictionary.
        /// </summary>
        /// <param name="headers"></param>
        /// <returns>The user agent header value if present,
        /// otherwise the default value 'RestCallExecutor'</returns>
        private String ExtractUserAgentFromHeaders(IDictionary<String, String> headers)
        {
            if (null != headers && headers.ContainsKey(USER_AGENT_HEADER_KEY))
            {
                var userAgent = headers[USER_AGENT_HEADER_KEY];
                headers.Remove(USER_AGENT_HEADER_KEY);
                return userAgent;
            }
            return DEFAULT_USER_AGENT;
        }
        #endregion

        /// <summary>
        /// Checks if the 'uri' passed as string is a valid URI.
        /// Throws an ArgumentException if uri is not valid.
        /// </summary>
        /// <param name="uri">Uri as string</param>
        private void ValidateUriParameter(String uri)
        {
            if (String.IsNullOrWhiteSpace(uri))
            {
                throw new ArgumentException(String.Format("uri: Parameter validation FAILED. Parameter cannot be null or empty."), "uri");
            }
        }
    }
}