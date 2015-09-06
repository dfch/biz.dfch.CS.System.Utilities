/**
 *
 *
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
 *
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using biz.dfch.CS.Utilities.Contracts.Endpoint;

namespace biz.dfch.CS.Utilities.Tests.Contracts.Endpoint
{
    public class ODataEndpointDataWithPriority : IODataEndpointData
    {
        public ODataEndpointDataWithPriority()
        {
            // N/A
        }

        public ODataEndpointDataWithPriority(int priority)
        {
            _priority = priority;
        }

        private int _priority;
        public int Priority
        {
            get
            {
                return _priority;
            }
        }

        public ServerRole ServerRole
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
