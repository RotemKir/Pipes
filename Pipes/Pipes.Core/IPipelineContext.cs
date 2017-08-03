namespace Pipes.Core
{
    internal interface IPipelineContext
    {
        int MaxVersion { get; set; }

        void SetValue(object value);
        object GetValue(string name);
    }
}