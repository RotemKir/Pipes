using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using System;

namespace Pipes.Core.Tests
{
    [TestClass]
    public class PipelineRepositoryTests
    {
        [TestMethod]
        public void PipelineRepository_CreateNew_CreatesPipelineWithName()
        {
            const string pipelineName = "Pipeline1";
            var pipelineRepository = new PipelineRepository();

            var pipeline = pipelineRepository.CreateNew(pipelineName);

            pipeline.Should().NotBeNull();
            pipeline.Name.Should().Be(pipelineName);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PipelineRepository_Get_NameDoesntExist_Throws()
        {
            const string pipelineName = "Pipeline2";
            var pipelineRepository = new PipelineRepository();

            pipelineRepository.Get(pipelineName);
        }

        [TestMethod]
        public void PipelineRepository_Get_NameExists_ReturnsPipeline()
        {
            const string pipelineName = "Pipeline3";
            var pipelineRepository = new PipelineRepository();
            var newPipeline = pipelineRepository.CreateNew(pipelineName);

            var getPipeline = pipelineRepository.Get(pipelineName);

            getPipeline.Should().Be(newPipeline);
        }

        [TestMethod]
        public void PipelineRepository_Get_FromOtherRepository_ReturnsPipeline()
        {
            const string pipelineName = "Pipeline4";
            var pipelineRepository = new PipelineRepository();
            var otherPipelineRepository = new PipelineRepository();
            var newPipeline = pipelineRepository.CreateNew(pipelineName);

            var getPipeline = otherPipelineRepository.Get(pipelineName);

            getPipeline.Should().Be(newPipeline);
        }
    }
}
