/**
 * Copyright 2015 d-fens GmbH
 * 
 * Uses work from Mike Wassom, Microsoft Corporation
 * Supporting Entity Relations in OData v3 with Web API 2
 * http://www.asp.net/web-api/overview/odata-support-in-aspnet-web-eapi/odata-v3/working-with-entity-relations
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
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Extensions;
using System.Web.Http.OData.Routing;
using System.Web.Http.Routing;
using Microsoft.Data.OData;
using Microsoft.Data.OData.Query;
using Diagnostics = System.Diagnostics;

namespace biz.dfch.CS.Utilities.OData
{
    public static class ODataControllerExtensions
    {
        // DFTODO - this cannot be the final solution - way to much overhead
        public static T GetKeyFromLinkUri<T>(this ODataController controller, Uri link)
        {
            Contract.Requires(null != link);

            T key = default(T);

            IHttpRoute route = controller.Request.GetRouteData().Route;
            IHttpRoute newRoute = new HttpRoute(
                route.RouteTemplate
                ,
                new HttpRouteValueDictionary(route.Defaults)
                ,
                new HttpRouteValueDictionary(route.Constraints)
                ,
                new HttpRouteValueDictionary(route.DataTokens), route.Handler
                );

            var newRequest = new HttpRequestMessage(HttpMethod.Get, link);
            var routeData = newRoute.GetRouteData(controller.Request.GetConfiguration().VirtualPathRoot, newRequest);
            if (null != routeData)
            {
                ODataPath path = newRequest.ODataProperties().Path;
                Diagnostics::Trace.Assert(null != path);
                var segment = path.Segments.OfType<KeyValuePathSegment>().FirstOrDefault();
                if (null != segment)
                {
                    key = (T)ODataUriUtils.ConvertFromUriLiteral(segment.Value, ODataVersion.V3);
                }
            }
            return key;
        }
    }
}
