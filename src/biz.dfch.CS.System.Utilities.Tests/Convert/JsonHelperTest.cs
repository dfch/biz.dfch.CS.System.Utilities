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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using biz.dfch.CS.Utilities.Convert;
using System.Diagnostics;

namespace biz.dfch.CS.Utilities.Tests.Convert
{
    [TestClass]
    public class JsonHelperTest
    {
        [ClassInitialize()]
        public static void ClassInitialize(TestContext testContext)
        {
            Trace.WriteLine(String.Format("classInitialize: '{0}'", testContext.TestName));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void doToJsonNullThrowsArgumentNullException()
        {
            var deserialisedObject = JsonHelper.ToJson(null);
            Assert.Fail("This test was expected to fail already in a previous assertion.");
        }

        [TestMethod]
        public void doToJsonEmptyReturnsNull()
        {
            var deserialisedObject = JsonHelper.ToJson(String.Empty);
            Assert.IsNull(deserialisedObject);
        }

        [TestMethod]
        [ExpectedException(typeof(JsonReaderException))]
        public void doToJsonInvalidJsonStringThrowsJsonReaderException()
        {
            var deserialisedObject = JsonHelper.ToJson("invalid-json-string");
            Assert.Fail("This test was expected to fail already in a previous assertion.");
        }

        [TestMethod]
        public void doToJsonValidJsonStringReturnsObject()
        {
            var jsonString = @"
                {
                    'number':  42
                    ,
                    'string':  'string'
                }";
            var deserialisedObject = JsonHelper.ToJson(jsonString);
            Assert.IsNotNull(deserialisedObject);

            Assert.IsTrue(deserialisedObject.ContainsKey("number"));
            Assert.IsInstanceOfType(deserialisedObject["number"], typeof(Int64));
            Assert.AreEqual((Int64)42, deserialisedObject["number"]);

            Assert.IsTrue(deserialisedObject.ContainsKey("string"));
            Assert.IsInstanceOfType(deserialisedObject["string"], typeof(String));
            Assert.AreEqual("string", deserialisedObject["string"]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void doParseNullThrowsArgumentNullException()
        {
            var deserialisedObject = JsonHelper.Parse(null);
            Assert.Fail("This test was expected to fail already in a previous assertion.");
        }

        [TestMethod]
        [ExpectedException(typeof(JsonReaderException))]
        public void doParseEmptyThrowsJsonReaderException()
        {
            var deserialisedObject = JsonHelper.Parse(String.Empty);
            Assert.Fail("This test was expected to fail already in a previous assertion.");
        }

        [TestMethod]
        public void doParseValidJsonStringReturnsObject()
        {
            var jsonString = @"
                {
                    'number':  42
                    ,
                    'string':  'string'
                }";
            var jsonObject = JsonHelper.Parse(jsonString);
            Assert.IsNotNull(jsonObject);

            var jtNumber = jsonObject.SelectToken("number", true);
            Assert.IsNotNull(jtNumber);
            Assert.AreEqual(42, jtNumber.Value<int>());

            var jtString = jsonObject.SelectToken("string", true);
            Assert.IsNotNull(jtString);
            Assert.AreEqual("string", jtString.Value<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(JsonReaderException))]
        public void doParseInvalidJsonStringReturnsObject()
        {
            var jsonString = @"
                {
                    'number':  42

                    'string':  'string'
                }";
            var jsonObject = JsonHelper.Parse(jsonString);
        }

        [TestMethod]
        public void doFromJsonJTokenReturnsString()
        {
            var defaultValue = "<default value>";
            var jsonString = @"
                {
                    'number':  42
                    ,
                    'string':  'string'
                }";
            var jsonObject = JsonHelper.Parse(jsonString);
            Assert.IsNotNull(jsonObject);

            var _string = JsonHelper.FromJson(jsonObject, "string", defaultValue);
            Assert.AreEqual("string", _string);

            var _number = JsonHelper.FromJson(jsonObject, "number", defaultValue);
            Assert.AreEqual("42", _number);
        }

        [TestMethod]
        public void doFromJsonJTokenReturnsDefaultValue()
        {
            var defaultValue = "<default value>";
            var jsonString = @"
                {
                    'number':  42
                    ,
                    'string':  'string'
                }";
            var jsonObject = JsonHelper.Parse(jsonString);
            Assert.IsNotNull(jsonObject);

            var _string = JsonHelper.FromJson(jsonObject, "string-does-not-exist", defaultValue);
            Assert.AreEqual(defaultValue, _string);
        }

        [TestMethod]
        public void doEmptyStringReturnsObject()
        {
            var jsonString = JsonHelper.EmptyString();
            Assert.IsNotNull(jsonString);
            Assert.AreEqual("\"\"", jsonString);
        }
    }
}
