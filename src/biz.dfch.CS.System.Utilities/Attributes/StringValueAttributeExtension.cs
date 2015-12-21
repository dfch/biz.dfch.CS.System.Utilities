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
using System.Reflection;

namespace biz.dfch.CS.Utilities.Attributes
{
    public static class StringValueAttributeExtension
    {
        /// <summary>
        /// Returns the string value for the passed enum value.
        /// IMPORTANT: The 'StringValue' attribute has to be assigned
        /// to the items in the provided enum.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static String GetStringValue(this Enum value)
        {
            // Get the type
            Type type = value.GetType();

            // Get fieldinfo for this type
            FieldInfo fieldInfo = type.GetField(value.ToString());

            // Get the stringvalue attributes
            StringValueAttribute[] attributes = fieldInfo.GetCustomAttributes(
                typeof(StringValueAttribute), false) as StringValueAttribute[];

            // Returns the first if there was a match.
            return attributes.Length > 0 ? attributes[0].StringValue : null;
        }
    }
}