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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace biz.dfch.CS.Utilities
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
            public int cb;
            public string lpReserved;
            public string lpDesktop;
            public string lpTitle;
            public int dwX;
            public int dwY;
            public int dwXSize;
            public int dwYSize;
            public int dwXCountChars;
            public int dwYCountChars;
            public int dwFillAttribute;
            public int dwFlags;
            public short wShowWindow;
            public short cbReserved2;
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
            public int dwProcessId;
            public int dwThreadId;
        }

        [DllImport("kernel32.dll")]
        static extern bool SetHandleInformation(IntPtr hObject, int dwMask, uint dwFlags);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern bool CreatePipe(out IntPtr phReadPipe, out IntPtr phWritePipe, IntPtr lpPipeAttributes, uint nSize);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool CreateProcessWithLogonW
        (
            string lpszUsername,
            string lpszDomain,
            string lpszPassword,
            int dwLogonFlags,
            string applicationName,
            string commandLine,
            int creationFlags,
            IntPtr environment,
            string currentDirectory,
            ref STARTUPINFO sui,
            out PROCESS_INFORMATION processInfo
        );

        [DllImport("kernel32", SetLastError = true)]
        public static extern bool CloseHandle(IntPtr handle);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetExitCodeProcess(IntPtr hProcess, out uint ExitCode);

        public static Dictionary<string, string> OutputParameter = new Dictionary<string, string>();

        private static object _readStandardOutputThreadCompleted = false;
        private static void ReadStandardOutputThread(object data)
        {
            lock (_readStandardOutputThreadCompleted)
            {
                var handle = (IntPtr)data;

                var buffer = new byte[1];
                var sb = new StringBuilder();

                var consoleEncoding = new ASCIIEncoding();
                var readerStdout = new FileReader(handle);
                sb.Clear();
                var bytesRead = 0;
                do
                {
                    bytesRead = readerStdout.Read(buffer, 0, buffer.Length);
                    var content = consoleEncoding.GetString(buffer, 0, bytesRead);
                    sb.Append(content);
                }
                while (bytesRead > 0);
                readerStdout.Close();
                lock (OutputParameter)
                {
                    OutputParameter[ResultDictionaryNameEnum.STDOUT.ToString()] = sb.ToString();
                }
                _readStandardOutputThreadCompleted = true;
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
            var pSaAttr = IntPtr.Zero;

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

                Debug.WriteLine("SystemUtilities.Process.StartProcess: '{0}' in '{1}' [as '{2}\\{3}] ...", commandLine, workingDirectory, domain, username);
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

                var buffer = new byte[1];
                var sb = new StringBuilder();

                var consoleEncoding = new ASCIIEncoding();

                var threadOutput = new Thread(Process.ReadStandardOutputThread);
                threadOutput.Start(hConsoleOutputRead);

                var readerStderr = new FileReader(hConsoleErrorRead);
                sb.Clear();
                var bytesRead = 0;
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

                lock (_readStandardOutputThreadCompleted)
                {
                    Debug.WriteLine("ReadStandardOutputThreadCompleted '{0}'", _readStandardOutputThreadCompleted);
                }

                var processExitCode = uint.MaxValue;
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
