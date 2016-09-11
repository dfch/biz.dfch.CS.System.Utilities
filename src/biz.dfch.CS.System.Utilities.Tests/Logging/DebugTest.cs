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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telerik.JustMock;
using biz.dfch.CS.Utilities.Logging;

namespace biz.dfch.CS.Utilities.Tests.Logging
{
    [TestClass]
    public class DebugTest
    {
        private readonly bool condition = true;
        private readonly object value = new object();
        private readonly string message = "arbitrary-message";
        private readonly Exception ex = new Exception("arbitrary-exception-message");
        private readonly string detailMessage = "arbitrary-detail-message";
        private readonly string category = "arbitrary-category";
        private readonly string arg0 = "arbitrary-arg0";
        private readonly string arg1 = "arbitrary-arg1";
        private readonly string arg2 = "arbitrary-arg2";

        #region ========== Assert ==========
        
        //[Ignore]
        //[TestMethod]
        //public void AssertWithConditionMessageDetailMessage()
        //{
        //    // Arrange
        //    Mock.SetupStatic(typeof(System.Diagnostics.Debug));
        //    Mock.Arrange(() => System.Diagnostics.Debug.Assert(condition, message, detailMessage)).OccursOnce();

        //    // Act
        //    Debug.Assert(condition, message, detailMessage);

        //    // Assert
        //    Mock.Assert(() => System.Diagnostics.Debug.Assert(condition, message, detailMessage));
        //}

        //[Ignore]
        //[TestMethod]
        //public void AssertWithConditionMessage()
        //{
        //    // Arrange
        //    Mock.SetupStatic(typeof(System.Diagnostics.Debug));
        //    Mock.Arrange(() => System.Diagnostics.Debug.Assert(condition, message)).OccursOnce();

        //    // Act
        //    Debug.Assert(condition, message);

        //    // Assert
        //    Mock.Assert(() => System.Diagnostics.Debug.Assert(condition, message));
        //}

        //[Ignore]
        //[TestMethod]
        //public void AssertWithCondition()
        //{
        //    // Arrange
        //    Mock.SetupStatic(typeof(System.Diagnostics.Debug));
        //    Mock.Arrange(() => System.Diagnostics.Debug.Assert(condition)).OccursOnce();

        //    // Act
        //    Debug.Assert(condition);

        //    // Assert
        //    Mock.Assert(() => System.Diagnostics.Debug.Assert(condition));
        //}
        
        #endregion

        # region ========== WriteLine ==========

        [TestMethod]
        public void WriteLineWithMessageParamArgsDebugEnabledIsTrue()
        {
            // Arrange
            var log4net = Mock.Create<log4net.ILog>();
            Mock.Arrange(() => log4net.IsDebugEnabled).Returns(true);
            Mock.Arrange(() => log4net.DebugFormat(message, arg0, arg1, arg2))
                .IgnoreInstance()
                .OccursOnce();

            // Act
            Debug.WriteLine(message, arg0, arg1, arg2);

            // Assert
            Mock.Assert(log4net);
        }

        [TestMethod]
        public void WriteLineWithMessageParamArgsDebugEnabledIsFalse()
        {
            // Arrange
            var log4net = Mock.Create<log4net.ILog>();
            Mock.Arrange(() => log4net.IsDebugEnabled).Returns(false);
            Mock.Arrange(() => log4net.DebugFormat(message, arg0, arg1, arg2))
                .IgnoreInstance()
                .OccursOnce();

            // Act
            Debug.WriteLine(message, arg0, arg1, arg2);

            // Assert
            Mock.Assert(log4net);
        }

        [TestMethod]
        public void WriteLineWithMessageCategoryDebugEnabledIsTrue()
        {
            // Arrange
            var log4net = Mock.Create<log4net.ILog>();
            Mock.Arrange(() => log4net.IsDebugEnabled).Returns(true);
            Mock.Arrange(() => log4net.DebugFormat(message, category))
                .IgnoreInstance()
                .OccursOnce();

            // Act
            Debug.WriteLine(message, category);

            // Assert
            Mock.Assert(log4net);
        }

