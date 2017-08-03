using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pipes.Core.Runners;

namespace Pipes.Core.Tests
{
    [TestClass]
    public class PipelineBuilderTests
    {
        private class TestSegment : SegmentBase
        { }

        [TestMethod]
        public void PipelineBuilder_Ctor_ValidName_AddsToRepository()
        {
            const string pipelineName = "Pipeline";
            var pipelineRepository = new Mock<IPipelineRepository>();
            var segmentRunnerFactory = new Mock<ISegmentRunnerFactory>();

            new PipelineBuilder(pipelineName, pipelineRepository.Object, segmentRunnerFactory.Object);

            pipelineRepository.Verify(r => r.CreateNew(pipelineName));
        }

        [TestMethod]
        public void PipelineBuilder_Do_ByType_AddsSegmentToPipeline()
        {
            const string pipelineName = "Pipeline";
            var pipeline = new Mock<IPipeline>();
            var pipelineRepository = new Mock<IPipelineRepository>();
            pipelineRepository.Setup(r => r.CreateNew(pipelineName)).Returns(pipeline.Object);
            var segmentRunner= new Mock<ISegmentRunner>();
            var segmentRunnerFactory = new Mock<ISegmentRunnerFactory>();
            segmentRunnerFactory.Setup(f => f.CreateSegmentRunner<TestSegment>()).Returns(segmentRunner.Object);

            new PipelineBuilder(pipelineName, pipelineRepository.Object, segmentRunnerFactory.Object).
                Do<TestSegment>();

            pipelineRepository.Verify(r => r.CreateNew(pipelineName));
            pipeline.Verify(p => p.AddSegmentRunner(segmentRunner.Object));
        }

        [TestMethod]
        public void PipelineBuilder_Do_ByInstance_AddsSegmentToPipeline()
        {
            var segment = new TestSegment();
            const string pipelineName = "Pipeline";
            var pipeline = new Mock<IPipeline>();
            var pipelineRepository = new Mock<IPipelineRepository>();
            pipelineRepository.Setup(r => r.CreateNew(pipelineName)).Returns(pipeline.Object);
            var segmentRunner = new Mock<ISegmentRunner>();
            var segmentRunnerFactory = new Mock<ISegmentRunnerFactory>();
            segmentRunnerFactory.Setup(f => f.CreateSegmentRunner(segment)).Returns(segmentRunner.Object);

            new PipelineBuilder(pipelineName, pipelineRepository.Object, segmentRunnerFactory.Object).
                Do(segment);

            pipelineRepository.Verify(r => r.CreateNew(pipelineName));
            pipeline.Verify(p => p.AddSegmentRunner(segmentRunner.Object));
        }

        [TestMethod]
        public void PipelineBuilder_DoByVersion_AddsSegmentToPipeline()
        {
            var segment = new TestSegment();
            const string pipelineName = "Pipeline";
            var pipeline = new Mock<IPipeline>();
            var pipelineRepository = new Mock<IPipelineRepository>();
            pipelineRepository.Setup(r => r.CreateNew(pipelineName)).Returns(pipeline.Object);
            var segmentRunner = new Mock<ISegmentRunner>();
            var segmentRunnerFactory = new Mock<ISegmentRunnerFactory>();
            segmentRunnerFactory.Setup(f => f.CreateVersionedRunner(segment)).Returns(segmentRunner.Object);

            new PipelineBuilder(pipelineName, pipelineRepository.Object, segmentRunnerFactory.Object).
                DoByVersion(segment);

            pipelineRepository.Verify(r => r.CreateNew(pipelineName));
            pipeline.Verify(p => p.AddSegmentRunner(segmentRunner.Object));
        }

        [TestMethod]
        public void PipelineBuilder_DoPipeline_AddsSegmentToPipeline()
        {
            const string pipelineName = "Pipeline";
            const string childPipelineName = "ChildPipeline";
            var pipeline = new Mock<IPipeline>();
            var pipelineRepository = new Mock<IPipelineRepository>();
            pipelineRepository.Setup(r => r.CreateNew(pipelineName)).Returns(pipeline.Object);
            var segmentRunner = new Mock<ISegmentRunner>();
            var segmentRunnerFactory = new Mock<ISegmentRunnerFactory>();
            segmentRunnerFactory.Setup(f => f.CreatePipelineRunner(childPipelineName, pipelineRepository.Object))
                .Returns(segmentRunner.Object);

            new PipelineBuilder(pipelineName, pipelineRepository.Object, segmentRunnerFactory.Object).
                DoPipeline(childPipelineName);

            pipelineRepository.Verify(r => r.CreateNew(pipelineName));
            pipeline.Verify(p => p.AddSegmentRunner(segmentRunner.Object));
        }
    }
}
