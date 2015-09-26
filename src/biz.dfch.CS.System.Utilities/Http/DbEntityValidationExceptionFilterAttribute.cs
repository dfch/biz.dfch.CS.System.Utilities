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

using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using biz.dfch.CS.Utilities.Logging;
using System.Data.Entity.Validation;

namespace biz.dfch.CS.Utilities.Http
{
    public class DbEntityValidationExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is DbEntityValidationException)
            {
                var ex = context.Exception as DbEntityValidationException;

                string errorMessage = ex.Message;
                if (null != ex.EntityValidationErrors)
                {
                    foreach (var error in ex.EntityValidationErrors)
                    {
                        if (null == error.ValidationErrors)
                        {
                            continue;
                        }
                        foreach (var errorInner in error.ValidationErrors)
                        {
                            errorMessage = string.Concat(errorMessage, "\r\n", errorInner.PropertyName, ": ", errorInner.ErrorMessage);
                        }
                    }
                }

                Trace.WriteException(errorMessage, ex);
                context.Response = context.Request.CreateErrorResponse(HttpStatusCode.BadRequest, errorMessage);
            }
        }
    }
}