        [TestMethod]
        public void WriteLineWithMessageCategoryDebugEnabledIsFalse()
        {
            // Arrange
            var log4net = Mock.Create<log4net.ILog>();
            Mock.Arrange(() => log4net.IsDebugEnabled).Returns(false);
            Mock.Arrange(() => log4net.DebugFormat(message, category))
                .IgnoreInstance()
                .OccursOnce();

            // Act
            Debug.WriteLine(message, category);

            // Assert
            Mock.Assert(log4net);
        }

        [TestMethod]
        public void WriteLineWithMessageDebugEnabledIsTrue()
        {
            // Arrange
            var log4net = Mock.Create<log4net.ILog>();
            Mock.Arrange(() => log4net.IsDebugEnabled).Returns(true);
            Mock.Arrange(() => log4net.DebugFormat(message))
                .IgnoreInstance()
                .OccursOnce();

            // Act
            Debug.WriteLine(message);

            // Assert
            Mock.Assert(log4net);
        }

        [TestMethod]
        public void WriteLineWithMessageDebugEnabledIsFalse()
        {
            // Arrange
            var log4net = Mock.Create<log4net.ILog>();
            Mock.Arrange(() => log4net.IsDebugEnabled).Returns(false);
            Mock.Arrange(() => log4net.DebugFormat(message))
                .IgnoreInstance()
                .OccursOnce();

            // Act
            Debug.WriteLine(message);

            // Assert
            Mock.Assert(log4net);
        }

        [TestMethod]
        public void WriteLineWithValueCategoryDebugEnabledIsTrue()
        {
            // Arrange
            var log4net = Mock.Create<log4net.ILog>();
            Mock.Arrange(() => log4net.IsDebugEnabled).Returns(true);
            Mock.Arrange(() => log4net.DebugFormat(Arg.AnyString, value, Arg.IsAny<object>()))
                .IgnoreInstance()
                .OccursOnce();

            // Act
            Debug.WriteLine(value, category);

            // Assert
            Mock.Assert(log4net);
        }

        [TestMethod]
        public void WriteLineWithValueCategoryDebugEnabledIsFalse()
        {
            // Arrange
            var log4net = Mock.Create<log4net.ILog>();
            Mock.Arrange(() => log4net.IsDebugEnabled).Returns(false);
            Mock.Arrange(() => log4net.DebugFormat(Arg.AnyString, value, Arg.IsAny<object>()))
                .IgnoreInstance()
                .OccursOnce();

            // Act
            Debug.WriteLine(value, category);

            // Assert
            Mock.Assert(log4net);
        }

        [TestMethod]
        public void WriteLineWithValueDebugEnabledIsTrue()
        {
            // Arrange
            var log4net = Mock.Create<log4net.ILog>();
            Mock.Arrange(() => log4net.IsDebugEnabled).Returns(true);
            Mock.Arrange(() => log4net.Debug(Arg.IsAny<object>()))
                .IgnoreInstance()
                .OccursOnce();

            // Act
            Debug.WriteLine(value, category);

            // Assert
            Mock.Assert(log4net);
        }

        [TestMethod]
        public void WriteLineWithValueDebugEnabledIsFalse()
        {
            // Arrange
            var log4net = Mock.Create<log4net.ILog>();
            Mock.Arrange(() => log4net.IsDebugEnabled).Returns(false);
            Mock.Arrange(() => log4net.Debug(Arg.IsAny<object>()))
                .IgnoreInstance()
                .OccursNever();

            // Act
            Debug.WriteLine(value, category);

            // Assert
            Mock.Assert(log4net);
        }

        [TestMethod]
        public void WriteLineWithMessageExceptionDebugEnabledIsTrue()
        {
            // Arrange
            var log4net = Mock.Create<log4net.ILog>();
            Mock.Arrange(() => log4net.IsDebugEnabled).Returns(true);
            Mock.Arrange(() => LogBase.WriteException(message, ex))
                .IgnoreInstance()
                .OccursOnce();
            Mock.NonPublic.Arrange<log4net.ILog>(typeof(LogBase), "log").Returns(log4net);

            // Act
            Debug.WriteLine(message, ex);

            // Assert
            Mock.Assert(log4net);
            Mock.Assert(typeof(LogBase));
        }

