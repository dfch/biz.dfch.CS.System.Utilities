/**
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
 */

using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using biz.dfch.CS.Utilities.Contracts.Endpoint;

namespace biz.dfch.CS.Utilities.Tests.Contracts.Endpoint
{
    [TestClass]
    public class ServerRoleTest
    {
        public ServerRoleTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void ServerRoleHasHostRoleSucceeds()
        {
            // if this test fails then we have a breaking change.
            // then the nuget version has to be upgraded by a major version number
            // see http://semver.org/ for details

            // Arrange
            ServerRole serverRole;

            // Act
            serverRole = ServerRole.HOST;

            // Assert
            Assert.AreEqual(ServerRole.HOST, serverRole);
        }

        [TestMethod]
        public void ServerRoleHasWorkerRoleSucceeds()
        {
            // if this test fails then we have a breaking change.
            // then the nuget version has to be upgraded by a major version number
            // see http://semver.org/ for details

            // Arrange
            ServerRole serverRole;

            // Act
            serverRole = ServerRole.WORKER;

            // Assert
            Assert.AreEqual(ServerRole.WORKER, serverRole);
        }
    }
}
