using System;

namespace Pipes.Core.Runners
{
    internal class InstanceSegmentRunner : ISegmentRunner
    {
        private readonly ISegment segment;
        private readonly ISegmentMethodInvoker methodInvoker;

        public InstanceSegmentRunner(ISegment segment, ISegmentMethodInvoker methodInvoker)
        {
            if (segment == null) throw new ArgumentNullException("segment");
            if (methodInvoker == null) throw new ArgumentNullException("methodInvoker");

            this.segment = segment;
            this.methodInvoker = methodInvoker;
        }

        public object Run(IPipelineContext context)
        {
            return CanRunSegment(segment, context) ? RunSegment(segment, context) : null;
        }

        private bool CanRunSegment(ISegment segment, IPipelineContext context)
        {
            if (segment.Version > context.MaxVersion) return false;

            var canRun = methodInvoker.InvokeMethod(segment, "CanRun", context, defaultValue: true);

            return canRun;
        }

        private object RunSegment(ISegment segment, IPipelineContext context)
        {
            var result = methodInvoker.InvokeMethod<object>(segment, "Run", context, defaultValue: null);

            return result;
        }
    }
}