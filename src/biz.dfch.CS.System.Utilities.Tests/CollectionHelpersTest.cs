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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Specialized;

namespace biz.dfch.CS.Utilities.Tests
{
    [TestClass]
    public class CollectionHelpersTests
    {
        [TestMethod]
        public void CompareNameValueCollectionsWithEqualCollectionsShouldReturnTrue()
        {
            // Arrange
            var left = new NameValueCollection();
            left.Add("arbitrary-name1", "arbitrary-value1");
            left.Add("arbitrary-name2", "arbitrary-value2");

            var right = new NameValueCollection();
            right.Add("arbitrary-name1", "arbitrary-value1");
            right.Add("arbitrary-name2", "arbitrary-value2");

            // Act
            var fReturnExactOrder = CollectionHelpers.CompareNameValueCollections(left, right, true);
            var fReturnAnyOrder = CollectionHelpers.CompareNameValueCollections(left, right, false);

            // Assert
            Assert.IsTrue(fReturnExactOrder);
            Assert.IsTrue(fReturnAnyOrder);
        }
        [TestMethod]
        public void CompareNameValueCollectionsWithEqualNotExactCollectionsShouldReturnFalse()
        {
            // Arrange
            var left = new NameValueCollection();
            left.Add("arbitrary-name1", "arbitrary-value1");
            left.Add("arbitrary-name2", "arbitrary-value2");

            var right = new NameValueCollection();
            right.Add("arbitrary-name2", "arbitrary-value2");
            right.Add("arbitrary-name1", "arbitrary-value1");

            // Act
            var fReturnExactOrder = CollectionHelpers.CompareNameValueCollections(left, right, true);
            var fReturnAnyOrder = CollectionHelpers.CompareNameValueCollections(left, right, false);

            // Assert
            Assert.IsFalse(fReturnExactOrder);
            Assert.IsTrue(fReturnAnyOrder);
        }
        [TestMethod]
        public void CompareNameValueCollectionsWithNotEqualCollectionsShouldReturnFalse()
        {
            // Arrange
            var left = new NameValueCollection();
            left.Add("arbitrary-name1", "arbitrary-value1");
            left.Add("arbitrary-name2", "arbitrary-value2");

            var right = new NameValueCollection();
            right.Add("arbitrary-name1", "arbitrary-value-slightly-different");
            right.Add("arbitrary-name2", "arbitrary-value2");

            // Act
            var fReturnExactOrder = CollectionHelpers.CompareNameValueCollections(left, right, true);
            var fReturnAnyOrder = CollectionHelpers.CompareNameValueCollections(left, right, false);

            // Assert
            Assert.IsFalse(fReturnExactOrder);
            Assert.IsFalse(fReturnAnyOrder);
        }
        [TestMethod]
        public void CompareNameValueCollectionsWithEmptyCollectionsShouldReturnTrue()
        {
            // Arrange
            var left = new NameValueCollection();
            var right = new NameValueCollection();

            // Act
            var fReturnExactOrder = CollectionHelpers.CompareNameValueCollections(left, right, true);
            var fReturnAnyOrder = CollectionHelpers.CompareNameValueCollections(left, right, false);

            // Assert
            Assert.IsTrue(fReturnExactOrder);
            Assert.IsTrue(fReturnAnyOrder);
        }
        [TestMethod]
        public void CompareNameValueCollectionsWithNullCollectionsShouldReturnFalse()
        {
            // Arrange
            NameValueCollection left = null;
            NameValueCollection right = null;

            // Act
            var fReturnExactOrder = CollectionHelpers.CompareNameValueCollections(left, right, true);
            var fReturnAnyOrder = CollectionHelpers.CompareNameValueCollections(left, right, false);

            // Assert
            Assert.IsFalse(fReturnExactOrder);
            Assert.IsFalse(fReturnAnyOrder);
        }
    }
}