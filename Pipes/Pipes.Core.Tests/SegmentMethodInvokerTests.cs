using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace Pipes.Core.Tests
{
    [TestClass]
    public class SegmentMethodInvokerTests
    {
        public interface ITestSegment : ISegment
        {
            void VoidMethod();
            void MethodWithParams(int number, string text);
            int IntMethod();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SegmentMethodInvoker_SegmentIsNull_Throws()
        {
            var methodInvoker = new SegmentMethodInvoker();
            ISegment segment = null;
            var pipelineContext = new Mock<IPipelineContext>();

            methodInvoker.InvokeMethod(segment, "Method", pipelineContext.Object, 5);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SegmentMethodInvoker_MethodIsNull_Throws()
        {
            var methodInvoker = new SegmentMethodInvoker();
            var segment = new Mock<ISegment>();
            var pipelineContext = new Mock<IPipelineContext>();

            methodInvoker.InvokeMethod(segment.Object, null, pipelineContext.Object, 5);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SegmentMethodInvoker_MethodIsEmpty_Throws()
        {
            var methodInvoker = new SegmentMethodInvoker();
            var segment = new Mock<ISegment>();
            var pipelineContext = new Mock<IPipelineContext>();

            methodInvoker.InvokeMethod(segment.Object, string.Empty, pipelineContext.Object, 5);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SegmentMethodInvoker_PipelineContextIsNull_Throws()
        {
            var methodInvoker = new SegmentMethodInvoker();
            var segment = new Mock<ISegment>();

            methodInvoker.InvokeMethod(segment.Object, "Hello", null, 5);
        }

        [TestMethod]
        public void SegmentMethodInvoker_MethodDoesntExist_ReturnsDefaultValue()
        {
            var methodInvoker = new SegmentMethodInvoker();
            var segment = new Mock<ISegment>();
            var pipelineContext = new Mock<IPipelineContext>();
            const int defaultValue = 5;

            var returnValue = methodInvoker.InvokeMethod(segment.Object, "Hello", pipelineContext.Object, defaultValue);

            returnValue.Should().Be(defaultValue);
        }

        [TestMethod]
        public void SegmentMethodInvoker_VoidMethod_ExpectingInt_ReturnsZero()
        {
            var methodInvoker = new SegmentMethodInvoker();
            var segment = new Mock<ITestSegment>();
            var pipelineContext = new Mock<IPipelineContext>();
            const int defaultValue = 5;

            var returnValue = methodInvoker.InvokeMethod(segment.Object, "VoidMethod", pipelineContext.Object, defaultValue);

            returnValue.Should().Be(defaultValue);
        }

        [TestMethod]
        public void SegmentMethodInvoker_VoidMethod_ExpectingObject_ReturnsDefualtValue()
        {
            var methodInvoker = new SegmentMethodInvoker();
            var segment = new Mock<ITestSegment>();
            var pipelineContext = new Mock<IPipelineContext>();
            var defaultValue = new object();

            var returnValue = methodInvoker.InvokeMethod(segment.Object, "VoidMethod", pipelineContext.Object, defaultValue);

            returnValue.Should().Be(defaultValue);
        }

        [TestMethod]
        public void SegmentMethodInvoker_MethodWithParams_NoNumber_NoText_CallsWithZeroAndNull()
        {
            var methodInvoker = new SegmentMethodInvoker();
            var segment = new Mock<ITestSegment>();
            var pipelineContext = new Mock<IPipelineContext>();
            var defaultValue = new object();

            var returnValue = methodInvoker.InvokeMethod(segment.Object, "MethodWithParams", pipelineContext.Object, defaultValue);

            segment.Verify(s => s.MethodWithParams(0, null));
        }

        [TestMethod]
        public void SegmentMethodInvoker_MethodWithParams_HasNumber_NoText_CallsWithNumberAndNull()
        {
            var methodInvoker = new SegmentMethodInvoker();
            var segment = new Mock<ITestSegment>();
            var pipelineContext = new Mock<IPipelineContext>();
            const int numberParam = 34;
            pipelineContext.Setup(s => s.GetValue("number")).Returns(numberParam);
            var defaultValue = new object();

            var returnValue = methodInvoker.InvokeMethod(segment.Object, "MethodWithParams", pipelineContext.Object, defaultValue);

            segment.Verify(s => s.MethodWithParams(numberParam, null));
        }

        [TestMethod]
        public void SegmentMethodInvoker_MethodWithParams_NoNumber_HasText_CallsWithZeroAndText()
        {
            var methodInvoker = new SegmentMethodInvoker();
            var segment = new Mock<ITestSegment>();
            var pipelineContext = new Mock<IPipelineContext>();
            var text = "Hello";
            pipelineContext.Setup(s => s.GetValue("text")).Returns(text);
            var defaultValue = new object();

            var returnValue = methodInvoker.InvokeMethod(segment.Object, "MethodWithParams", pipelineContext.Object, defaultValue);

            segment.Verify(s => s.MethodWithParams(0, text));
        }

        [TestMethod]
        public void SegmentMethodInvoker_MethodWithParams_HasNumber_HasText_CallsWithNumberAndText()
        {
            var methodInvoker = new SegmentMethodInvoker();
            var segment = new Mock<ITestSegment>();
            var pipelineContext = new Mock<IPipelineContext>();
            const int numberParam = 34;
            var text = "Hello";
            pipelineContext.Setup(s => s.GetValue("text")).Returns(text);
            pipelineContext.Setup(s => s.GetValue("number")).Returns(numberParam);
            var defaultValue = new object();

            var returnValue = methodInvoker.InvokeMethod(segment.Object, "MethodWithParams", pipelineContext.Object, defaultValue);

            segment.Verify(s => s.MethodWithParams(numberParam, text));
        }

        [TestMethod]
        public void SegmentMethodInvoker_IntMethod_ReturnsInt()
        {
            var methodInvoker = new SegmentMethodInvoker();
            var segment = new Mock<ITestSegment>();
            const int methodReturnValue = 34;
            segment.Setup(s => s.IntMethod()).Returns(methodReturnValue);
            var pipelineContext = new Mock<IPipelineContext>();

            var returnValue = methodInvoker.InvokeMethod(segment.Object, "IntMethod", pipelineContext.Object, 0);

            returnValue.Should().Be(methodReturnValue);
        }

        [TestMethod]
        public void SegmentMethodInvoker_IntMethod_ExpectsString_ReturnsDefaultValue()
        {
            var methodInvoker = new SegmentMethodInvoker();
            var segment = new Mock<ITestSegment>();
            const int methodReturnValue = 34;
            segment.Setup(s => s.IntMethod()).Returns(methodReturnValue);
            var pipelineContext = new Mock<IPipelineContext>();
            var defaultValue = "Hello";

            var returnValue = methodInvoker.InvokeMethod(segment.Object, "IntMethod", pipelineContext.Object, defaultValue);

            returnValue.Should().Be(defaultValue);
        }
    }
}