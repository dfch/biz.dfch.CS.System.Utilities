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
using System.Configuration;
using System.Security.Cryptography;
using System.Text;

[assembly: System.Runtime.CompilerServices.InternalsVisibleToAttribute("biz.dfch.CS.System.Utilities.Tests")]
namespace biz.dfch.CS.Utilities.Security
{
    public class Cryptography
    {
        private const string _AppSettingsPassword = "Cryptograhpy.Password";
        private static string _Password;

        internal static string Password
        {
            get { return Cryptography._Password; }
            set { Cryptography._Password = value; }
        }

        internal Cryptography(string Password)
        {
            if (string.IsNullOrEmpty(Password))
            {
                throw new ArgumentNullException(string.Format("No or empty password specified."));
            }
            _Password = Password;
        }

        public static string Encrypt(string Data, string Password = null)
        {
            if (string.IsNullOrEmpty(Password))
            {
                if (string.IsNullOrEmpty(_Password))
                {
                    _Password = ConfigurationManager.AppSettings[_AppSettingsPassword];
                    Password = _Password;
                    if (string.IsNullOrEmpty(Password))
                    {
                        throw new ArgumentNullException(string.Format("{0}: no password in configuration found or no password specified.", _AppSettingsPassword));
                    }
                }
                else
                {
                    Password = _Password;
                }
            }
            string Result;
            byte[] abResult;
            UTF8Encoding UTF8 = new UTF8Encoding();
            var HashProvider = new SHA256CryptoServiceProvider();
            byte[] Key = HashProvider.ComputeHash(UTF8.GetBytes(Password));
            var Algorithm = new AesCryptoServiceProvider();
            try
            {
                Algorithm.Key = Key;
                Algorithm.Mode = CipherMode.ECB;
                Algorithm.Padding = PaddingMode.PKCS7;
                byte[] abData = UTF8.GetBytes(Data);
                using (ICryptoTransform Encryptor = Algorithm.CreateEncryptor())
                {
                    abResult = Encryptor.TransformFinalBlock(abData, 0, abData.Length);
                    Result = System.Convert.ToBase64String(abResult);
                }
            }
            finally
            {
                Algorithm.Clear();
                HashProvider.Clear();
            }
            return Result;
        }

        public static string Decrypt(string EncryptedData, string Password = null)
        {
            if (string.IsNullOrEmpty(Password))
            {
                if (string.IsNullOrEmpty(_Password))
                {
                    _Password = ConfigurationManager.AppSettings[_AppSettingsPassword];
                    Password = _Password;
                    if (string.IsNullOrEmpty(Password))
                    {
                        throw new ArgumentNullException(string.Format("{0}: no password in configuration found or no password specified.", _AppSettingsPassword));
                    }
                }
                else
                {
                    Password = _Password;
                }
            }
            string Result;
            byte[] abResult;
            UTF8Encoding UTF8 = new UTF8Encoding();
            var HashProvider = new SHA256CryptoServiceProvider();
            byte[] Key = HashProvider.ComputeHash(UTF8.GetBytes(Password));
            var Algorithm = new AesCryptoServiceProvider();
            try
            {
                Algorithm.Key = Key;
                Algorithm.Mode = CipherMode.ECB;
                Algorithm.Padding = PaddingMode.PKCS7;
                byte[] abEncryptedData = System.Convert.FromBase64String(EncryptedData);
                using (ICryptoTransform Decryptor = Algorithm.CreateDecryptor())
                {
                    abResult = Decryptor.TransformFinalBlock(abEncryptedData, 0, abEncryptedData.Length);
                    Result = UTF8.GetString(abResult);
                }
            }
            catch
            {
                Result = EncryptedData;
            }
            finally
            {
                Algorithm.Clear();
                HashProvider.Clear();
            }
            return Result;
        }
    }
}
