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
using biz.dfch.CS.Utilities.Security;
using biz.dfch.CS.Utilities.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace biz.dfch.CS.Utilities.Tests.Security
{
    [TestClass]
    public class CryptographyTest
    {
        private const string MASTER_PASSWORD = "MasterP@ssw0rd";

        [TestCleanup]
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
            var cryptography = new Cryptography(MASTER_PASSWORD);
            Assert.AreEqual(typeof(Cryptography), cryptography.GetType());
        }

        [TestMethod]
        [ExpectContractFailure]
        public void EncryptNullPasswordThrowsContractException()
        {
            Cryptography.Encrypt("Plaintext", null);
        }

        [TestMethod]
        [ExpectContractFailure]
        public void EncryptEmptyPasswordThrowsContractException()
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
            var password = Cryptography.Encrypt("Plaintext", MASTER_PASSWORD);
            Assert.AreEqual("tFnA+Mj47qqW6Uk5336Y9g==", password);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EncryptNullThrowsArgumentNullException()
        {
            Cryptography.Encrypt(null, MASTER_PASSWORD);
        }

        [TestMethod]
        public void EncryptEmpty()
        {
            var password = Cryptography.Encrypt(string.Empty, MASTER_PASSWORD);
            Assert.AreEqual("Bt9avcuVQC2SliZ79DJUFw==", password);
        }

        [TestMethod]
        public void EncryptViaConstructorSucceeds()
        {
            const string plaintext = "myP@ssw0rd";
            var cryptography = new Cryptography(MASTER_PASSWORD);
            var password = Cryptography.Encrypt(plaintext);
            Assert.AreEqual("X5eEcbMPLitplNgkYkDPpw==", password);
        }

        [TestMethod]
        [ExpectContractFailure]
        public void DecryptNullPasswordThrowsContractException()
        {
            Cryptography.Decrypt("hX/ms5U/9cYKxmrgF0k7jg==", null);
        }

        [TestMethod]
        [ExpectContractFailure]
        public void DecryptEmptyPasswordThrowsContractException()
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
            var plaintext = Cryptography.Decrypt("tFnA+Mj47qqW6Uk5336Y9g==", MASTER_PASSWORD);
            Assert.AreEqual("Plaintext", plaintext);
        }

        [TestMethod]
        public void DecryptNullThrowsArgumentNullException()
        {
            var plaintext = Cryptography.Decrypt(null, MASTER_PASSWORD);
            Assert.IsNull(plaintext);
        }

        [TestMethod]
        public void DecryptEmpty()
        {
            var plaintext = Cryptography.Decrypt(string.Empty, MASTER_PASSWORD);
            Assert.AreEqual("", plaintext);
        }

        [TestMethod]
        public void DecryptViaConstructorSucceeds()
        {
            var cryptography = new Cryptography(MASTER_PASSWORD);
            var plaintext = Cryptography.Decrypt("X5eEcbMPLitplNgkYkDPpw==");
            Assert.AreEqual("myP@ssw0rd", plaintext);
        }
    }
}
