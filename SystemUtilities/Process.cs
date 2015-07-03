using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace biz.dfch.CS.System.Utilities
{
    public class Process
    {
        private const int HANDLE_FLAG_INHERIT = 1;
        private enum ResultDictionaryNameEnum
        {
            ERRORLEVEL
            ,
            STDIN
            ,
            STDOUT
            ,
            STDERR
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SECURITY_ATTRIBUTES
        {
            public int length;
            public IntPtr lpSecurityDescriptor;
            public bool bInheritHandle;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct STARTUPINFO
        {
            public Int32 cb;
            public String lpReserved;
            public String lpDesktop;
            public String lpTitle;
            public Int32 dwX;
            public Int32 dwY;
            public Int32 dwXSize;
            public Int32 dwYSize;
            public Int32 dwXCountChars;
            public Int32 dwYCountChars;
            public Int32 dwFillAttribute;
            public Int32 dwFlags;
            public Int16 wShowWindow;
            public Int16 cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PROCESS_INFORMATION
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public Int32 dwProcessId;
            public Int32 dwThreadId;
        }

        [DllImport("kernel32.dll")]
        static extern bool SetHandleInformation(IntPtr hObject, int dwMask, uint dwFlags);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern bool CreatePipe(out IntPtr phReadPipe, out IntPtr phWritePipe, IntPtr lpPipeAttributes, UInt32 nSize);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Boolean CreateProcessWithLogonW
        (
            String lpszUsername,
            String lpszDomain,
            String lpszPassword,
            Int32 dwLogonFlags,
            String applicationName,
            String commandLine,
            Int32 creationFlags,
            IntPtr environment,
            String currentDirectory,
            ref STARTUPINFO sui,
            out PROCESS_INFORMATION processInfo
        );

        [DllImport("kernel32", SetLastError = true)]
        public static extern Boolean CloseHandle(IntPtr handle);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetExitCodeProcess(IntPtr hProcess, out uint ExitCode);

        public static Dictionary<string, string> OutputParameter = new Dictionary<string, string>();

        private static object readStandardOutputThreadCompleted = false;
        private static void ReadStandardOutputThread(object data)
        {
            lock (readStandardOutputThreadCompleted)
            {
                IntPtr handle = (IntPtr)data;

                byte[] buffer = new byte[1];
                int bytesRead;
                var sb = new StringBuilder();

                dynamic consoleEncoding;
                consoleEncoding = new ASCIIEncoding();
                var readerStdout = new FileReader(handle);
                sb.Clear();
                bytesRead = 0;
                do
                {
                    bytesRead = readerStdout.Read(buffer, 0, buffer.Length);
                    string content = consoleEncoding.GetString(buffer, 0, bytesRead);
                    sb.Append(content);
                }
                while (bytesRead > 0);
                readerStdout.Close();
                lock (OutputParameter)
                {
                    OutputParameter[ResultDictionaryNameEnum.STDOUT.ToString()] = sb.ToString();
                }
                readStandardOutputThreadCompleted = true;
                return;
            }
        }

        public static Dictionary<string, string> StartProcess(string commandLine, string workingDirectory, NetworkCredential credential)
        {
            return StartProcess(commandLine, workingDirectory, credential.Domain, credential.UserName, credential.Password);
        }

        public static Dictionary<string, string> StartProcess(string commandLine, string workingDirectory, string domain, string username, string password)
        {
            var fReturn = false;

            lock (OutputParameter)
            {
                OutputParameter.Clear();
                OutputParameter.Add(ResultDictionaryNameEnum.ERRORLEVEL.ToString(), string.Empty);
                OutputParameter.Add(ResultDictionaryNameEnum.STDOUT.ToString(), string.Empty);
                OutputParameter.Add(ResultDictionaryNameEnum.STDERR.ToString(), string.Empty);
            }

            IntPtr hConsoleInputRead, hConsoleInputWrite = IntPtr.Zero;
            IntPtr hConsoleOutputRead, hConsoleOutputWrite = IntPtr.Zero;
            IntPtr hConsoleErrorRead, hConsoleErrorWrite = IntPtr.Zero;

            var saAttr = new SECURITY_ATTRIBUTES();
            // check http://msdn.microsoft.com/en-us/library/windows/desktop/ms682499%28v=vs.85%29.aspx
            IntPtr pSaAttr = IntPtr.Zero;

            var processInfo = new PROCESS_INFORMATION();
            var startInfo = new STARTUPINFO();
            try
            {
                pSaAttr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SECURITY_ATTRIBUTES)));

                saAttr.bInheritHandle = true;
                saAttr.lpSecurityDescriptor = IntPtr.Zero;
                saAttr.length = Marshal.SizeOf(typeof(SECURITY_ATTRIBUTES));
                saAttr.lpSecurityDescriptor = IntPtr.Zero;

                Marshal.StructureToPtr(saAttr, pSaAttr, true);

                // Create STDOUT pipe.
                fReturn = CreatePipe(out hConsoleOutputRead, out hConsoleOutputWrite, pSaAttr, 1024);
                //ensure the read handle to pipe for stdout is not inherited
                SetHandleInformation(hConsoleOutputRead, HANDLE_FLAG_INHERIT, 0);

                // Create STDIN pipe
                fReturn = CreatePipe(out hConsoleInputRead, out hConsoleInputWrite, pSaAttr, 1024);
                SetHandleInformation(hConsoleInputRead, HANDLE_FLAG_INHERIT, 0);

                // Create STDERR pipe
                fReturn = CreatePipe(out hConsoleErrorRead, out hConsoleErrorWrite, pSaAttr, 1024);
                SetHandleInformation(hConsoleErrorRead, HANDLE_FLAG_INHERIT, 0);

                startInfo.cb = Marshal.SizeOf(startInfo);
                startInfo.hStdError = hConsoleErrorWrite;
                //startInfo.hStdInput = hConsoleInputWrite;
                startInfo.hStdOutput = hConsoleOutputWrite;
                startInfo.dwFlags = 0x00000100; // STARTF_USESTDHANDLES

                Debug.WriteLine(string.Format("SystemUtilities.Process.StartProcess: '{0}' in '{1}' [as '{2}\\{3}] ...", commandLine, workingDirectory, domain, username, password));
                // Create process
                fReturn = CreateProcessWithLogonW(
                    username,
                    domain,
                    password,
                    0,
                    null,
                    commandLine,
                    0,
                    IntPtr.Zero,
                    workingDirectory,
                    ref startInfo,
                    out processInfo
                );
                if (!fReturn)
                {
                    throw new Exception(string.Format("StartProcess() FAILED with error #{0}", Marshal.GetLastWin32Error()));
                }

                CloseHandle(hConsoleErrorWrite);
                hConsoleErrorWrite = IntPtr.Zero;
                CloseHandle(hConsoleOutputWrite);
                hConsoleOutputWrite = IntPtr.Zero;
                CloseHandle(hConsoleInputWrite);
                hConsoleInputWrite = IntPtr.Zero;

                byte[] buffer = new byte[1];
                int bytesRead;
                var sb = new StringBuilder();

                dynamic consoleEncoding;
                consoleEncoding = new ASCIIEncoding();

                Thread threadOutput = new Thread(Process.ReadStandardOutputThread);
                threadOutput.Start(hConsoleOutputRead);

                var readerStderr = new FileReader(hConsoleErrorRead);
                sb.Clear();
                bytesRead = 0;
                do
                {
                    bytesRead = readerStderr.Read(buffer, 0, buffer.Length);
                    string content = consoleEncoding.GetString(buffer, 0, bytesRead);
                    sb.Append(content);
                }
                while (bytesRead > 0);
                readerStderr.Close();
                lock (OutputParameter)
                {
                    OutputParameter["STDERR"] = sb.ToString();
                }

                lock (readStandardOutputThreadCompleted)
                {
                    Debug.WriteLine("ReadStandardOutputThreadCompleted '{0}'", readStandardOutputThreadCompleted);
                }

                // This read is no longer used here but implemented in a separate thread
                //var readerStdout = new FileReader(hConsoleOutputRead);
                //sb.Clear();
                //bytesRead = 0;
                //do
                //{
                //    bytesRead = readerStdout.Read(buffer, 0, buffer.Length);
                //    string content = ConsoleEncoding.GetString(buffer, 0, bytesRead);
                //    sb.Append(content);
                //}
                //while (bytesRead > 0);
                //readerStdout.Close();
                //OutputParameter[ResultDictionaryNameEnum.STDOUT.ToString()] = sb.ToString();

                UInt32 processExitCode = UInt32.MaxValue;
                fReturn = GetExitCodeProcess(processInfo.hProcess, out processExitCode);
                lock (OutputParameter)
                {
                    OutputParameter[ResultDictionaryNameEnum.ERRORLEVEL.ToString()] = processExitCode.ToString();
                }

                return OutputParameter;
            }
            finally
            {
                if (null != pSaAttr)
                {
                    Marshal.FreeHGlobal(pSaAttr);
                }
                // Close all handles
                CloseHandle(processInfo.hThread);
                CloseHandle(processInfo.hProcess);
                if (IntPtr.Zero != hConsoleErrorWrite) CloseHandle(hConsoleErrorWrite);
                if (IntPtr.Zero != hConsoleOutputWrite) CloseHandle(hConsoleOutputWrite);
                if (IntPtr.Zero != hConsoleInputWrite) CloseHandle(hConsoleInputWrite);
            }
        }
    }
}
