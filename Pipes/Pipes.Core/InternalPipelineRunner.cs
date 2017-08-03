using System;

namespace Pipes.Core
{
    internal class InternalPipelineRunner : IInternalPipelineRunner
    {
        public object Run(IPipeline pipeline, IPipelineContext context)
        {
            if (pipeline == null) throw new ArgumentNullException("pipeline");
            if (context == null) throw new ArgumentNullException("context");

            object result = null;
            var segmentRunners = pipeline.SegmentRunners;

            foreach (var runner in segmentRunners)
            {
                var tempResult = runner.Run(context);
                context.SetValue(tempResult);

                if (tempResult != null) result = tempResult;
            }

            return result;
        }
    }
}