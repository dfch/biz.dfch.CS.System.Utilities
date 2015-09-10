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
using System.Net.Http;
using System.Net;
using biz.dfch.CS.Utilities.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telerik.JustMock;

namespace biz.dfch.CS.Utilities.Tests.Http
{
    [TestClass]
    public class HttpRequestMessageExtensionTest
    {
        [TestMethod]
        public void CreateCustomErrorWithStringResponseReturnsHttpResponse()
        {
            //Arrange
            var errorCode = 42;
            var errorMessage = "arbitrary-error-httpStatusErrorMessage";
            var statusCode = HttpStatusCode.NotImplemented;
            var request = new HttpRequestMessage();

            Mock.Arrange(() => request.CreateResponse(Arg.IsAny<HttpStatusCode>(), Arg.AnyString))
                .Returns(new HttpResponseMessage(statusCode))
                .OccursOnce();
            
            //Act
            var response = request.CreateCustomErrorResponse(statusCode, errorMessage, errorCode);

            //Assert
            Mock.Assert(request);
            Assert.AreEqual(statusCode, response.StatusCode);
        }

        [TestMethod]
        public void CreateCustomErrorWithExceptionResponseReturnsHttpResponse()
        {
            //Arrange
            var errorCode = 42;
            var errorMessage = "arbitrary-error-httpStatusErrorMessage";
            var exception = new Exception(errorMessage);
            var statusCode = HttpStatusCode.NotImplemented;
            var request = new HttpRequestMessage();

            Mock.Arrange(() => request.CreateResponse(Arg.IsAny<HttpStatusCode>(), Arg.AnyString))
                .Returns(new HttpResponseMessage(statusCode))
                .OccursOnce();
            
            //Act
            var response = request.CreateCustomErrorResponse(statusCode, exception, errorCode);

            //Assert
            Mock.Assert(request);
            Assert.AreEqual(statusCode, response.StatusCode);
        }

        [TestMethod]
        public void CreateCustomErrorWithHttpStatusErrorMessageResponseReturnsHttpResponse()
        {
            //Arrange
            var errorMessage = "arbitrary-error-httpStatusErrorMessage";
            var statusErrorMessage = new HttpStatusErrorMessage(errorMessage);
            var statusCode = HttpStatusCode.NotImplemented;
            var request = new HttpRequestMessage();

            //Mock.Arrange(() => request.CreateResponse()).Returns(new HttpResponseMessage(statusCode)).OccursOnce();
            //Mock.Arrange(() => request.CreateResponse(Arg.IsAny<HttpStatusCode>())).Returns(new HttpResponseMessage(statusCode)).OccursOnce();
            Mock.Arrange(() => request.CreateResponse(Arg.IsAny<HttpStatusCode>(), Arg.AnyString)).Returns(new HttpResponseMessage(statusCode)).OccursOnce();

            //Act
            var response = request.CreateCustomErrorResponse(statusCode, statusErrorMessage);

            //Assert
            Mock.Assert(request);
            Assert.AreEqual(statusCode, response.StatusCode);
        }
    }
}
