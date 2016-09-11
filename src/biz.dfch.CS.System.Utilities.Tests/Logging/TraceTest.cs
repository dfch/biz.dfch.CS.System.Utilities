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
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telerik.JustMock;
using biz.dfch.CS.Utilities.Logging;

namespace biz.dfch.CS.Utilities.Tests.Logging
{
    [TestClass]
    public class TraceTest
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

        [TestMethod]
        public void AssertWithConditionMessageDetailMessage()
        {
            // Arrange
            Mock.SetupStatic(typeof(System.Diagnostics.Trace));
            Mock.Arrange(() => System.Diagnostics.Trace.Assert(condition, message, detailMessage)).OccursOnce();

            // Act
            Trace.Assert(condition, message, detailMessage);

            // Assert
            Mock.Assert(() => System.Diagnostics.Trace.Assert(condition, message, detailMessage));
        }

        [TestMethod]
        public void AssertWithConditionMessage()
        {
            // Arrange
            Mock.SetupStatic(typeof(System.Diagnostics.Trace));
            Mock.Arrange(() => System.Diagnostics.Trace.Assert(condition, message)).OccursOnce();

            // Act
            Trace.Assert(condition, message);

            // Assert
            Mock.Assert(() => System.Diagnostics.Trace.Assert(condition, message));
        }

        [TestMethod]
        public void AssertWithCondition()
        {
            // Arrange
            Mock.SetupStatic(typeof(System.Diagnostics.Trace));
            Mock.Arrange(() => System.Diagnostics.Trace.Assert(condition)).OccursOnce();

            // Act
            Trace.Assert(condition);

            // Assert
            Mock.Assert(() => System.Diagnostics.Trace.Assert(condition));
        }

        #endregion

        # region ========== WriteLine ==========

        [TestMethod]
        public void WriteLineWithMessageParamArgsInfoEnabledIsTrue()
        {
            // Arrange
            var log4net = Mock.Create<log4net.ILog>();
            Mock.Arrange(() => log4net.IsInfoEnabled).Returns(true);
            Mock.Arrange(() => log4net.InfoFormat(message, arg0, arg1, arg2))
                .IgnoreInstance()
                .OccursOnce();

            // Act
            Trace.WriteLine(message, arg0, arg1, arg2);

            // Assert
            Mock.Assert(log4net);
        }

        [TestMethod]
        public void WriteLineWithMessageParamArgsInfoEnabledIsFalse()
        {
            // Arrange
            var log4net = Mock.Create<log4net.ILog>();
            Mock.Arrange(() => log4net.IsInfoEnabled).Returns(false);
            Mock.Arrange(() => log4net.InfoFormat(message, arg0, arg1, arg2))
                .IgnoreInstance()
                .OccursOnce();

            // Act
            Trace.WriteLine(message, arg0, arg1, arg2);

            // Assert
            Mock.Assert(log4net);
        }

        [TestMethod]
        public void WriteLineWithMessageCategoryInfoEnabledIsTrue()
        {
            // Arrange
            var log4net = Mock.Create<log4net.ILog>();
            Mock.Arrange(() => log4net.IsInfoEnabled).Returns(true);
            Mock.Arrange(() => log4net.InfoFormat(message, category))
                .IgnoreInstance()
                .OccursOnce();

            // Act
            Trace.WriteLine(message, category);

            // Assert
            Mock.Assert(log4net);
        }

        [TestMethod]
        public void WriteLineWithMessageCategoryInfoEnabledIsFalse()
        {
            // Arrange
            var log4net = Mock.Create<log4net.ILog>();
            Mock.Arrange(() => log4net.IsInfoEnabled).Returns(false);
            Mock.Arrange(() => log4net.InfoFormat(message, category))
                .IgnoreInstance()
                .OccursOnce();

            // Act
            Trace.WriteLine(message, category);

            // Assert
            Mock.Assert(log4net);
        }

        [TestMethod]
        public void WriteLineWithMessageInfoEnabledIsTrue()
        {
            // Arrange
            var log4net = Mock.Create<log4net.ILog>();
            Mock.Arrange(() => log4net.IsInfoEnabled).Returns(true);
            Mock.Arrange(() => log4net.InfoFormat(message))
                .IgnoreInstance()
                .OccursOnce();

            // Act
            Trace.WriteLine(message);

            // Assert
            Mock.Assert(log4net);
        }

        [TestMethod]
        public void WriteLineWithMessageInfoEnabledIsFalse()
        {
            // Arrange
            var log4net = Mock.Create<log4net.ILog>();
            Mock.Arrange(() => log4net.IsInfoEnabled).Returns(false);
            Mock.Arrange(() => log4net.InfoFormat(message))
                .IgnoreInstance()
                .OccursOnce();

            // Act
            Trace.WriteLine(message);

            // Assert
            Mock.Assert(log4net);
        }

        [TestMethod]
        public void WriteLineWithValueCategoryInfoEnabledIsTrue()
        {
            // Arrange
            var log4net = Mock.Create<log4net.ILog>();
            Mock.Arrange(() => log4net.IsInfoEnabled).Returns(true);
            Mock.Arrange(() => log4net.InfoFormat(Arg.AnyString, value, Arg.IsAny<object>()))
                .IgnoreInstance()
                .OccursOnce();

            // Act
            Trace.WriteLine(value, category);

            // Assert
            Mock.Assert(log4net);
        }

        [TestMethod]
        public void WriteLineWithValueCategoryInfoEnabledIsFalse()
        {
            // Arrange
            var log4net = Mock.Create<log4net.ILog>();
            Mock.Arrange(() => log4net.IsInfoEnabled).Returns(false);
            Mock.Arrange(() => log4net.InfoFormat(Arg.AnyString, value, Arg.IsAny<object>()))
                .IgnoreInstance()
                .OccursOnce();

            // Act
            Trace.WriteLine(value, category);

            // Assert
            Mock.Assert(log4net);
        }

