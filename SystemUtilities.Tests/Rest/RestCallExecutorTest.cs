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

using System;
using System.Collections.Generic;
using System.Net;
using biz.dfch.CS.System.Utilities.Rest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Net.Http.Headers;
using biz.dfch.CS.System.Utilities.Attributes;
using MSTestExtensions;
using Telerik.JustMock;
using HttpMethod = biz.dfch.CS.System.Utilities.Rest.HttpMethod;

namespace biz.dfch.CS.System.Utilities.Tests.Rest
{
    [TestClass]
    public class RestCallExecutorTest
    {
        private const String URI = "http://test/api/entities";
        private const String CONTENT_TYPE_KEY = "Content-Type";
        private const String USER_AGENT_KEY = "User-Agent";
        private const String AUTHORIZATION_HEADER_KEY = "Authorization";
        private const String TEST_USER_AGENT = "test-agent";
        private const String SAMPLE_REQUEST_BODY = "{\"Property\":\"value\"}";
        private const String SAMPLE_RESPONSE_BODY = "{\"Property2\":\"value2\"}";
        private const String BEARER_AUTH_SCHEME = "Bearer";
        private const String SAMPLE_BEARER_TOKEN = "AbCdEf123456";

        private HttpClient _httpClient;

        [TestInitialize]
        public void TestInitilize()
        {
            _httpClient = Mock.Create<HttpClient>();
        }

        [TestMethod]
        public void RestCallExecutorConstructorSetsProperties()
        {
            var restCallExecutor = new RestCallExecutor();
            Assert.AreEqual(true, restCallExecutor.EnsureSuccessStatusCode);
            Assert.AreEqual(ContentType.ApplicationJson, restCallExecutor.ContentType);
            Assert.AreEqual(90, restCallExecutor.Timeout);
        }

        [TestMethod]
        public void RestCallExecutorConstructorSetsEnsureSuccessCodePropertyAccordingConstructorParameter()
        {
            var restCallExecutor = new RestCallExecutor(false);
            Assert.AreEqual(false, restCallExecutor.EnsureSuccessStatusCode);
            Assert.AreEqual(ContentType.ApplicationJson, restCallExecutor.ContentType);
            Assert.AreEqual(90, restCallExecutor.Timeout);
        }

        [TestMethod]
        public void InvokeWithInvalidUriThrowsUriFormatException()
        {
            var invalidUri = "abc";
            RestCallExecutor restCallExecutor = new RestCallExecutor();
            ThrowsAssert.Throws<UriFormatException>(() => restCallExecutor.Invoke(invalidUri));
            ThrowsAssert.Throws<UriFormatException>(() => restCallExecutor.Invoke(invalidUri, null));
            ThrowsAssert.Throws<UriFormatException>(() => restCallExecutor.Invoke(HttpMethod.Head, invalidUri, null, null));
        }

        [TestMethod]
        public void InvokeWithNullUriThrowsArgumentException()
        {
            String nullUri = null;
            RestCallExecutor restCallExecutor = new RestCallExecutor();
            ThrowsAssert.Throws<ArgumentException>(() => restCallExecutor.Invoke(nullUri));
            ThrowsAssert.Throws<ArgumentException>(() => restCallExecutor.Invoke(nullUri, null));
            ThrowsAssert.Throws<ArgumentException>(() => restCallExecutor.Invoke(HttpMethod.Head, nullUri, null, null));
        }

        [TestMethod]
        public void InvokeWithWhitespaceUriThrowsArgumentException()
        {
            var whitespaceUri = " ";
            RestCallExecutor restCallExecutor = new RestCallExecutor();
            ThrowsAssert.Throws<ArgumentException>(() => restCallExecutor.Invoke(whitespaceUri));
            ThrowsAssert.Throws<ArgumentException>(() => restCallExecutor.Invoke(whitespaceUri, null));
            ThrowsAssert.Throws<ArgumentException>(() => restCallExecutor.Invoke(HttpMethod.Head, whitespaceUri, null, null));
        }

