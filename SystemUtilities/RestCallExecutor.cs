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
using System.Collections.Generic;
﻿using System.Net;
﻿using System.Net.Http;
﻿using System.Net.Http.Headers;

namespace biz.dfch.CS.System.Utilities
{
    public class RestCallExecutor
    {
        private HttpClient _httpClient;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public RestCallExecutor()
        {
            _httpClient = new HttpClient();
        }

        /// <summary>
        /// Extended Constructor to set credentials of HttpClient
        /// </summary>
        /// <param name="credentials"></param>
        public RestCallExecutor(ICredentials credentials)
        {
            var clientHandler = new HttpClientHandler();
            clientHandler.Credentials = credentials;
            _httpClient = new HttpClient(clientHandler);
        }

        // DFTODO pass headers
        public String Invoke(String Method, String Uri, String Body)
        {
            _httpClient.BaseAddress = new Uri(Uri);
            int _TimeoutSec = 90;
            _httpClient.Timeout = new TimeSpan(0, 0, _TimeoutSec);
            string _ContentType = "application/json";
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_ContentType));
            var _CredentialBase64 = "RWRnYXJTY2huaXR0ZW5maXR0aWNoOlJvY2taeno=";
            _httpClient.DefaultRequestHeaders.Add("Authorization", String.Format("Basic {0}", _CredentialBase64));
            var _UserAgent = "d-fens HttpClient";
            // You can actually also set the User-Agent via a built-in property
            _httpClient.DefaultRequestHeaders.Add("User-Agent", _UserAgent);
            // You get the following exception when trying to set the "Content-Type" header like this:
            // cl.DefaultRequestHeaders.Add("Content-Type", _ContentType);
            // "Misused header name. Make sure request headers are used with HttpRequestMessage, response headers with HttpResponseMessage, and content headers with HttpContent objects."

            HttpResponseMessage response;
            var _Method = new HttpMethod(Method);

            switch (_Method.ToString().ToUpper())
            {
                case "GET":
                case "HEAD":
                    // synchronous request without the need for .ContinueWith() or await
                    response = httpClient.GetAsync(Uri).Result;
                    break;
                case "POST":
                    {
                        // Construct an HttpContent from a StringContent
                        HttpContent _Body = new StringContent(Body);
                        // and add the header to this object instance
                        // optional: add a formatter option to it as well
                        _Body.Headers.ContentType = new MediaTypeHeaderValue(_ContentType);
                        // synchronous request without the need for .ContinueWith() or await
                        response = httpClient.PostAsync(Uri, _Body).Result;
                    }
                    break;
                case "PUT":
                    {
                        // Construct an HttpContent from a StringContent
                        HttpContent _Body = new StringContent(Body);
                        // and add the header to this object instance
                        // optional: add a formatter option to it as well
                        _Body.Headers.ContentType = new MediaTypeHeaderValue(_ContentType);
                        // synchronous request without the need for .ContinueWith() or await
                        response = httpClient.PutAsync(Uri, _Body).Result;
                    }
                    break;
                case "DELETE":
                    response = httpClient.DeleteAsync(Uri).Result;
                    break;
                default:
                    throw new NotImplementedException();
                    break;
            }
            // either this - or check the status to retrieve more information
            response.EnsureSuccessStatusCode();
            // get the rest/content of the response in a synchronous way
            var content = response.Content.ReadAsStringAsync().Result;

            return content;
        }
    }
}