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
 */

using System;
using System.Runtime.InteropServices;

namespace biz.dfch.CS.System.Utilities
{
    public class FileReader
    {
        const uint GENERIC_READ = 0x80000000;
        const uint OPEN_EXISTING = 3;
        private IntPtr handle;

        public FileReader(IntPtr handle)
        {
            this.handle = handle;
        }

        // check http://msdn.microsoft.com/en-US/library/2d9wy99d%28v=vs.80%29.aspx
        // set Project, Properties, Build, General, "Allow unsafe code" or set "/unsafe"
        // or check http://msdn.microsoft.com/en-us/library/tstb6eyt.aspx
        [DllImport("kernel32", SetLastError = true)]
        static extern unsafe bool ReadFile
        (
            IntPtr hFile
            ,
            void* pBuffer
            ,
            int NumberOfBytesToRead
            ,
            int* pNumberOfBytesRead
            ,
            int Overlapped
        );

        [DllImport("kernel32", SetLastError = true)]
        static extern unsafe bool CloseHandle
        (
            IntPtr hObject // handle to object
        );

        public unsafe int Read
        (
            byte[] Buffer
            ,
            int StartOffset
            ,
            int BytesToRead
        )
        {
            int BytesRead = 0;
            fixed (byte* p = Buffer)
            {
                if (!ReadFile(handle, p + StartOffset, BytesToRead, &BytesRead, 0))
                {
                    BytesRead = 0;
                }
            }
            return BytesRead;
        }

        public bool Close()
        {
            var fReturn = false;
            fReturn = CloseHandle(handle);
            handle = IntPtr.Zero;
            return fReturn;
        }
    }
}
