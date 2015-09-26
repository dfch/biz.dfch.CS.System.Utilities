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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using biz.dfch.CS.Utilities.Testing;

namespace biz.dfch.CS.Utilities.Tests.Testing
{
    [TestClass]
    public class ExpectContractFailureAttributeTest
    {
        [TestMethod]
        public void RunningTestWithTrueSucceeds()
        {
            var sut = new ArbitrarySubjectUnderTest();

            var result = sut.CallingMeWithTrueReturns42ThrowsContractExceptionOtherwise(true);

            Assert.AreEqual(42, result);
        }

        // DFTODO -- @rufer7 - TeamCity build fails on this test
        // maybe because CodeContract extensions are not installed? pls investigate
        [Ignore]
        [TestMethod]
        [ExpectContractFailureAttribute]
        public void RunningTestWithFalseThrowsCodeContractException()
        {
            var sut = new ArbitrarySubjectUnderTest();

            var result = sut.CallingMeWithTrueReturns42ThrowsContractExceptionOtherwise(false);

            Assert.AreEqual(42, result);
        }
    }
}
