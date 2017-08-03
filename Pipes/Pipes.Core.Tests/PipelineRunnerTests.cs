using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Pipes.Core.Tests
{
    [TestClass]
    public class PipelineRunnerTests
    {
        [TestMethod]
        public void Run_Void_NoInitialValues_CallsInternalRunner()
        {
            const string pipelineName = "Pipeline";
            var pipeline = new Mock<IPipeline>();
            var pipelineRepository = new Mock<IPipelineRepository>();
            pipelineRepository.Setup(r => r.Get(pipelineName)).Returns(pipeline.Object);
            var pipelineContext = new Mock<IPipelineContext>();
            var internalPipelineRunner = new Mock<IInternalPipelineRunner>();
            internalPipelineRunner
                .Setup(r => r.Run(pipeline.Object, pipelineContext.Object))
                .Callback(() => pipelineContext.Verify(c => c.SetValue(null)));

            var runner = new PipelineRunner(pipelineName,
                pipelineRepository.Object,
                internalPipelineRunner.Object,
                pipelineContext.Object);

            runner.Run();
            
            internalPipelineRunner.Verify(r => r.Run(pipeline.Object, pipelineContext.Object));
        }

        [TestMethod]
        public void Run_Void_WithInitialValues_CallsInternalRunner()
        {
            var initialValue = new object();
            const string pipelineName = "Pipeline";
            var pipeline = new Mock<IPipeline>();
            var pipelineRepository = new Mock<IPipelineRepository>();
            pipelineRepository.Setup(r => r.Get(pipelineName)).Returns(pipeline.Object);
            var pipelineContext = new Mock<IPipelineContext>();
            var internalPipelineRunner = new Mock<IInternalPipelineRunner>();
            internalPipelineRunner
                .Setup(r => r.Run(pipeline.Object, pipelineContext.Object))
                .Callback(() => pipelineContext.Verify(c => c.SetValue(initialValue)));

            var runner = new PipelineRunner(pipelineName,
                pipelineRepository.Object,
                internalPipelineRunner.Object,
                pipelineContext.Object);

            runner.Run(initialValue);

            internalPipelineRunner.Verify(r => r.Run(pipeline.Object, pipelineContext.Object));
        }

        [TestMethod]
        public void Run_ReturnValue_NoInitialValues_CallsInternalRunner()
        {
            const string pipelineName = "Pipeline";
            var runResult = new object();
            var pipeline = new Mock<IPipeline>();
            var pipelineRepository = new Mock<IPipelineRepository>();
            pipelineRepository.Setup(r => r.Get(pipelineName)).Returns(pipeline.Object);
            var pipelineContext = new Mock<IPipelineContext>();
            var internalPipelineRunner = new Mock<IInternalPipelineRunner>();
            internalPipelineRunner
                .Setup(r => r.Run(pipeline.Object, pipelineContext.Object))
                .Returns(runResult)
                .Callback(() => pipelineContext.Verify(c => c.SetValue(null)));

            var runner = new PipelineRunner(pipelineName,
                pipelineRepository.Object,
                internalPipelineRunner.Object,
                pipelineContext.Object);

            var result = runner.Run<object>();

            internalPipelineRunner.Verify(r => r.Run(pipeline.Object, pipelineContext.Object));
            result.Should().Be(runResult);
        }

        [TestMethod]
        public void Run_ReturnValue_WithInitialValues_CallsInternalRunner()
        {
            var initialValue = new object();
            var runResult = new object();
            const string pipelineName = "Pipeline";
            var pipeline = new Mock<IPipeline>();
            var pipelineRepository = new Mock<IPipelineRepository>();
            pipelineRepository.Setup(r => r.Get(pipelineName)).Returns(pipeline.Object);
            var pipelineContext = new Mock<IPipelineContext>();
            var internalPipelineRunner = new Mock<IInternalPipelineRunner>();
            internalPipelineRunner
                .Setup(r => r.Run(pipeline.Object, pipelineContext.Object))
                .Returns(runResult)
                .Callback(() => pipelineContext.Verify(c => c.SetValue(initialValue)));

            var runner = new PipelineRunner(pipelineName,
                pipelineRepository.Object,
                internalPipelineRunner.Object,
                pipelineContext.Object);

            var result = runner.Run<object>(initialValue);

            internalPipelineRunner.Verify(r => r.Run(pipeline.Object, pipelineContext.Object));
            result.Should().Be(runResult);
        }
    }
}
