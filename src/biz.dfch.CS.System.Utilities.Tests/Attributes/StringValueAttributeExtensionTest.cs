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
 
﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
﻿using System.Text;
using System.Threading.Tasks;
﻿using biz.dfch.CS.Utilities.Attributes;

namespace biz.dfch.CS.Utilities.Tests.Attributes
{
    [TestClass]
    public class StringValueAttributeExtensionTest
    {
        [TestMethod]
        public void GetStringValueForEnumWithStringValueAnnotationReturnsValueProvidedInAnnotation()
        {
            Assert.AreEqual("Regular", Importance.Regular.GetStringValue());
            Assert.AreEqual("Important", Importance.Important.GetStringValue());
        }
    }
}

