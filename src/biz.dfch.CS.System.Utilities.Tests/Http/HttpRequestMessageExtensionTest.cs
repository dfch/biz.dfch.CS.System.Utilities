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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using biz.dfch.CS.Utilities.Http;
using Newtonsoft.Json;
using Telerik.JustMock;
using biz.dfch.CS.Utilities.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace biz.dfch.CS.Utilities.Tests.Http
{
    [TestClass]
    public class HttpRequestMessageExtensionTest
    {
        [TestMethod]
        public void CreateCustomErrorWithStringResponseReturnsHttpResponse()
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

        [TestMethod]
        public void CreateCustomErrorWithExceptionResponseReturnsHttpResponse()
        {
            var errorCode = 42;
            var errorMessage = "arbitrary-error-httpStatusErrorMessage";
            var exception = new Exception(errorMessage);
            var statusCode = HttpStatusCode.NotImplemented;
            var request = Mock.Create<HttpRequestMessage>(Behavior.RecursiveLoose, HttpMethod.Get, "http://localhost");
            var response = request.CreateCustomErrorResponse(statusCode, exception, errorCode);

            Assert.AreEqual(statusCode, response.StatusCode);
            Assert.IsTrue(response.Content.ToString().Contains(errorMessage));
            var httpStatusErrorMessage = JsonConvert.DeserializeObject<HttpStatusErrorMessage>(response.Content.ToString());
            Assert.IsNotNull(httpStatusErrorMessage);
        }

        [TestMethod]
        public void CreateCustomErrorWithHttpStatusErrorMessageResponseReturnsHttpResponse()
        {
            //Arrange
            var errorCode = 42;
            var errorMessage = "arbitrary-error-httpStatusErrorMessage";
            var statusErrorMessage = new HttpStatusErrorMessage(errorMessage);
            var exception = new Exception(errorMessage);
            var statusCode = HttpStatusCode.NotImplemented;

            var request = Mock.Create<HttpRequestMessage>(Behavior.RecursiveLoose, HttpMethod.Get, "http://localhost");
            var content = Mock.Create<HttpContent>();
            Mock.Arrange(() => request.Content).IgnoreInstance().Returns(content);
            var headers = Mock.Create<System.Net.Http.Headers.HttpRequestHeaders>();
            headers.Host = "localhost:80";

            Mock.Arrange(() => request.Headers).IgnoreInstance().Returns(headers);
            Mock.Arrange(() => request.Method).IgnoreInstance().Returns(HttpMethod.Get);
            Mock.Arrange(() => request.Properties).IgnoreInstance().Returns(new Dictionary<string, object>());
            Mock.Arrange(() => request.RequestUri).IgnoreInstance().Returns(new Uri("http://localhost"));
            Mock.Arrange(() => request.Version).IgnoreInstance().Returns(new Version(1, 0));

            //Act
            var response = request.CreateCustomErrorResponse(statusCode, statusErrorMessage);

            //Assert
            Assert.AreEqual(statusCode, response.StatusCode);
            Assert.IsTrue(response.Content.ToString().Contains(errorMessage));
            var httpStatusErrorMessage = JsonConvert.DeserializeObject<HttpStatusErrorMessage>(response.Content.ToString());
            Assert.IsNotNull(httpStatusErrorMessage);
        }
    }
}
