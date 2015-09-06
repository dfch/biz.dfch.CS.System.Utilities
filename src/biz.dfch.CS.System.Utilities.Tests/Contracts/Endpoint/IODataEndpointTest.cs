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

﻿using System;
﻿using biz.dfch.CS.Utilities.Contracts.Endpoint;
﻿using Microsoft.Data.Edm;
﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;

namespace biz.dfch.CS.Utilities.Tests.Contracts.Endpoint
{
    [TestClass]
    public class IODataEndpointTest : IODataEndpoint
    {
        public IEdmModel GetModel()
        {
            throw new NotImplementedException();
        }

        private readonly String containerName = "myContainer";
        public string GetContainerName()
        {
            return containerName;
        }

        [TestMethod]
        public void GetContainerNameReturnsName()
        {
            Assert.AreEqual(containerName, this.GetContainerName());
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void GetModelThrowsNotImplementedException()
        {
            this.GetModel();
            Assert.Fail("Exception expected, but no exception has been thrown.");
        }

        public Version GetVersion()
        {
            var assembly = Assembly.GetExecutingAssembly();
            return assembly.GetName().Version;
        }

        [TestMethod]
        public void VersionReturnsAssemblyVersion()
        {
            // this version is encoded in Assembly.cs as assembly version
            // file and product version should match assembly version or left
            // undefined (0.0.0.0) so they would stay equal to assembly version
            var expectedVersion = new Version(4200, 4201, 4202, 4203);

            var version = this.GetVersion();
            
            Assert.AreEqual(expectedVersion, version);
        }
    }
}
