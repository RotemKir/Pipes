using Pipes.Core.Runners;
using System;

namespace Pipes.Core
{
    public class PipelineBuilder : IPipelineBuilder
    {
        private readonly IPipelineRepository pipelineRepository = new PipelineRepository();
        private readonly ISegmentRunnerFactory segmentRunnerFactory = new SegmentRunnerFactory();
        private IPipeline pipeline;

        internal PipelineBuilder(string name, 
            IPipelineRepository pipelineRepository, 
            ISegmentRunnerFactory segmentRunnerFactory)
        {
            this.pipelineRepository = pipelineRepository;
            this.segmentRunnerFactory = segmentRunnerFactory;
            pipeline = pipelineRepository.CreateNew(name);
        }

        public PipelineBuilder(string name) : this(name, new PipelineRepository(), new SegmentRunnerFactory())
        {
        }
        
        public IPipelineBuilder Do<S>() where S : ISegment, new()
        {
            AddSegmentRunner(f => f.CreateSegmentRunner<S>());

            return this;
        }

        public IPipelineBuilder Do(ISegment segment)
        {
            AddSegmentRunner(f => f.CreateSegmentRunner(segment));

            return this;
        }

        public IPipelineBuilder DoByVersion(params ISegment[] segments)
        {
            AddSegmentRunner(f => f.CreateVersionedRunner(segments));

            return this;
        }

        public IPipelineBuilder DoPipeline(string pipelineName)
        {
            AddSegmentRunner(f => f.CreatePipelineRunner(pipelineName, pipelineRepository));

            return this;
        }

        private void AddSegmentRunner(Func<ISegmentRunnerFactory, ISegmentRunner> createSegmentRunner)
        {
            var segmentRunner = createSegmentRunner(segmentRunnerFactory);
            pipeline.AddSegmentRunner(segmentRunner);
        }
    }
}