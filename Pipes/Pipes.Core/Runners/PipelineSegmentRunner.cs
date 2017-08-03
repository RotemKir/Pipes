using System;

namespace Pipes.Core.Runners
{
    internal class PipelineSegmentRunner : ISegmentRunner
    {
        private readonly string pipelineName;
        private readonly IPipelineRepository pipelineRepository;
        private readonly IInternalPipelineRunner internalPipelineRunner;

        public PipelineSegmentRunner(string pipelineName, 
            IPipelineRepository pipelineRepository, 
            IInternalPipelineRunner internalPipelineRunner)
        {
            if (pipelineName == null) throw new ArgumentNullException("pipelineName");
            if (string.IsNullOrWhiteSpace(pipelineName)) throw new ArgumentException("Pipeline name cannot be empty", "pipelineName");
            if (pipelineRepository == null) throw new ArgumentNullException("pipelineRepository");
            if (internalPipelineRunner == null) throw new ArgumentNullException("internalPipelineRunner");

            this.pipelineName = pipelineName;
            this.pipelineRepository = pipelineRepository;
            this.internalPipelineRunner = internalPipelineRunner;
        }

        public object Run(IPipelineContext context)
        {
            var pipeline = pipelineRepository.Get(pipelineName);

            return internalPipelineRunner.Run(pipeline, context);
        }
    }
}