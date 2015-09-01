/**
 * Copyright 2014-2015 Ronald Rink, d-fens GmbH
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
 * */

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;

namespace biz.dfch.CS.Utilities.Http
{
    public class HttpStatusErrorMessage
    {
        public HttpStatusErrorMessage()
        {
        }

        public HttpStatusErrorMessage(string message)
        {
            _code = Code;
            _message = message;
            _innererror = Innererror;
        }
        
        public override string ToString()
        {
            if (_innererror is Exception)
            {
                dynamic innererrordyn = Innererror;
                var exception = new Dictionary<string, object>();
                exception.Add("message", innererrordyn.Message);
                exception.Add("stacktrace", innererrordyn.StackTrace);
                exception.Add("type", Innererror.GetType().Name);
                _innererror = exception;
            }
            var json = JsonConvert.SerializeObject(this);
            return json;
        }
        
        private int? _code = null;
        public int? Code
        {
            get { return _code; }
            set { _code = value; }
        }
        
        private string _message;
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }
        
        private string _category;
        public string Category
        {
            get { return _category; }
            set { _category = value; }
        }
        
        private object _innererror;
        public object Innererror
        {
            get { return _innererror; }
            set { _innererror = value; }
        }
    }
}
