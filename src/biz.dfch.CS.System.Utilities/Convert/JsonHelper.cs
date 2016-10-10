/**
 * Copyright 2014-2016 d-fens GmbH
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

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using biz.dfch.CS.Utilities.Logging;

// http://stackoverflow.com/questions/3391936/using-webclient-for-json-serialization
namespace biz.dfch.CS.Utilities.Convert
{
    public class JsonHelper
    {
        public static string ToJson<T>(T instance)
        {
            return JsonConvert.SerializeObject(instance);
        }

        public static T ToJson<T>(string instance)
        {
            return JsonConvert.DeserializeObject<T>(instance);
        }

        // DFTODO - this method will probably never be called 
        // as the signature is the same as the next method
        [Obsolete("Do not use this method.")]
        public static string ToJson(object instance)
        {
            return JsonConvert.SerializeObject(instance);
        }

        public static Dictionary<string, object> ToJson(string instance)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(instance);
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

        [Obsolete("Do not use this method.")]
        public static string FromJson(Dictionary<string, object> instance, string key, string defaultValue = "")
        {
            Contract.Ensures(null != Contract.Result<string>());

            string _return = defaultValue;
            try
            {
                if (instance.ContainsKey(key))
                {
                    var value = instance[key];

                    var collection = value as ICollection;
                    if (null != collection && 0 < collection.Count)
                    {
                        foreach (var item in collection)
                        {
                            _return = item.ToString();
                            break;
                        }
                    }
                    else
                    {
                        _return = value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteException(string.Format("FromJson: ERROR on instance with key '{0}'", key), ex);
                _return = defaultValue;
            }
            return _return;
        }

        public static string FromJson(JToken instance, string key, string defaultValue = "")
        {
            Contract.Ensures(null != Contract.Result<string>());

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
                        _return = jArray.ToObject<List<string>>()[0].ToString();
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
                Debug.WriteException(string.Format("FromJson: ERROR on instance with key '{0}'", key), ex);
                _return = defaultValue;
            }

            return _return;
        }

        public static string EmptyString()
        {
            return JsonConvert.SerializeObject(string.Empty);
        }
    }
}