        [TestMethod]
        public void InvokeGetExecutesGetRequestOnUriWithProvidedHeadersEnsuresSuccessStatusCodeAndReturnsResponseContent()
        {
            var mockedResponseMessage = Mock.Create<HttpResponseMessage>();

            Mock.Arrange(() => _httpClient.DefaultRequestHeaders.Add(USER_AGENT_KEY, TEST_USER_AGENT))
                .IgnoreInstance()
                .OccursOnce();

            Mock.Arrange(() => _httpClient.DefaultRequestHeaders.Add("MyHeader", "a value"))
                .IgnoreInstance()
                .OccursOnce();

            Mock.Arrange(() => _httpClient.GetAsync(Arg.Is(new Uri(URI))).Result)
                .IgnoreInstance()
                .Returns(mockedResponseMessage)
                .OccursOnce();

            Mock.Arrange(() => mockedResponseMessage.EnsureSuccessStatusCode())
                .OccursOnce();

            Mock.Arrange(() => mockedResponseMessage.Content.ReadAsStringAsync().Result)
                .Returns(SAMPLE_RESPONSE_BODY)
                .OccursOnce();

            RestCallExecutor restCallExecutor = new RestCallExecutor();
            Assert.AreEqual(SAMPLE_RESPONSE_BODY, restCallExecutor.Invoke(URI, CreateSampleHeaders()));
            Assert.AreEqual(ContentType.ApplicationXml, restCallExecutor.ContentType);

            Mock.Assert(_httpClient);
            Mock.Assert(mockedResponseMessage);
        }

        [TestMethod]
        public void InvokeGetExecutesGetRequestOnUriWithProvidedHeadersNotEnsuringSuccessStatusCodeReturnsResponseContent()
        {
            var mockedResponseMessage = Mock.Create<HttpResponseMessage>();

            Mock.Arrange(() => _httpClient.DefaultRequestHeaders.Add(USER_AGENT_KEY, TEST_USER_AGENT))
                .IgnoreInstance()
                .OccursOnce();

            Mock.Arrange(() => _httpClient.DefaultRequestHeaders.Add("MyHeader", "a value"))
                .IgnoreInstance()
                .OccursOnce();

            Mock.Arrange(() => _httpClient.GetAsync(Arg.Is(new Uri(URI))).Result)
                .IgnoreInstance()
                .Returns(mockedResponseMessage)
                .OccursOnce();

            Mock.Arrange(() => mockedResponseMessage.Content.ReadAsStringAsync().Result)
                .Returns(SAMPLE_RESPONSE_BODY)
                .OccursOnce();

            RestCallExecutor restCallExecutor = new RestCallExecutor(false);
            Assert.AreEqual(SAMPLE_RESPONSE_BODY, restCallExecutor.Invoke(URI, CreateSampleHeaders()));

            Mock.Assert(_httpClient);
            Mock.Assert(mockedResponseMessage);
        }

        [TestMethod]
        public void InvokeExecutesThrowsHttpRequestExceptionIfEnsureSuccessStatusCodeFails()
        {
            var mockedResponseMessage = Mock.Create<HttpResponseMessage>();

            Mock.Arrange(() => _httpClient.DefaultRequestHeaders.Add(USER_AGENT_KEY, TEST_USER_AGENT))
                .IgnoreInstance()
                .OccursOnce();

            Mock.Arrange(() => _httpClient.DefaultRequestHeaders.Add("MyHeader", "a value"))
                .IgnoreInstance()
                .OccursOnce();

            Mock.Arrange(() => _httpClient.GetAsync(Arg.Is(new Uri(URI))).Result)
                .IgnoreInstance()
                .Returns(mockedResponseMessage)
                .OccursOnce();

            Mock.Arrange(() => mockedResponseMessage.EnsureSuccessStatusCode())
                .Throws<HttpRequestException>()
                .OccursOnce();

            RestCallExecutor restCallExecutor = new RestCallExecutor();
            ThrowsAssert.Throws<HttpRequestException>(() => restCallExecutor.Invoke(URI, CreateSampleHeaders()));

            Mock.Assert(_httpClient);
            Mock.Assert(mockedResponseMessage);
        }

