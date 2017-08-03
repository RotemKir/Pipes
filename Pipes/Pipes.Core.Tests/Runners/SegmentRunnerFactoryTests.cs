using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pipes.Core.Runners;
using Moq;
using FluentAssertions;

namespace Pipes.Core.Tests.Runners
{
    [TestClass]
    public class SegmentRunnerFactoryTests
    {
        private class TestSegment : SegmentBase
        { }

        [TestMethod]
        public void SegmentRunnerFactory_CreateSegmentRunner_ByType_ReturnsNewRunner()
        {
            var factory = new SegmentRunnerFactory();

            var runner = factory.CreateSegmentRunner<TestSegment>();

            runner.Should().NotBeNull().And.BeOfType<InstanceSegmentRunner>();
        }

        [TestMethod]
        public void SegmentRunnerFactory_CreateSegmentRunner_ByInstance_ReturnsNewRunner()
        {
            var segment = new Mock<ISegment>();
            var factory = new SegmentRunnerFactory();

            var runner = factory.CreateSegmentRunner(segment.Object);

            runner.Should().NotBeNull().And.BeOfType<InstanceSegmentRunner>();
        }

        [TestMethod]
        public void SegmentRunnerFactory_CreateVersionedRunner_ReturnsNewRunner()
        {
            var segment1 = new Mock<ISegment>();
            segment1.Setup(s => s.Version).Returns(1);
            var segment2 = new Mock<ISegment>();
            segment2.Setup(s => s.Version).Returns(2);
            var factory = new SegmentRunnerFactory();

            var runner = factory.CreateVersionedRunner(segment1.Object, segment2.Object);

            runner.Should().NotBeNull().And.BeOfType<VersionedSegmentRunner>();
        }

        [TestMethod]
        public void SegmentRunnerFactory_CreatePipelineRunner_ReturnsNewRunner()
        {
            var pipelineName = "Pipeline";
            var pipelineRepository = new Mock<IPipelineRepository>();
            var factory = new SegmentRunnerFactory();

            var runner = factory.CreatePipelineRunner(pipelineName, pipelineRepository.Object);

            runner.Should().NotBeNull().And.BeOfType<PipelineSegmentRunner>();
        }
    }
}
