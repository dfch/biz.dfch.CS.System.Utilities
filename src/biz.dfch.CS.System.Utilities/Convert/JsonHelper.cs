/**
 *
 *
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
 *
 */
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Script.Serialization;
using System.Diagnostics;

// http://stackoverflow.com/questions/3391936/using-webclient-for-json-serialization
namespace biz.dfch.CS.Utilities.Convert
{
    class JsonHelper
    {
        public static string ToJson<T>(T instance)
        {
            string _return = String.Empty;
            _return = new JavaScriptSerializer().Serialize(instance);
            return _return;
        }

        public static string ToJson(Object instance)
        {
            string _return = String.Empty;
            _return = new JavaScriptSerializer().Serialize(instance);
            return _return;
        }

        public static Dictionary<String, Object> ToJson(String instance)
        {
            var _return = JsonConvert.DeserializeObject<Dictionary<String, Object>>(instance);
            return _return;
        }

        public static JToken Parse(string instance)
        {
            return JObject.Parse(instance);
        }

        public static string FromJsonString(string json)
        {
            return JsonHelper.FromJson<string>(json);
        }

        public static T FromJson<T>(string json)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));
            using (var tempStream = new MemoryStream(Encoding.Unicode.GetBytes(json)))
            {
                return (T)serializer.ReadObject(tempStream);
            }
        }

        public static string FromJson(Dictionary<String, Object> instance, string key, string defaultValue = "")
        {
            string _return = defaultValue;
            try
            {
                if (instance.ContainsKey(key))
                {
                    //_return = instance[key].ToString();
                    dynamic value = instance[key];
                    if (value is Array && 0 <= value.Count)
                    {
                        _return = value[0];
                    }
                    else
                    {
                        _return = value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(String.Format("FromJson: ERROR on instance with key '{0}'", key));
                Debug.WriteLine(String.Format("{0}: {1}\r\n{2}", ex.Source, ex.Message, ex.StackTrace));
                _return = defaultValue;
            }
            if (null == _return)
            {
                throw new ArgumentException(String.Format("Key '{0}' not found.", key));
            }
            return _return;
        }

        public static string FromJson(JToken instance, string key, string defaultValue = "")
        {
            string _return = defaultValue;
            try
            {
                var jv = instance.SelectToken(key);
                if (null != jv)
                {
                    _return = jv.ToString();
                }
                try
                {
                    var jArray = JToken.Parse(_return);
                    if (jArray is JArray)
                    {
                        _return = jArray.ToObject<List<String>>()[0].ToString();
                    }
                }
                catch
                {
                    // Intended not to catch this exception
                    // If an exception is triggered, it just means we do not have a JArray
                    // in which case we just continue with the string we already have.
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(String.Format("FromJson: ERROR on instance with key '{0}'", key));
                Debug.WriteLine(String.Format("{0}: {1}\r\n{2}", ex.Source, ex.Message, ex.StackTrace));
                _return = defaultValue;
            }
            if (null == _return)
            {
                throw new ArgumentException(String.Format("Key '{0}' not found.", key));
            }
            return _return;
        }

        public static string EmptyString()
        {
            return new JavaScriptSerializer().Serialize(String.Empty);
        }
    }
}
