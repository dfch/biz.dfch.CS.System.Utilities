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
using biz.dfch.CS.System.Utilities.Rest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTestExtensions;

namespace biz.dfch.CS.System.Utilities.Tests
{
    [TestClass]
    public class EnumUtilsTest
    {
        [TestMethod]
        public void ParseForValidValueIgnoringCaseReturnsCorrespondingEnumValue()
        {
            Assert.AreEqual(ContentType.ApplicationJson, EnumUtil.Parse<ContentType>("ApplicationJson"));
            Assert.AreEqual(ContentType.ApplicationJson, EnumUtil.Parse<ContentType>("APPLICATIONjson"));
        }

        [TestMethod]
        public void ParseForValidValueNotIgnoringCaseReturnsCorrespondingEnumValueIfCasesMatch()
        {
            Assert.AreEqual(ContentType.ApplicationJson, EnumUtil.Parse<ContentType>("ApplicationJson", false));
        }

        [TestMethod]
        public void ParseWithNonCaseMatchingValueNotIgnoringCaseThrowsArgumentException()
        {
            ThrowsAssert.Throws<ArgumentException>(() => EnumUtil.Parse<ContentType>("aPPlicationJson", false),
                "Requested value 'aPPlicationJson' was not found.");
        }

        [TestMethod]
        public void ParseWithInvalidValueThrowsArgumentException()
        {
            ThrowsAssert.Throws<ArgumentException>(() => EnumUtil.Parse<ContentType>("test", false),
                "Requested value 'test' was not found.");
        }
    }
}
