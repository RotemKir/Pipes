namespace Pipes.Core
{
    internal interface ISegmentMethodInvoker
    {
        R InvokeMethod<R>(ISegment segment, string methodName, IPipelineContext context, R defaultValue);
    }
}