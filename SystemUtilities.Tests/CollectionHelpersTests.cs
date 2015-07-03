using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Specialized;
using biz.dfch.CS.System.Utilities;

namespace biz.dfch.CS.System.Utilties.Tests
{
    [TestClass]
    public class CollectionHelpersTests
    {
        [TestMethod]
        public void EqualCollectionsShouldReturnTrue()
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
        public void EqualNotExactCollectionsShouldReturnFalse()
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
        public void NotEqualCollectionsShouldReturnFalse()
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
        public void EmptyCollectionsShouldReturnTrue()
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
        public void NullCollectionsShouldReturnFalse()
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