using System;
using System.Linq;

namespace Pipes.Core.Runners
{
    internal class VersionedSegmentRunner : ISegmentRunner
    {
        private readonly ISegment[] segments;
        private readonly ISegmentRunnerFactory segmentRunnerFactory;

        public VersionedSegmentRunner(ISegment[] segments, ISegmentRunnerFactory segmentRunnerFactory = null)
        {
            if (segments == null) throw new ArgumentNullException("segments");
            if (segments.Length == 0) throw new ArgumentException("Segments cannot be empty","segments");
            if (segments.Length != segments.Select(s => s.Version).Distinct().Count()) throw new ArgumentException("Segments must have different versions", "segments");

            this.segments = segments;
            this.segmentRunnerFactory = segmentRunnerFactory ?? new SegmentRunnerFactory();
        }

        public object Run(IPipelineContext context)
        {
            var segment = segments
                .Where(s => s.Version <= context.MaxVersion)
                .OrderByDescending(s => s.Version)
                .FirstOrDefault();

            if (segment == null) return null;

            var segmentRunner = segmentRunnerFactory.CreateSegmentRunner(segment);

            return segmentRunner.Run(context);
        }
    }
}