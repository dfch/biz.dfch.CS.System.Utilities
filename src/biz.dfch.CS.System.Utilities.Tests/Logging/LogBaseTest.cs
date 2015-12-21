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
    public class LogBaseTest
    {
        [TestMethod]
        public void WriteExceptionWithErrorEnabledIsTrueWillBeCalled()
        {
            //Arrange
            var message = "some-arbitrary-message";
            var ex = new Exception(message);

            var log4net = Mock.Create<log4net.ILog>();
            Mock.Arrange(() => log4net.IsErrorEnabled).Returns(true);
            Mock.Arrange(() => log4net.ErrorFormat(Arg.AnyString, ex.GetType().Name, ex.Source, message, ex.Message, Arg.AnyString))
                .IgnoreInstance()
                .OccursOnce();

            //Act
            LogBase.WriteException(message, ex);

            //Assert
            Mock.Assert(log4net);
        }

        [TestMethod]
        public void WriteExceptionWithErrorEnabledIsFalseWillNotBeCalled()
        {
            //Arrange
            var message = "some-arbitrary-message";
            var ex = new Exception(message);

            var log4net = Mock.Create<log4net.ILog>();
            Mock.Arrange(() => log4net.IsErrorEnabled).Returns(false);
            Mock.Arrange(() => log4net.ErrorFormat(Arg.AnyString, Arg.AnyString, Arg.AnyString, Arg.AnyString, Arg.AnyString, Arg.AnyString))
                .IgnoreInstance()
                .OccursNever();

            //Act
            LogBase.WriteException(message, ex);

            //Assert
            Mock.Assert(log4net);
        }
    }
}
