using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pipes.Core.Runners;
using Moq;
using FluentAssertions;

namespace Pipes.Core.Tests.Runners
{
    [TestClass]
    public class InstanceSegmentRunnerTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void InstanceSegmentRunner_Ctor_SegmentIsNull_Throws()
        {
            var methodInvoker = new Mock<ISegmentMethodInvoker>();

            new InstanceSegmentRunner(null, methodInvoker.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void InstanceSegmentRunner_Ctor_MethodInvokerIsNull_Throws()
        {
            var segment = new Mock<ISegment>();

            new InstanceSegmentRunner(segment.Object, null);
        }

        [TestMethod]
        public void InstanceSegmentRunner_Run_SegmentVersionHigherThanContextVersion_ReturnsNull()
        {
            var methodInvoker = new Mock<ISegmentMethodInvoker>();
            var segment = new Mock<ISegment>();
            segment.Setup(s => s.Version).Returns(3);
            var pipelineContext = new Mock<IPipelineContext>();
            pipelineContext.Setup(c => c.MaxVersion).Returns(2);
            var runner = new InstanceSegmentRunner(segment.Object, methodInvoker.Object);

            var result = runner.Run(pipelineContext.Object);

            result.Should().BeNull();
        }

        [TestMethod]
        public void InstanceSegmentRunner_Run_SegmentVersionEqualsContextVersion_CanRunIsFalse_ReturnsNull()
        {
            var segment = new Mock<ISegment>();
            segment.Setup(s => s.Version).Returns(1);
            var pipelineContext = new Mock<IPipelineContext>();
            pipelineContext.Setup(c => c.MaxVersion).Returns(1);
            var methodInvoker = new Mock<ISegmentMethodInvoker>();
            methodInvoker
                .Setup(mi => mi.InvokeMethod(segment.Object, "CanRun", pipelineContext.Object, true))
                .Returns(false);
            var runner = new InstanceSegmentRunner(segment.Object, methodInvoker.Object);

            var result = runner.Run(pipelineContext.Object);

            result.Should().BeNull();
        }

        [TestMethod]
        public void InstanceSegmentRunner_Run_SegmentVersionLowerThanContextVersion_CanRunIsFalse_ReturnsNull()
        {
            var segment = new Mock<ISegment>();
            segment.Setup(s => s.Version).Returns(1);
            var pipelineContext = new Mock<IPipelineContext>();
            pipelineContext.Setup(c => c.MaxVersion).Returns(2);
            var methodInvoker = new Mock<ISegmentMethodInvoker>();
            methodInvoker
                .Setup(mi => mi.InvokeMethod(segment.Object, "CanRun", pipelineContext.Object, true))
                .Returns(false);
            var runner = new InstanceSegmentRunner(segment.Object, methodInvoker.Object);

            var result = runner.Run(pipelineContext.Object);

            result.Should().BeNull();
        }

        [TestMethod]
        public void InstanceSegmentRunner_Run_SegmentVersionEqualsContextVersion_CanRunIsTrue_ReturnsRunResult()
        {
            var runResult = new object();
            var segment = new Mock<ISegment>();
            segment.Setup(s => s.Version).Returns(1);
            var pipelineContext = new Mock<IPipelineContext>();
            pipelineContext.Setup(c => c.MaxVersion).Returns(1);
            var methodInvoker = new Mock<ISegmentMethodInvoker>();
            methodInvoker
                .Setup(mi => mi.InvokeMethod(segment.Object, "CanRun", pipelineContext.Object, true))
                .Returns(true);
            methodInvoker
                .Setup(mi => mi.InvokeMethod<object>(segment.Object, "Run", pipelineContext.Object, null))
                .Returns(runResult);
            var runner = new InstanceSegmentRunner(segment.Object, methodInvoker.Object);

            var result = runner.Run(pipelineContext.Object);

            result.Should().Be(runResult);
        }

        [TestMethod]
        public void InstanceSegmentRunner_Run_SegmentVersionLowerThanContextVersion_CanRunIsTrue_ReturnsRunResult()
        {
            var runResult = new object();
            var segment = new Mock<ISegment>();
            segment.Setup(s => s.Version).Returns(1);
            var pipelineContext = new Mock<IPipelineContext>();
            pipelineContext.Setup(c => c.MaxVersion).Returns(2);
            var methodInvoker = new Mock<ISegmentMethodInvoker>();
            methodInvoker
                .Setup(mi => mi.InvokeMethod(segment.Object, "CanRun", pipelineContext.Object, true))
                .Returns(true);
            methodInvoker
                .Setup(mi => mi.InvokeMethod<object>(segment.Object, "Run", pipelineContext.Object, null))
                .Returns(runResult);
            var runner = new InstanceSegmentRunner(segment.Object, methodInvoker.Object);

            var result = runner.Run(pipelineContext.Object);

            result.Should().Be(runResult);
        }
    }
}