        [TestMethod]
        public void InvokeSetsDefaultUserAgentHeaderIfNoCustomUserAgentHeaderProvided()
        {
            var mockedResponseMessage = Mock.Create<HttpResponseMessage>();

            Mock.Arrange(() => _httpClient.DefaultRequestHeaders.Add(USER_AGENT_KEY, "RestCallExecutor"))
                .IgnoreInstance()
                .OccursOnce();

            Mock.Arrange(() => _httpClient.GetAsync(Arg.Is(new Uri(URI))).Result)
                .IgnoreInstance()
                .Returns(mockedResponseMessage)
                .OccursOnce();

            Mock.Arrange(() => mockedResponseMessage.EnsureSuccessStatusCode())
                .OccursOnce();

            Mock.Arrange(() => mockedResponseMessage.Content.ReadAsStringAsync().Result)
                .Returns(SAMPLE_RESPONSE_BODY)
                .OccursOnce();

            RestCallExecutor restCallExecutor = new RestCallExecutor();
            Assert.AreEqual(SAMPLE_RESPONSE_BODY, restCallExecutor.Invoke(URI));

            Mock.Assert(_httpClient);
            Mock.Assert(mockedResponseMessage);
        }

        [TestMethod]
        public void InvokeHeadExecutesHeadRequestOnUriWithProvidedHeadersEnsuresSuccessStatusCodeAndReturnsResponseContent()
        {
            var mockedResponseMessage = Mock.Create<HttpResponseMessage>();

            Mock.Arrange(() => _httpClient.DefaultRequestHeaders.Add(USER_AGENT_KEY, TEST_USER_AGENT))
                .IgnoreInstance()
                .OccursOnce();

            Mock.Arrange(() => _httpClient.DefaultRequestHeaders.Add("MyHeader", "a value"))
                .IgnoreInstance()
                .OccursOnce();

            Mock.Arrange(() => _httpClient.GetAsync(Arg.Is(new Uri(URI))).Result)
                .IgnoreInstance()
                .Returns(mockedResponseMessage)
                .OccursOnce();

            Mock.Arrange(() => mockedResponseMessage.EnsureSuccessStatusCode())
                .OccursOnce();

            Mock.Arrange(() => mockedResponseMessage.Content.ReadAsStringAsync().Result)
                .Returns(SAMPLE_RESPONSE_BODY)
                .OccursOnce();

            RestCallExecutor restCallExecutor = new RestCallExecutor();
            Assert.AreEqual(SAMPLE_RESPONSE_BODY, restCallExecutor.Invoke(HttpMethod.Get, URI, CreateSampleHeaders(), null));
            Assert.AreEqual(ContentType.ApplicationXml, restCallExecutor.ContentType);

            Mock.Assert(_httpClient);
            Mock.Assert(mockedResponseMessage);
        }

        [TestMethod]
        public void InvokePostExecutesPostRequestOnUriWithProvidedHeadersAndBodyEnsuresSuccessStatusCodeAndReturnsResponseContent()
        {
            var mockedResponseMessage = Mock.Create<HttpResponseMessage>();

            Mock.Arrange(() => _httpClient.DefaultRequestHeaders.Add(USER_AGENT_KEY, TEST_USER_AGENT))
                .IgnoreInstance()
                .OccursOnce();

            Mock.Arrange(() => _httpClient.DefaultRequestHeaders.Add("MyHeader", "a value"))
                .IgnoreInstance()
                .OccursOnce();

            HttpContent content = new StringContent(SAMPLE_REQUEST_BODY);
            content.Headers.ContentType = new MediaTypeHeaderValue(ContentType.ApplicationXml.GetStringValue());
            Mock.Arrange(() => _httpClient.PostAsync(Arg.Is(new Uri(URI)), Arg.Is(content)).Result)
                .IgnoreInstance()
                .Returns(mockedResponseMessage)
                .OccursOnce();

            Mock.Arrange(() => mockedResponseMessage.EnsureSuccessStatusCode())
                .OccursOnce();

            Mock.Arrange(() => mockedResponseMessage.Content.ReadAsStringAsync().Result)
                .Returns(SAMPLE_RESPONSE_BODY)
                .OccursOnce();

            RestCallExecutor restCallExecutor = new RestCallExecutor();
            Assert.AreEqual(SAMPLE_RESPONSE_BODY, restCallExecutor.Invoke(HttpMethod.Post, URI, CreateSampleHeaders(), SAMPLE_REQUEST_BODY));
            Assert.AreEqual(ContentType.ApplicationXml, restCallExecutor.ContentType);

            Mock.Assert(_httpClient);
            Mock.Assert(mockedResponseMessage);
        }

