using Newtonsoft.Json;
using Telerik.JustMock;
using biz.dfch.CS.Utilities.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
/**
 *
 *
 * Copyright 2015 Ronald Rink, d-fens GmbH
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

using System.Net.Http;
using System.Net;

namespace biz.dfch.CS.Utilities.Tests.Http
{
    [TestClass]
    public class HttpRequestMessageExtensionTest
    {
        [Ignore]
        [TestMethod]
        public void CreateCustomErrorResponseReturnsHttpResponse()
        {
            var errorCode = 42;
            var errorMessage = "arbitrary-error-httpStatusErrorMessage";
            var statusCode = HttpStatusCode.NotImplemented;
            var request = Mock.Create<HttpRequestMessage>(Behavior.RecursiveLoose, HttpMethod.Get, "http://localhost");
            var response = request.CreateCustomErrorResponse(statusCode, errorMessage, errorCode);

            Assert.AreEqual(statusCode, response.StatusCode);
            Assert.IsTrue(response.Content.ToString().Contains(errorMessage));
            var httpStatusErrorMessage = JsonConvert.DeserializeObject<HttpStatusErrorMessage>(response.Content.ToString());
            Assert.IsNotNull(httpStatusErrorMessage);
        }
    }
}
