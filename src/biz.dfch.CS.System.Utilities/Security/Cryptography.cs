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
using System.Configuration;
using System.Diagnostics.Contracts;
using System.Security.Cryptography;
using System.Text;

namespace biz.dfch.CS.Utilities.Security
{
    public class Cryptography
    {
        private const string APP_SETTINGS_PASSWORD = "Cryptograhpy.Password";
        private static string _password;
        private static readonly object _lock = new object();

        private static readonly UTF8Encoding _utf8 = new UTF8Encoding();

        internal static string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        internal Cryptography(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("password", "No or empty password specified.");
            }
            _password = password;
        }

        private static string GetPassword()
        {
            string result;

            if (string.IsNullOrEmpty(_password))
            {
                lock (_lock)
                {
                    if (string.IsNullOrEmpty(_password))
                    {
                        _password = ConfigurationManager.AppSettings[APP_SETTINGS_PASSWORD];
                    }
                    result = _password;
                }
            }
            else
            {
                result = _password;
            }

            return result;
        }

        public static string Encrypt(string data, string password = null)
        {
            if (string.IsNullOrEmpty(password))
            {
                password = GetPassword();
                Contract.Assert(!string.IsNullOrEmpty(password), "No password in AppSettings found or no password specified");
            }
            
            var hashProvider = new SHA256CryptoServiceProvider();
            var algorithm = new AesManaged
            {
                Key = hashProvider.ComputeHash(_utf8.GetBytes(password)),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            string result;
            try
            {
                var abData = _utf8.GetBytes(data);
                using (var encryptor = algorithm.CreateEncryptor())
                {
                    var abResult = encryptor.TransformFinalBlock(abData, 0, abData.Length);
                    result = System.Convert.ToBase64String(abResult);
                }
            }
            finally
            {
                algorithm.Clear();
                algorithm.Dispose();

                hashProvider.Clear();
                hashProvider.Dispose();
            }
            return result;
        }

        public static string Decrypt(string encryptedData, string password = null)
        {
            if (string.IsNullOrEmpty(password))
            {
                password = GetPassword();
                Contract.Assert(!string.IsNullOrEmpty(password), "No password in AppSettings found or no password specified");
            }

            var hashProvider = new SHA256CryptoServiceProvider();
            var algorithm = new AesCryptoServiceProvider
            {
                Key = hashProvider.ComputeHash(_utf8.GetBytes(password)),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            string result;
            try
            {
                var abEncryptedData = System.Convert.FromBase64String(encryptedData);
                using (var decryptor = algorithm.CreateDecryptor())
                {
                    var abResult = decryptor.TransformFinalBlock(abEncryptedData, 0, abEncryptedData.Length);
                    result = _utf8.GetString(abResult);
                }
            }
            catch (Exception)
            {
                result = encryptedData;
            }
            finally
            {
                algorithm.Clear();
                algorithm.Dispose();
                hashProvider.Clear();
                hashProvider.Dispose();
            }
            return result;
        }
    }
}
