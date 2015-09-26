/**
 *
 *
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
 *
 */
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using biz.dfch.CS.Utilities.Logging;

// http://stackoverflow.com/questions/3391936/using-webclient-for-json-serialization
namespace biz.dfch.CS.Utilities.Convert
{
    class JsonHelper
    {
        public static string ToJson<T>(T instance)
        {
            return JsonConvert.SerializeObject(instance);
        }

        public static T ToJson<T>(String instance)
        {
            return JsonConvert.DeserializeObject<T>(instance);
        }

        // DFTODO - this method will probably never be called 
        // as the signature is the same as the next method
        public static string ToJson(Object instance)
        {
            return JsonConvert.SerializeObject(instance);
        }

        public static Dictionary<String, Object> ToJson(String instance)
        {
            return JsonConvert.DeserializeObject<Dictionary<String, Object>>(instance);
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
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static string FromJson(Dictionary<String, Object> instance, string key, string defaultValue = "")
        {
            string _return = defaultValue;
            try
            {
                if (instance.ContainsKey(key))
                {
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
                Debug.WriteLine("FromJson: ERROR on instance with key '{0}'", key, "");
                Debug.WriteException(key, ex);
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
                Debug.WriteLine("FromJson: ERROR on instance with key '{0}'", key, "");
                Debug.WriteException(key, ex);
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
            return JsonConvert.SerializeObject(string.Empty);
        }
    }
}