        [TestMethod]
        public void WriteLineWithMessageExceptionDebugEnabledIsFalse()
        {
            // Arrange
            var log4net = Mock.Create<log4net.ILog>();
            Mock.Arrange(() => log4net.IsDebugEnabled).Returns(false);
            Mock.Arrange(() => LogBase.WriteException(message, ex))
                .IgnoreInstance()
                .OccursNever();
            Mock.NonPublic.Arrange<log4net.ILog>(typeof(LogBase), "log").Returns(log4net);

            // Act
            Debug.WriteLine(message, ex);

            // Assert
            Mock.Assert(log4net);
            Mock.Assert(typeof(LogBase));
        }

        #endregion

        #region ========== Write ==========

        [TestMethod]
        public void WriteWithMessageCategoryDebugEnabledIsTrue()
        {
            // Arrange
            var log4net = Mock.Create<log4net.ILog>();
            Mock.Arrange(() => log4net.IsDebugEnabled).Returns(true);
            Mock.Arrange(() => log4net.DebugFormat(message, category))
                .IgnoreInstance()
                .OccursOnce();

            // Act
            Debug.Write(message, category);

            // Assert
            Mock.Assert(log4net);
        }

        [TestMethod]
        public void WriteWithMessageCategoryDebugEnabledIsFalse()
        {
            // Arrange
            var log4net = Mock.Create<log4net.ILog>();
            Mock.Arrange(() => log4net.IsDebugEnabled).Returns(false);
            Mock.Arrange(() => log4net.DebugFormat(message, category))
                .IgnoreInstance()
                .OccursOnce();

            // Act
            Debug.Write(message, category);

            // Assert
            Mock.Assert(log4net);
        }

        [TestMethod]
        public void WriteWithMessageDebugEnabledIsTrue()
        {
            // Arrange
            var log4net = Mock.Create<log4net.ILog>();
            Mock.Arrange(() => log4net.IsDebugEnabled).Returns(true);
            Mock.Arrange(() => log4net.DebugFormat(message))
                .IgnoreInstance()
                .OccursOnce();

            // Act
            Debug.Write(message);

            // Assert
            Mock.Assert(log4net);
        }

        [TestMethod]
        public void WriteWithMessageDebugEnabledIsFalse()
        {
            // Arrange
            var log4net = Mock.Create<log4net.ILog>();
            Mock.Arrange(() => log4net.IsDebugEnabled).Returns(false);
            Mock.Arrange(() => log4net.DebugFormat(message))
                .IgnoreInstance()
                .OccursOnce();

            // Act
            Debug.Write(message);

            // Assert
            Mock.Assert(log4net);
        }

        [TestMethod]
        public void WriteWithValueCategoryDebugEnabledIsTrue()
        {
            // Arrange
            var log4net = Mock.Create<log4net.ILog>();
            Mock.Arrange(() => log4net.IsDebugEnabled).Returns(true);
            Mock.Arrange(() => log4net.DebugFormat(Arg.AnyString, value, Arg.IsAny<object>()))
                .IgnoreInstance()
                .OccursOnce();

            // Act
            Debug.Write(value, category);

            // Assert
            Mock.Assert(log4net);
        }

        [TestMethod]
        public void WriteWithValueCategoryDebugEnabledIsFalse()
        {
            // Arrange
            var log4net = Mock.Create<log4net.ILog>();
            Mock.Arrange(() => log4net.IsDebugEnabled).Returns(false);
            Mock.Arrange(() => log4net.DebugFormat(Arg.AnyString, value, Arg.IsAny<object>()))
                .IgnoreInstance()
                .OccursOnce();

            // Act
            Debug.Write(value, category);

            // Assert
            Mock.Assert(log4net);
        }

        #endregion
    }
}
