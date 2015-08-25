/**
 *
 *
 * Copyright 2014-2015 Ronald Rink, d-fens GmbH
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
 *
 */
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Formatting;

namespace biz.dfch.CS.Utilities.Http
{
    public static class HttpRequestMessageExtension
    {
        public static HttpResponseMessage CreateCustomErrorResponse(this HttpRequestMessage request, HttpStatusCode httpStatusCode, HttpStatusErrorMessage httpStatusErrorMessage)
        {

            return request.CreateResponse(httpStatusCode, httpStatusErrorMessage.ToString());
        }
        
        public static HttpResponseMessage CreateCustomErrorResponse(this HttpRequestMessage request, HttpStatusCode httpStatusCode, string message, int? code = null)
        {
            var errmsg = new HttpStatusErrorMessage()
            {
                Code = code
                ,
                Message = message
            };
            return request.CreateResponse(httpStatusCode, errmsg.ToString());
        }

        public static HttpResponseMessage CreateCustomErrorResponse(this HttpRequestMessage request, HttpStatusCode httpStatusCode, Exception ex, int? code = null)
        {
            var errmsg = new HttpStatusErrorMessage()
            {
                Code = code ?? ex.HResult
                ,
                Message = ex.Message
                ,
                Innererror = ex
            };
            return request.CreateResponse(httpStatusCode, errmsg.ToString());
        }
    }
}
