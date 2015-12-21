/**
 * Copyright 2014-2015 d-fens GmbH
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

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]
namespace biz.dfch.CS.Utilities.Logging
{
    public class LogBase
    {
        private static log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected static log4net.ILog log 
        { 
            get
            {
                return _log;
            }
            private set
            {
                _log = value;
            }
        }

        public static void WriteException(string message, Exception ex)
        {
            if (log.IsErrorEnabled)
            {
                log.ErrorFormat("{0}@{1}: '{2}'\r\n[{3}]\r\n{4}", ex.GetType().Name, ex.Source, message, ex.Message, ex.StackTrace);
            }
        }
    }
}
