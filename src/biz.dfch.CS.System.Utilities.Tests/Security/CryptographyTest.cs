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
        public void testCleanup()
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
            new Cryptography(String.Empty);
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
            var MasterPassword = "myMasterPassword";
            var cryptography = new Cryptography(MasterPassword);
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
            Cryptography.Encrypt("Plaintext", String.Empty);
        }

        [TestMethod]
        public void EncryptWhitespacePasswordSucceeds()
        {
            var Password = Cryptography.Encrypt("Plaintext", " ");
            Assert.AreEqual("hX/ms5U/9cYKxmrgF0k7jg==", Password);
        }

        [TestMethod]
        public void EncryptMasterPasswordSucceeds()
        {
            var Password = Cryptography.Encrypt("Plaintext", "MasterP@ssw0rd");
            Assert.AreEqual("tFnA+Mj47qqW6Uk5336Y9g==", Password);
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
            var Password = Cryptography.Encrypt(String.Empty, "MasterP@ssw0rd");
            Assert.AreEqual("Bt9avcuVQC2SliZ79DJUFw==", Password);
        }

        [TestMethod]
        public void EncryptViaConstructorSucceeds()
        {
            var Plaintext = "myP@ssw0rd";
            var cryptography = new Cryptography("MasterP@ssw0rd");
            var Password = Cryptography.Encrypt(Plaintext);
            Assert.AreEqual("X5eEcbMPLitplNgkYkDPpw==", Password);
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
            Cryptography.Decrypt("hX/ms5U/9cYKxmrgF0k7jg==", String.Empty);
        }

        [TestMethod]
        public void DecryptWhitespacePasswordSucceeds()
        {
            var Plaintext = Cryptography.Decrypt("hX/ms5U/9cYKxmrgF0k7jg==", " ");
            Assert.AreEqual("Plaintext", Plaintext);
        }

        [TestMethod]
        public void DecryptMasterPasswordSucceeds()
        {
            var Plaintext = Cryptography.Decrypt("tFnA+Mj47qqW6Uk5336Y9g==", "MasterP@ssw0rd");
            Assert.AreEqual("Plaintext", Plaintext);
        }

        [TestMethod]
        public void DecryptNullThrowsArgumentNullException()
        {
            var Plaintext = Cryptography.Decrypt(null, "MasterP@ssw0rd");
            Assert.IsNull(Plaintext);
        }

        [TestMethod]
        public void DecryptEmpty()
        {
            var Plaintext = Cryptography.Decrypt(String.Empty, "MasterP@ssw0rd");
            Assert.AreEqual("", Plaintext);
        }

        [TestMethod]
        public void DecryptViaConstructorSucceeds()
        {
            var cryptography = new Cryptography("MasterP@ssw0rd");
            var Plaintext = Cryptography.Decrypt("X5eEcbMPLitplNgkYkDPpw==");
            Assert.AreEqual("myP@ssw0rd", Plaintext);
        }
    }
}
