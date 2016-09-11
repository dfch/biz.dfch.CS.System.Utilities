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

namespace biz.dfch.CS.Utilities.General
{
    public static class Method
    {
        public static string GetName([System.Runtime.CompilerServices.CallerMemberName] string value = "")
        {
            return value;
        }

        // this method is just an alias for the previous method
        public static string fn([System.Runtime.CompilerServices.CallerMemberName] string value = "")
        {
            return value;
        }

        public static string GetFilePath([System.Runtime.CompilerServices.CallerFilePath] string value = "")
        {
            return value;
        }

        public static int GetLineNumber([System.Runtime.CompilerServices.CallerLineNumber] int value = 0)
        {
            return value;
        }
    }
}
