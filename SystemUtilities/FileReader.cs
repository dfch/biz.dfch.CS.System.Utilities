using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SystemUtilities
{
    class FileReader
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
