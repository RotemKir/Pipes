using Pipes.Core.Runners;
using System;
using System.Collections.Generic;

namespace Pipes.Core
{
    internal class Pipeline : IPipeline
    {
        private readonly List<ISegmentRunner> segmentRunners = new List<ISegmentRunner>();

        public string Name { get; private set; }
        public IEnumerable<ISegmentRunner> SegmentRunners => segmentRunners;

        public Pipeline(string name)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot be empty", "name");

            Name = name;
        }

        public void AddSegmentRunner(ISegmentRunner segmentRunner)
        {
            if (segmentRunner == null) throw new ArgumentNullException("segmentRunner");

            segmentRunners.Add(segmentRunner);
        }
    }
}