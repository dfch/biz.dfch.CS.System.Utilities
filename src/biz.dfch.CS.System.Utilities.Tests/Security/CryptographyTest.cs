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
using biz.dfch.CS.Utilities.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace biz.dfch.CS.Utilities.Tests.Security
{
    [TestClass]
    public class CryptographyTest
    {

        [TestCleanup()]
        public void TestCleanup()
        {
            Cryptography.Password = null; ;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NewCryptographyNullThrowsArgumentNullException()
        {
            new Cryptography(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NewCryptographyEmptyThrowsArgumentNullException()
        {
            new Cryptography(string.Empty);
        }

        [TestMethod]
        public void NewCryptographyWhitespaceReturnsObject()
        {
            var cryptography = new Cryptography(" ");
            Assert.AreEqual(typeof(Cryptography), cryptography.GetType());
        }

        [TestMethod]
        public void NewCryptographyReturnsObject()
        {
            var masterPassword = "myMasterPassword";
            var cryptography = new Cryptography(masterPassword);
            Assert.AreEqual(typeof(Cryptography), cryptography.GetType());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EncryptNullPasswordThrowsArgumentNullException()
        {
            Cryptography.Encrypt("Plaintext", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EncryptEmptyPasswordThrowsArgumentNullException()
        {
            Cryptography.Encrypt("Plaintext", string.Empty);
        }

        [TestMethod]
        public void EncryptWhitespacePasswordSucceeds()
        {
            var password = Cryptography.Encrypt("Plaintext", " ");
            Assert.AreEqual("hX/ms5U/9cYKxmrgF0k7jg==", password);
        }

        [TestMethod]
        public void EncryptMasterPasswordSucceeds()
        {
            var password = Cryptography.Encrypt("Plaintext", "MasterP@ssw0rd");
            Assert.AreEqual("tFnA+Mj47qqW6Uk5336Y9g==", password);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EncryptNullThrowsArgumentNullException()
        {
            Cryptography.Encrypt(null, "MasterP@ssw0rd");
        }

        [TestMethod]
        public void EncryptEmpty()
        {
            var password = Cryptography.Encrypt(string.Empty, "MasterP@ssw0rd");
            Assert.AreEqual("Bt9avcuVQC2SliZ79DJUFw==", password);
        }

        [TestMethod]
        public void EncryptViaConstructorSucceeds()
        {
            const string plaintext = "myP@ssw0rd";
            var cryptography = new Cryptography("MasterP@ssw0rd");
            var password = Cryptography.Encrypt(plaintext);
            Assert.AreEqual("X5eEcbMPLitplNgkYkDPpw==", password);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DecryptNullPasswordThrowsArgumentNullException()
        {
            Cryptography.Decrypt("hX/ms5U/9cYKxmrgF0k7jg==", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DecryptEmptyPasswordThrowsArgumentNullException()
        {
            Cryptography.Decrypt("hX/ms5U/9cYKxmrgF0k7jg==", string.Empty);
        }

        [TestMethod]
        public void DecryptWhitespacePasswordSucceeds()
        {
            var plaintext = Cryptography.Decrypt("hX/ms5U/9cYKxmrgF0k7jg==", " ");
            Assert.AreEqual("Plaintext", plaintext);
        }

        [TestMethod]
        public void DecryptMasterPasswordSucceeds()
        {
            var plaintext = Cryptography.Decrypt("tFnA+Mj47qqW6Uk5336Y9g==", "MasterP@ssw0rd");
            Assert.AreEqual("Plaintext", plaintext);
        }

        [TestMethod]
        public void DecryptNullThrowsArgumentNullException()
        {
            var plaintext = Cryptography.Decrypt(null, "MasterP@ssw0rd");
            Assert.IsNull(plaintext);
        }

        [TestMethod]
        public void DecryptEmpty()
        {
            var plaintext = Cryptography.Decrypt(string.Empty, "MasterP@ssw0rd");
            Assert.AreEqual("", plaintext);
        }

        [TestMethod]
        public void DecryptViaConstructorSucceeds()
        {
            var cryptography = new Cryptography("MasterP@ssw0rd");
            var plaintext = Cryptography.Decrypt("X5eEcbMPLitplNgkYkDPpw==");
            Assert.AreEqual("myP@ssw0rd", plaintext);
        }
    }
}
