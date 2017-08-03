namespace Pipes.Core.Runners
{
    internal interface ISegmentRunner
    {
        object Run(IPipelineContext context);
    }   
}