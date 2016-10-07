/**
 * Copyright 2014-2016 d-fens GmbH
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
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            //Trace.WriteLine(string.Format("classInitialize: '{0}'", testContext.TestName));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ToJsonNullThrowsArgumentNullException()
        {
            var deserialisedObject = JsonHelper.ToJson(null);
            Assert.Fail("This test was expected to fail already in a previous assertion.");
        }

        [TestMethod]
        public void ToJsonEmptyReturnsNull()
        {
            var deserialisedObject = JsonHelper.ToJson(string.Empty);
            Assert.IsNull(deserialisedObject);
        }

        [TestMethod]
        [ExpectedException(typeof(JsonReaderException))]
        public void ToJsonInvalidJsonStringThrowsJsonReaderException()
        {
            var deserialisedObject = JsonHelper.ToJson("invalid-json-string");
            Assert.Fail("This test was expected to fail already in a previous assertion.");
        }

        [TestMethod]
        public void ToJsonValidJsonStringReturnsObject()
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
            Assert.IsInstanceOfType(deserialisedObject["number"], typeof(long));
            Assert.AreEqual((long)42, deserialisedObject["number"]);

            Assert.IsTrue(deserialisedObject.ContainsKey("string"));
            Assert.IsInstanceOfType(deserialisedObject["string"], typeof(string));
            Assert.AreEqual("string", deserialisedObject["string"]);
        }

        [TestMethod]
        public void ConvertingToJsonGenericwithValidJsonStringReturnsTypedObject()
        {
            var jsonObjectGeneric = new JsonObjectGeneric()
            {
                String = "string"
                ,
                Number = 42
            };

            var serialisedObject = JsonHelper.ToJson<JsonObjectGeneric>(jsonObjectGeneric);
            Assert.IsNotNull(serialisedObject);

            var deserialisedObject = JsonHelper.ToJson<JsonObjectGeneric>(serialisedObject);

            Assert.IsNotNull(deserialisedObject);
            Assert.IsInstanceOfType(deserialisedObject, typeof(JsonObjectGeneric));

            Assert.AreEqual((long)42, deserialisedObject.Number);
            Assert.AreEqual("string", deserialisedObject.String);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ParseNullThrowsArgumentNullException()
        {
            var deserialisedObject = JsonHelper.Parse(null);
            Assert.Fail("This test was expected to fail already in a previous assertion.");
        }

        [TestMethod]
        [ExpectedException(typeof(JsonReaderException))]
        public void DoParseEmptyThrowsJsonReaderException()
        {
            var deserialisedObject = JsonHelper.Parse(string.Empty);
            Assert.Fail("This test was expected to fail already in a previous assertion.");
        }

        [TestMethod]
        public void ParseValidJsonStringReturnsObject()
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
        public void ParseInvalidJsonStringReturnsObject()
        {
            var jsonString = @"
                {
                    'number':  42

                    'string':  'string'
                }";
            var jsonObject = JsonHelper.Parse(jsonString);
        }

        [TestMethod]
        public void FromJsonJTokenReturnsString()
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

            var number = JsonHelper.FromJson(jsonObject, "number", defaultValue);
            Assert.AreEqual("42", number);
        }

        [TestMethod]
        public void FromJsonJTokenReturnsDefaultValue()
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
        public void EmptyStringReturnsObject()
        {
            var jsonString = JsonHelper.EmptyString();
            Assert.IsNotNull(jsonString);
            Assert.AreEqual("\"\"", jsonString);
        }

        [TestMethod]
        public void GettingStringValueOfExistingKeySucceeds()
        {
            var key = "string-key1";
            var value = "string-value1";
            var defaultValue = "default-string-value";
            var list = new List<string>
            {
                "item1", "item2", "item3"
            };

            var dic = new Dictionary<string, object>
            {
                {key, value},
                {"int-key2", 42},
                {"array-key3", list}
            };

            var result = JsonHelper.FromJson(dic, key, defaultValue);

            Assert.AreEqual(value, result);
        }

        [TestMethod]
        public void GettingDefaultStringValueOfInexistingKeySucceeds()
        {
            var key = "inexistent-string-key1";
            var value = "string-value1";
            var defaultValue = "default-string-value";
            var list = new List<string>
            {
                "item1", "item2", "item3"
            };

            var dic = new Dictionary<string, object>
            {
                {"string-key1", value},
                {"int-key2", 42},
                {"array-key3", list}
            };

            var result = JsonHelper.FromJson(dic, key, defaultValue);

            Assert.AreEqual(defaultValue, result);
        }

        [TestMethod]
        public void GettingIntValueOfExistingKeySucceeds()
        {
            var key = "int-key2";
            var value = 42;
            var defaultValue = 5;
            var list = new List<string>
            {
                "item1", "item2", "item3"
            };

            var dic = new Dictionary<string, object>
            {
                {"string-key1", "string-value"},
                {key, value},
                {"array-key3", list}
            };

            var result = JsonHelper.FromJson(dic, key, defaultValue.ToString());

            Assert.AreEqual(value.ToString(), result);
        }

        [TestMethod]
        public void GettingDefaultIntValueOfInexistingKeySucceeds()
        {
            var key = "inexistent-int-key2";
            var value = 42;
            var defaultValue = 5;
            var list = new List<string>
            {
                "item1", "item2", "item3"
            };

            var dic = new Dictionary<string, object>
            {
                {"string-key1", "string-value"},
                {"int-key2", value},
                {"array-key3", list}
            };

            var result = JsonHelper.FromJson(dic, key, defaultValue.ToString());

            Assert.AreEqual(defaultValue.ToString(), result);
        }

        [TestMethod]
        public void GettingArrayValueOfExistingKeySucceeds()
        {
            var key ="array-key3";
            var defaultValue = "default-string-value";
            var value = "item1";
            var list = new List<string>
            {
                value, "item2", "item3"
            };

            var dic = new Dictionary<string, object>
            {
                {"string-key1", "string-value1"},
                {"int-key2", 42},
                {key, list},
            };

            var result = JsonHelper.FromJson(dic, key, defaultValue);

            Assert.AreEqual(value, result);
        }

        [TestMethod]
        public void GettingDefaultArrayValueOfInexistingKeySucceeds()
        {
            var key = "inexistent-array-key3";
            var value = "item1";
            var defaultValue = "default-string-value";
            var list = new List<string>
            {
                value, "item2", "item3"
            };

            var dic = new Dictionary<string, object>
            {
                {"string-key1", "string-value1"},
                {"int-key2", 42},
                {"array-key3", list},
            };

            var result = JsonHelper.FromJson(dic, key, defaultValue);

            Assert.AreEqual(defaultValue, result);
        }

    }
}