        [TestMethod]
        public void InvokeDeleteExecutesDeleteRequestOnUriWithProvidedHeadersAndBodyEnsuresSuccessStatusCodeAndReturnsResponseContent()
        {
            var mockedResponseMessage = Mock.Create<HttpResponseMessage>();

            Mock.Arrange(() => _httpClient.DefaultRequestHeaders.Add(USER_AGENT_KEY, TEST_USER_AGENT))
                .IgnoreInstance()
                .OccursOnce();

            Mock.Arrange(() => _httpClient.DefaultRequestHeaders.Add("MyHeader", "a value"))
                .IgnoreInstance()
                .OccursOnce();

            Mock.Arrange(() => _httpClient.DeleteAsync(Arg.Is(new Uri(URI))).Result)
                .IgnoreInstance()
                .Returns(mockedResponseMessage)
                .OccursOnce();

            Mock.Arrange(() => mockedResponseMessage.EnsureSuccessStatusCode())
                .OccursOnce();

            Mock.Arrange(() => mockedResponseMessage.Content.ReadAsStringAsync().Result)
                .Returns(SAMPLE_RESPONSE_BODY)
                .OccursOnce();

            RestCallExecutor restCallExecutor = new RestCallExecutor();
            Assert.AreEqual(SAMPLE_RESPONSE_BODY, restCallExecutor.Invoke(HttpMethod.Delete, URI, CreateSampleHeaders(), SAMPLE_REQUEST_BODY));
            Assert.AreEqual(ContentType.ApplicationXml, restCallExecutor.ContentType);

            Mock.Assert(_httpClient);
            Mock.Assert(mockedResponseMessage);
        }

        [TestMethod]
        public void InvokePutExecutesPutRequestOnUriWithProvidedHeadersAndBodyEnsuresSuccessStatusCodeAndReturnsResponseContent()
        {
            var mockedResponseMessage = Mock.Create<HttpResponseMessage>();

            Mock.Arrange(() => _httpClient.DefaultRequestHeaders.Add(USER_AGENT_KEY, TEST_USER_AGENT))
                .IgnoreInstance()
                .OccursOnce();

            Mock.Arrange(() => _httpClient.DefaultRequestHeaders.Add("MyHeader", "a value"))
                .IgnoreInstance()
                .OccursOnce();

            HttpContent content = new StringContent(SAMPLE_REQUEST_BODY);
            content.Headers.ContentType = new MediaTypeHeaderValue(ContentType.ApplicationXml.GetStringValue());
            Mock.Arrange(() => _httpClient.PutAsync(Arg.Is(new Uri(URI)), Arg.Is(content)).Result)
                .IgnoreInstance()
                .Returns(mockedResponseMessage)
                .OccursOnce();

            Mock.Arrange(() => mockedResponseMessage.EnsureSuccessStatusCode())
                .OccursOnce();

            Mock.Arrange(() => mockedResponseMessage.Content.ReadAsStringAsync().Result)
                .Returns(SAMPLE_RESPONSE_BODY)
                .OccursOnce();

            RestCallExecutor restCallExecutor = new RestCallExecutor();
            Assert.AreEqual(SAMPLE_RESPONSE_BODY, restCallExecutor.Invoke(HttpMethod.Put, URI, CreateSampleHeaders(), SAMPLE_REQUEST_BODY));
            Assert.AreEqual(ContentType.ApplicationXml, restCallExecutor.ContentType);

            Mock.Assert(_httpClient);
            Mock.Assert(mockedResponseMessage);
        }

