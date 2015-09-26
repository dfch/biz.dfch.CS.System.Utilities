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
using System;
﻿using biz.dfch.CS.Utilities.Contracts.Endpoint;
﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace biz.dfch.CS.Utilities.Tests.Contracts.Endpoint
{
    [TestClass]
    public class IODataEndpointDataTest
    {
        [TestMethod]
        public void DefaultServerRoleRetunsHOST()
        {
            // Arrange
            var exptectedServerRole = ServerRole.HOST;
            var endpointData = new ODataEndpointDataWithServerRole();

            // Act
            var result = endpointData.ServerRole;

            // Assert
            Assert.AreEqual(exptectedServerRole, result);
        }

        [TestMethod]
        public void ServerRoleReturnsName()
        {
            // Arrange
            var exptectedServerRole = ServerRole.WORKER;
            var endpointData = new ODataEndpointDataWithServerRole(exptectedServerRole);

            // Act
            var result = endpointData.ServerRole;

            // Assert
            Assert.AreEqual(exptectedServerRole, result);
        }

        [TestMethod]
        public void DefaultPriorityReturnsZero()
        {
            // Arrange
            var exptectedPriority = 0;
            var endpointData = new ODataEndpointDataWithPriority();

            // Act
            var result = endpointData.Priority;
            
            // Assert
            Assert.AreEqual(exptectedPriority, result);
        }

        [TestMethod]
        public void GetPriorityReturnsPriority()
        {
            // Arrange
            var exptectedPriority = 42;
            var endpointData = new ODataEndpointDataWithPriority(exptectedPriority);
            
            // Act
            var result = endpointData.Priority;

            // Assert
            Assert.AreEqual(exptectedPriority, result);
        }
    }
}
