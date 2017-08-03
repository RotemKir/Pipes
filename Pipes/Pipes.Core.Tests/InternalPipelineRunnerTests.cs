using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pipes.Core.Runners;
using System;

namespace Pipes.Core.Tests
{
    [TestClass]
    public class InternalPipelineRunnerTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void InternalPipelineRunner_Run_NullPipeliine_Throws()
        {
            var pipelineContext = new Mock<IPipelineContext>();
            var runner = new InternalPipelineRunner();

            runner.Run(null, pipelineContext.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void InternalPipelineRunner_Run_NullPipeliineContext_Throws()
        {
            var pipeline = new Mock<IPipeline>();
            var runner = new InternalPipelineRunner();

            runner.Run(pipeline.Object, null);
        }

        [TestMethod]
        public void InternalPipelineRunner_Run_PipelineHasNoSegments_ReturnsNull()
        {
            var pipeline = new Mock<IPipeline>();
            pipeline.Setup(p => p.SegmentRunners).Returns(new ISegmentRunner[0]);
            var pipelineContext = new Mock<IPipelineContext>();
            var runner = new InternalPipelineRunner();

            var result = runner.Run(pipeline.Object, pipelineContext.Object);

            result.Should().BeNull();
        }

        [TestMethod]
        public void InternalPipelineRunner_Run_SegmentReturnsNull_ReturnsNull()
        {
            var pipelineContext = new Mock<IPipelineContext>();
            var segmentRunner = new Mock<ISegmentRunner>();
            segmentRunner.Setup(s => s.Run(pipelineContext.Object)).Returns(null);
            var segmentRunners = new[] { segmentRunner.Object };
            var pipeline = new Mock<IPipeline>();
            pipeline.Setup(p => p.SegmentRunners).Returns(segmentRunners);
            var runner = new InternalPipelineRunner();

            var result = runner.Run(pipeline.Object, pipelineContext.Object);

            result.Should().BeNull();
        }

        [TestMethod]
        public void InternalPipelineRunner_Run_SegmentReturnsValue_ReturnsValue()
        {
            var runResult = new object();
            var pipelineContext = new Mock<IPipelineContext>();
            var segmentRunner = new Mock<ISegmentRunner>();
            segmentRunner.Setup(s => s.Run(pipelineContext.Object)).Returns(runResult);
            var segmentRunners = new[] { segmentRunner.Object };
            var pipeline = new Mock<IPipeline>();
            pipeline.Setup(p => p.SegmentRunners).Returns(segmentRunners);
            var runner = new InternalPipelineRunner();

            var result = runner.Run(pipeline.Object, pipelineContext.Object);

            result.Should().Be(runResult);
        }

        [TestMethod]
        public void InternalPipelineRunner_Run_SegmentReturnsValue_SegmentReturnsNull_ReturnsValue()
        {
            var runResult = new { a = 1 };
            var pipelineContext = new Mock<IPipelineContext>();
            var segmentRunner1 = new Mock<ISegmentRunner>();
            segmentRunner1.Setup(s => s.Run(pipelineContext.Object)).Returns(runResult);
            var segmentRunner2 = new Mock<ISegmentRunner>();
            segmentRunner2.Setup(s => s.Run(pipelineContext.Object)).Returns(null);
            var segmentRunners = new[] { segmentRunner1.Object,segmentRunner2.Object };
            var pipeline = new Mock<IPipeline>();
            pipeline.Setup(p => p.SegmentRunners).Returns(segmentRunners);
            var runner = new InternalPipelineRunner();

            var result = runner.Run(pipeline.Object, pipelineContext.Object);

            result.Should().Be(runResult);
            pipelineContext.Verify(c => c.SetValue(runResult));
        }

        [TestMethod]
        public void InternalPipelineRunner_Run_SegmentReturnsValue_SegmentReturnsValue_ReturnsLastValue()
        {
            var runResult1 = new object();
            var runResult2 = new object();
            var pipelineContext = new Mock<IPipelineContext>();
            var segmentRunner1 = new Mock<ISegmentRunner>();
            segmentRunner1.Setup(s => s.Run(pipelineContext.Object)).Returns(runResult1);
            var segmentRunner2 = new Mock<ISegmentRunner>();
            segmentRunner2.Setup(s => s.Run(pipelineContext.Object)).Returns(runResult2);
            var segmentRunners = new[] { segmentRunner1.Object,segmentRunner2.Object };
            var pipeline = new Mock<IPipeline>();
            pipeline.Setup(p => p.SegmentRunners).Returns(segmentRunners);
            var runner = new InternalPipelineRunner();

            var result = runner.Run(pipeline.Object, pipelineContext.Object);

            result.Should().Be(runResult2);
            pipelineContext.Verify(c => c.SetValue(runResult1));
            pipelineContext.Verify(c => c.SetValue(runResult2));
        }

    }
}
