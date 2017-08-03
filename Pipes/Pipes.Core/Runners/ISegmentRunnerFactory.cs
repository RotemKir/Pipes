namespace Pipes.Core.Runners
{
    internal interface ISegmentRunnerFactory
    {
        ISegmentRunner CreateSegmentRunner(ISegment segment);
        ISegmentRunner CreateSegmentRunner<S>() where S : ISegment, new();
        ISegmentRunner CreateVersionedRunner(params ISegment[] segments);
        ISegmentRunner CreatePipelineRunner(string pipelineName, IPipelineRepository pipelineRepository);
    }
}