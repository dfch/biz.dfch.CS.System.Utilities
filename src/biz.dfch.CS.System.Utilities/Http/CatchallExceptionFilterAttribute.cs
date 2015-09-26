/**
 * Copyright 2015 d-fens GmbH
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

using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using biz.dfch.CS.Utilities.Logging;

namespace biz.dfch.CS.Utilities.Http
{
    public class CatchallExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private static bool _suppressStackTrace = false;

        public CatchallExceptionFilterAttribute()
        {
            // N/A
        }

        public CatchallExceptionFilterAttribute(bool suppressStackTrace)
        {
            _suppressStackTrace = suppressStackTrace;
        }
        
        public override void OnException(HttpActionExecutedContext context)
        {
            if (null == context || null == context.Exception)
            {
                return;
            }

            var ex = context.Exception;
            var message = String.Format(
                "{0}-EX {1}"
                ,
                context.ActionContext.Request.GetCorrelationId().ToString()
                ,
                ex.Message
                );
            Trace.WriteException(message, ex);

            if(_suppressStackTrace)
            {
                context.Response = context.Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}