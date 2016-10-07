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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using biz.dfch.CS.Utilities.General;

namespace biz.dfch.CS.Utilities.Tests.General
{
    [TestClass]
    public class MethodTest
    {
        [TestMethod]
        public void GetNameReturnsMethodName()
        {
            //Arrange
            var exptectedMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            //Act
            var result = Method.GetName();

            //Assert
            Assert.AreEqual(exptectedMethodName, result);
        }

        [TestMethod]
        public void fnReturnsMethodName()
        {
            //Arrange
            var exptectedMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            //Act
            var result = Method.fn();

            //Assert
            Assert.AreEqual(exptectedMethodName, result);
        }

        [TestMethod]
        public void GetFilePathReturnsMethodFilePath()
        {
            //Arrange
            // file name is hard coded - change it here in case you are refactoring the actual source code file
            var exptectedFilePath = "MethodTest.cs";

            //Act
            var result = Method.GetFilePath();

            //Assert
            Assert.IsTrue(result.EndsWith(exptectedFilePath));
        }

        [TestMethod]
        public void GetLineNumberReturnsMethodLineNumber()
        {
            //Arrange
            // file number is hard coded - change it here in case you are changing the actual source code file
            var exptectedLineNumber = 74;

            //Act
            var result = Method.GetLineNumber();    // < -- this line must match the defined number in exptectedLineNumber

            //Assert
            Assert.AreEqual(exptectedLineNumber, result);
        }
    }
}
