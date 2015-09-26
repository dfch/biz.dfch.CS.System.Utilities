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
using System.Net;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace biz.dfch.CS.Utilities.Http
{
    [Serializable]
    public class HttpStatusException : Exception, ISerializable
    {
        private string _message = HttpStatusCode.InternalServerError.ToString();
        public new string Message { 
            get
            {
                return _message;
            }
            set
            {
                _message = value;
            }
        }

        private HttpStatusCode _statusCode = HttpStatusCode.InternalServerError;
        public HttpStatusCode StatusCode 
        { 
            get
            {
                return _statusCode;
            }
            set
            {
                _statusCode = value;
            }
        }

        private Exception _innerException = null;
        public new Exception InnerException
        { 
            get
            {
                return _innerException;
            }
            set
            {
                _innerException = value;
            }
        }

        public HttpStatusException()
        {
            // N/A
        }

        public HttpStatusException(string message)
        {
            _message = message;
        }

        public HttpStatusException(HttpStatusCode statusCode, string message)
        {
            _message = message;
            _statusCode = statusCode;
        }

        public HttpStatusException(string message, Exception inner)
        {
            _message = message;
            _innerException = inner;
        }

        public HttpStatusException(HttpStatusCode statusCode, string message, Exception inner)
        {
            _message = message;
            _statusCode = statusCode;
            _innerException = inner;
        }

        public HttpStatusException(HttpStatusCode statusCode, Exception inner)
        {
            _message = inner.Message;
            _statusCode = statusCode;
            _innerException = inner;
        }

        protected HttpStatusException(SerializationInfo info, StreamingContext context)
        {
            _message = info.GetString("Message");
            _statusCode = (HttpStatusCode) info.GetValue("StatusCode", typeof(HttpStatusCode));
            _innerException = info.GetValue("InnerException", typeof(Exception)) as Exception;
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        protected new virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (null == info)
            {
                throw new ArgumentNullException("info");
            }

            info.AddValue("Message", _message);
            info.AddValue("StatusCode", _statusCode);
            info.AddValue("InnerException", _innerException);
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (null == info)
            {
                throw new ArgumentNullException("info");
            }

            GetObjectData(info, context);
        }
    }
}
