namespace Pipes.Core
{
    internal interface IPipelineRepository
    {
        IPipeline CreateNew(string name);
        IPipeline Get(string name);
    }
}