        [TestMethod]
        public void InvokeWhenAuthSchemeIsSetSetsAuthorizationHeaderAccordingAuthScheme()
        {
            var mockedResponseMessage = Mock.Create<HttpResponseMessage>();

            Mock.Arrange(() => _httpClient.DefaultRequestHeaders.Add(USER_AGENT_KEY, TEST_USER_AGENT))
                .IgnoreInstance()
                .OccursOnce();

            Mock.Arrange(() => new AuthenticationHeaderValue(Arg.Is(BEARER_AUTH_SCHEME), Arg.Is(SAMPLE_BEARER_TOKEN)))
                .OccursOnce();

            Mock.Arrange(() => _httpClient.GetAsync(Arg.Is(new Uri(URI))).Result)
                .IgnoreInstance()
                .Returns(mockedResponseMessage)
                .OccursOnce();

            Mock.Arrange(() => mockedResponseMessage.EnsureSuccessStatusCode())
                .OccursOnce();

            Mock.Arrange(() => mockedResponseMessage.Content.ReadAsStringAsync().Result)
                .Returns(SAMPLE_RESPONSE_BODY)
                .OccursOnce();

            RestCallExecutor restCallExecutor = new RestCallExecutor();
            restCallExecutor.AuthScheme = BEARER_AUTH_SCHEME;
            var headers = CreateSampleHeaders();
            headers.Add(AUTHORIZATION_HEADER_KEY, SAMPLE_BEARER_TOKEN);
            Assert.AreEqual(SAMPLE_RESPONSE_BODY, restCallExecutor.Invoke(HttpMethod.Get, URI, headers, null));

            Mock.Assert(_httpClient);
            Mock.Assert(mockedResponseMessage);
        }

        [TestMethod]
        public void InvokeSetsAuthorizationHeaderAccordingHeadersDictionaryWhenAuthSchemeIsNotSet()
        {
            var mockedResponseMessage = Mock.Create<HttpResponseMessage>();

            Mock.Arrange(() => _httpClient.DefaultRequestHeaders.Add(USER_AGENT_KEY, TEST_USER_AGENT))
                .IgnoreInstance()
                .OccursOnce();

            Mock.Arrange(() => new AuthenticationHeaderValue(Arg.Is(BEARER_AUTH_SCHEME), Arg.Is(SAMPLE_BEARER_TOKEN)))
                .OccursNever();

            Mock.Arrange(() => _httpClient.DefaultRequestHeaders.Add(AUTHORIZATION_HEADER_KEY, SAMPLE_BEARER_TOKEN))
                .IgnoreInstance()
                .OccursOnce();

            Mock.Arrange(() => _httpClient.GetAsync(Arg.Is(new Uri(URI))).Result)
                .IgnoreInstance()
                .Returns(mockedResponseMessage)
                .OccursOnce();

            Mock.Arrange(() => mockedResponseMessage.EnsureSuccessStatusCode())
                .OccursOnce();

            Mock.Arrange(() => mockedResponseMessage.Content.ReadAsStringAsync().Result)
                .Returns(SAMPLE_RESPONSE_BODY)
                .OccursOnce();

            RestCallExecutor restCallExecutor = new RestCallExecutor();
            var headers = CreateSampleHeaders();
            headers.Add(AUTHORIZATION_HEADER_KEY, SAMPLE_BEARER_TOKEN);
            Assert.AreEqual(SAMPLE_RESPONSE_BODY, restCallExecutor.Invoke(HttpMethod.Get, URI, headers, null));

            Mock.Assert(_httpClient);
            Mock.Assert(mockedResponseMessage);
        }

        private IDictionary<String, String> CreateSampleHeaders()
        {
            var headers = new Dictionary<String, String>();
            headers.Add(CONTENT_TYPE_KEY, ContentType.ApplicationXml.ToString());
            headers.Add(USER_AGENT_KEY, TEST_USER_AGENT);
            headers.Add("MyHeader",  "a value");
            return headers;
        }
    }
}
