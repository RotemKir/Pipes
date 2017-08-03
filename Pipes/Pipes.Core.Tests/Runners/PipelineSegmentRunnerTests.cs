using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pipes.Core.Runners;
using Moq;
using FluentAssertions;

namespace Pipes.Core.Tests.Runners
{
    [TestClass]
    public class PipelineSegmentRunnerTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PipelineSegmentRunner_Ctor_PiplineNameIsNull_Throws()
        {
            var pipelineRepository = new Mock<IPipelineRepository>();
            var internalPipelineRunner = new Mock<IInternalPipelineRunner>();

            new PipelineSegmentRunner(null, pipelineRepository.Object, internalPipelineRunner.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PipelineSegmentRunner_Ctor_PiplineNameIsEmpty_Throws()
        {
            var pipelineRepository = new Mock<IPipelineRepository>();
            var internalPipelineRunner = new Mock<IInternalPipelineRunner>();

            new PipelineSegmentRunner(string.Empty, pipelineRepository.Object, internalPipelineRunner.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PipelineSegmentRunner_Ctor_PipelineRepositoryIsNull_Throws()
        {
            var internalPipelineRunner = new Mock<IInternalPipelineRunner>();

            new PipelineSegmentRunner("Pipeline", null, internalPipelineRunner.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PipelineSegmentRunner_Ctor_PiplineRunnerIsNull_Throws()
        {
            var pipelineRepository = new Mock<IPipelineRepository>();
            var internalPipelineRunner = new Mock<IInternalPipelineRunner>();

            new PipelineSegmentRunner("Pipline", pipelineRepository.Object, null);
        }

        [TestMethod]
        public void PipelineSegmentRunner_Run_PipelineDoesntExist_CallsRunner()
        {
            var runResult = new object();
            var pipeline = new Mock<IPipeline>();
            var pipelineContext = new Mock<IPipelineContext>();
            var pipelineRepository = new Mock<IPipelineRepository>();
            pipelineRepository.Setup(r => r.Get("Pipeline")).Returns(pipeline.Object);
            var internalPipelineRunner = new Mock<IInternalPipelineRunner>();
            internalPipelineRunner.Setup(r => r.Run(pipeline.Object, pipelineContext.Object)).Returns(runResult);
            var runner = new PipelineSegmentRunner("Pipeline",
                pipelineRepository.Object,
                internalPipelineRunner.Object);

            var result = runner.Run(pipelineContext.Object);

            result.Should().Be(runResult);
        }
    }
}
