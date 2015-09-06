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
    public class HttpApplicationEvents : IHttpApplicationEvents
    {
        public void Application_Start(object sender, EventArgs e)
        {
            return;
        }

        public void Session_Start(object sender, EventArgs e)
        {
            return;
        }

        public void Application_BeginRequest(object sender, EventArgs e)
        {
            return;
        }

        public void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            return;
        }

        public void Application_Error(object sender, EventArgs e)
        {
            return;
        }

        public void Session_End(object sender, EventArgs e)
        {
            return;
        }

        public void Application_End(object sender, EventArgs e)
        {
            return;
        }

        public Version GetVersion()
        {
            var assembly = Assembly.GetExecutingAssembly();
            return assembly.GetName().Version;
        }
    }
}
