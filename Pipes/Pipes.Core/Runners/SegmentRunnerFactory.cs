namespace Pipes.Core.Runners
{
    internal class SegmentRunnerFactory : ISegmentRunnerFactory
    {
        public ISegmentRunner CreateSegmentRunner<S>() where S : ISegment, new()
        {
            var segmentRunner = new S();

            return CreateSegmentRunner(segmentRunner);
        }

        public ISegmentRunner CreateSegmentRunner(ISegment segment)
        {
            var segmentMethodInvoker = new SegmentMethodInvoker();

            return new InstanceSegmentRunner(segment, segmentMethodInvoker);
        }

        public ISegmentRunner CreateVersionedRunner(params ISegment[] segments)
        {
            return new VersionedSegmentRunner(segments);
        }

        public ISegmentRunner CreatePipelineRunner(string pipelineName, IPipelineRepository pipelineRepository)
        {
            var internalPipelineRunner = new InternalPipelineRunner();

            return new PipelineSegmentRunner(pipelineName, pipelineRepository, internalPipelineRunner);
        }
    }
}
