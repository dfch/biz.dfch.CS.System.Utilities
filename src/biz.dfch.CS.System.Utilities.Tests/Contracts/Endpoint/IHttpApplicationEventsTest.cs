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
﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace biz.dfch.CS.Utilities.Tests.Contracts.Endpoint
{
    [TestClass]
    public class IHttpApplicationEventsTest
    {
        private readonly HttpApplicationEvents httpApplicationEvents = new HttpApplicationEvents();
        private readonly object sender = new Object();
        private readonly EventArgs e = new EventArgs();

        [TestMethod]
        public void VersionReturnsAssemblyVersion()
        {
            // this version is encoded in Assembly.cs as assembly version
            // file and product version should match assembly version or left
            // undefined (0.0.0.0) so they would stay equal to assembly version
            var expectedVersion = new Version(4200, 4201, 4202, 4203);

            var result = httpApplicationEvents.GetVersion();

            Assert.AreEqual(expectedVersion, result);
        }

        // These tests are only to fix the behaviour of the interface
        [TestMethod]
        public void Application_Start()
        {
            httpApplicationEvents.Application_Start(sender, e);
        }

        [TestMethod]
        public void Application_End()
        {
            httpApplicationEvents.Application_End(sender, e);
        }

        [TestMethod]
        public void Session_Start()
        {
            httpApplicationEvents.Session_Start(sender, e);
        }

        [TestMethod]
        public void Session_End()
        {
            httpApplicationEvents.Session_End(sender, e);
        }

        [TestMethod]
        public void Application_BeginRequest()
        {
            httpApplicationEvents.Application_BeginRequest(sender, e);
        }

        [TestMethod]
        public void Application_AuthenticateRequest()
        {
            httpApplicationEvents.Application_AuthenticateRequest(sender, e);
        }

        [TestMethod]
        public void Application_Error()
        {
            httpApplicationEvents.Application_Error(sender, e);
        }
    }
}