        [TestMethod]
        public void WriteLineWithValueInfoEnabledIsTrue()
        {
            // Arrange
            var log4net = Mock.Create<log4net.ILog>();
            Mock.Arrange(() => log4net.IsInfoEnabled).Returns(true);
            Mock.Arrange(() => log4net.Info(Arg.IsAny<object>()))
                .IgnoreInstance()
                .OccursOnce();

            // Act
            Trace.WriteLine(value, category);

            // Assert
            Mock.Assert(log4net);
        }

        [TestMethod]
        public void WriteLineWithValueInfoEnabledIsFalse()
        {
            // Arrange
            var log4net = Mock.Create<log4net.ILog>();
            Mock.Arrange(() => log4net.IsInfoEnabled).Returns(false);
            Mock.Arrange(() => log4net.Info(Arg.IsAny<object>()))
                .IgnoreInstance()
                .OccursNever();

            // Act
            Trace.WriteLine(value, category);

            // Assert
            Mock.Assert(log4net);
        }

        [TestMethod]
        public void WriteLineWithMessageExceptionInfoEnabledIsTrue()
        {
            // Arrange
            var log4net = Mock.Create<log4net.ILog>();
            Mock.Arrange(() => log4net.IsInfoEnabled).Returns(true);
            Mock.Arrange(() => LogBase.WriteException(message, ex))
                .IgnoreInstance()
                .OccursOnce();
            Mock.NonPublic.Arrange<log4net.ILog>(typeof(LogBase), "log").Returns(log4net);

            // Act
            Trace.WriteLine(message, ex);

            // Assert
            Mock.Assert(log4net);
            Mock.Assert(typeof(LogBase));
        }

        [TestMethod]
        public void WriteLineWithMessageExceptionInfoEnabledIsFalse()
        {
            // Arrange
            var log4net = Mock.Create<log4net.ILog>();
            Mock.Arrange(() => log4net.IsInfoEnabled).Returns(false);
            Mock.Arrange(() => LogBase.WriteException(message, ex))
                .IgnoreInstance()
                .OccursNever();
            Mock.NonPublic.Arrange<log4net.ILog>(typeof(LogBase), "log").Returns(log4net);

            // Act
            Trace.WriteLine(message, ex);

            // Assert
            Mock.Assert(log4net);
            Mock.Assert(typeof(LogBase));
        }

        #endregion

        #region ========== Write ==========

        [TestMethod]
        public void WriteWithMessageCategoryInfoEnabledIsTrue()
        {
            // Arrange
            var log4net = Mock.Create<log4net.ILog>();
            Mock.Arrange(() => log4net.IsInfoEnabled).Returns(true);
            Mock.Arrange(() => log4net.InfoFormat(message, category))
                .IgnoreInstance()
                .OccursOnce();

            // Act
            Trace.Write(message, category);

            // Assert
            Mock.Assert(log4net);
        }

        [TestMethod]
        public void WriteWithMessageCategoryInfoEnabledIsFalse()
        {
            // Arrange
            var log4net = Mock.Create<log4net.ILog>();
            Mock.Arrange(() => log4net.IsInfoEnabled).Returns(false);
            Mock.Arrange(() => log4net.InfoFormat(message, category))
                .IgnoreInstance()
                .OccursOnce();

            // Act
            Trace.Write(message, category);

            // Assert
            Mock.Assert(log4net);
        }

        [TestMethod]
        public void WriteWithMessageInfoEnabledIsTrue()
        {
            // Arrange
            var log4net = Mock.Create<log4net.ILog>();
            Mock.Arrange(() => log4net.IsInfoEnabled).Returns(true);
            Mock.Arrange(() => log4net.InfoFormat(message))
                .IgnoreInstance()
                .OccursOnce();

            // Act
            Trace.Write(message);

            // Assert
            Mock.Assert(log4net);
        }

        [TestMethod]
        public void WriteWithMessageInfoEnabledIsFalse()
        {
            // Arrange
            var log4net = Mock.Create<log4net.ILog>();
            Mock.Arrange(() => log4net.IsInfoEnabled).Returns(false);
            Mock.Arrange(() => log4net.InfoFormat(message))
                .IgnoreInstance()
                .OccursOnce();

            // Act
            Trace.Write(message);

            // Assert
            Mock.Assert(log4net);
        }

        [TestMethod]
        public void WriteWithValueCategoryInfoEnabledIsTrue()
        {
            // Arrange
            var log4net = Mock.Create<log4net.ILog>();
            Mock.Arrange(() => log4net.IsInfoEnabled).Returns(true);
            Mock.Arrange(() => log4net.InfoFormat(Arg.AnyString, value, Arg.IsAny<object>()))
                .IgnoreInstance()
                .OccursOnce();

            // Act
            Trace.Write(value, category);

            // Assert
            Mock.Assert(log4net);
        }

        [TestMethod]
        public void WriteWithValueCategoryInfoEnabledIsFalse()
        {
            // Arrange
            var log4net = Mock.Create<log4net.ILog>();
            Mock.Arrange(() => log4net.IsInfoEnabled).Returns(false);
            Mock.Arrange(() => log4net.InfoFormat(Arg.AnyString, value, Arg.IsAny<object>()))
                .IgnoreInstance()
                .OccursOnce();

            // Act
            Trace.Write(value, category);

            // Assert
            Mock.Assert(log4net);
        }

        #endregion
    }
}
