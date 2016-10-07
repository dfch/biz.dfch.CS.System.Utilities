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

﻿using System;

namespace biz.dfch.CS.Utilities
{
    /// <summary>
    /// Provides a generic Parse method to parse a string value
    /// to a specific enum type.
    /// </summary>
    public static class EnumUtil
    {
        /// <summary>
        /// Parses the provided value to the desired enum type (defined by T).
        /// </summary>
        /// <typeparam name="T">Enum type to parse value into</typeparam>
        /// <param name="value">Enum string value</param>
        /// <param name="ignoreCase">Ignores case of specified value</param>
        /// <returns>T (enum value)</returns>
        public static T Parse<T>(string value, bool ignoreCase)
            where T : struct, IConvertible
        {
            return (T) Enum.Parse(typeof(T), value, ignoreCase);
        }

        /// <summary>
        /// Parses the provided value to the desired enum type (defined by T).
        /// </summary>
        /// <typeparam name="T">Enum type to parse value into</typeparam>
        /// <param name="value">Enum string value</param>
        /// <returns>T (enum value)</returns>
        public static T Parse<T>(string value)
            where T : struct, IConvertible
        {
            return Parse<T>(value, true);
        }
    }
}
