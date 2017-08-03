namespace Pipes.Core
{
    internal interface IInternalPipelineRunner
    {
        object Run(IPipeline pipeline, IPipelineContext context);
    }
}