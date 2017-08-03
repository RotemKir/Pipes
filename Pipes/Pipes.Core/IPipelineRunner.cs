namespace Pipes.Core
{
    public interface IPipelineRunner
    {
        IPipelineRunner Version(int version);
        void Run(object initialValues = null);
        T Run<T>(object initialValues = null);
    }
}
