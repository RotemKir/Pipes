using Pipes.Core.Runners;
using System.Collections.Generic;

namespace Pipes.Core
{
    internal interface IPipeline
    {
        string Name { get; }
        IEnumerable<ISegmentRunner> SegmentRunners { get; }

        void AddSegmentRunner(ISegmentRunner segmentRunner);
    }
}