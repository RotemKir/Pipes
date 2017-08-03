namespace Pipes.Core
{
    public interface IPipelineBuilder
    {
        IPipelineBuilder Do<S>() where S : ISegment, new();
        IPipelineBuilder Do(ISegment segment);
        IPipelineBuilder DoByVersion(params ISegment[] segments);
        IPipelineBuilder DoPipeline(string name);
    }
}