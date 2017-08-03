using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pipes.Core.Runners;
using Moq;
using FluentAssertions;

namespace Pipes.Core.Tests.Runners
{
    [TestClass]
    public class VersionedSegmentRunnerTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void VersionedSegmentRunner_Ctor_SegmentsAreNull_Throws()
        {
            new VersionedSegmentRunner(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void VersionedSegmentRunner_Ctor_SegmentsAreEmpty_Throws()
        {
            new VersionedSegmentRunner(new ISegment[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void VersionedSegmentRunner_Ctor_SegmentsWithSameVersion_Throws()
        {
            var segment1 = new Mock<ISegment>();
            segment1.Setup(s => s.Version).Returns(2);
            var segment2 = new Mock<ISegment>();
            segment2.Setup(s => s.Version).Returns(2);
            new VersionedSegmentRunner(new[] { segment1.Object, segment2.Object });
        }

        [TestMethod]
        public void VersionedSegmentRunner_Run_OneSegment_SegmentVersionHigherThanContextVersion_ReturnsNull()
        {
            var segment = new Mock<ISegment>();
            segment.Setup(s => s.Version).Returns(2);
            var pipelineContext = new Mock<IPipelineContext>();
            pipelineContext.Setup(c => c.MaxVersion).Returns(1);
            var runner = new VersionedSegmentRunner(new[] { segment.Object });

            var result = runner.Run(pipelineContext.Object);

            result.Should().BeNull();
        }

        [TestMethod]
        public void VersionedSegmentRunner_Run_OneSegment_RunsTheSegment()
        {
            var runResult = new object();
            var segment = new Mock<ISegment>();
            segment.Setup(s => s.Version).Returns(1);
            var pipelineContext = new Mock<IPipelineContext>();
            pipelineContext.Setup(c => c.MaxVersion).Returns(1);
            var segmentRunner = new Mock<ISegmentRunner>();
            segmentRunner.Setup(r => r.Run(pipelineContext.Object)).Returns(runResult);
            var segmentRunnerFactory = new Mock<ISegmentRunnerFactory>();
            segmentRunnerFactory.Setup(f => f.CreateSegmentRunner(segment.Object)).Returns(segmentRunner.Object);
            var runner = new VersionedSegmentRunner(new[] { segment.Object }, segmentRunnerFactory.Object);

            var result = runner.Run(pipelineContext.Object);

            result.Should().Be(runResult);
        }

        [TestMethod]
        public void VersionedSegmentRunner_Run_TwoSegments_OneValidSegment_RunsTheSegment()
        {
            var runResult = new object();
            var segment1 = new Mock<ISegment>();
            segment1.Setup(s => s.Version).Returns(1);
            var segment2 = new Mock<ISegment>();
            segment2.Setup(s => s.Version).Returns(2);
            var pipelineContext = new Mock<IPipelineContext>();
            pipelineContext.Setup(c => c.MaxVersion).Returns(1);
            var segmentRunner = new Mock<ISegmentRunner>();
            segmentRunner.Setup(r => r.Run(pipelineContext.Object)).Returns(runResult);
            var segmentRunnerFactory = new Mock<ISegmentRunnerFactory>();
            segmentRunnerFactory.Setup(f => f.CreateSegmentRunner(segment1.Object)).Returns(segmentRunner.Object);
            var runner = new VersionedSegmentRunner(new[] { segment1.Object,segment2.Object }, segmentRunnerFactory.Object);

            var result = runner.Run(pipelineContext.Object);

            result.Should().Be(runResult);
        }

        [TestMethod]
        public void VersionedSegmentRunner_Run_TwoSegments_TwoValidSegments_RunsTheSegmentWithMaxVersion()
        {
            var runResult = new object();
            var segment1 = new Mock<ISegment>();
            segment1.Setup(s => s.Version).Returns(1);
            var segment2 = new Mock<ISegment>();
            segment2.Setup(s => s.Version).Returns(2);
            var pipelineContext = new Mock<IPipelineContext>();
            pipelineContext.Setup(c => c.MaxVersion).Returns(2);
            var segmentRunner = new Mock<ISegmentRunner>();
            segmentRunner.Setup(r => r.Run(pipelineContext.Object)).Returns(runResult);
            var segmentRunnerFactory = new Mock<ISegmentRunnerFactory>();
            segmentRunnerFactory.Setup(f => f.CreateSegmentRunner(segment2.Object)).Returns(segmentRunner.Object);
            var runner = new VersionedSegmentRunner(new[] { segment1.Object, segment2.Object }, segmentRunnerFactory.Object);

            var result = runner.Run(pipelineContext.Object);

            result.Should().Be(runResult);
        }
    }
}