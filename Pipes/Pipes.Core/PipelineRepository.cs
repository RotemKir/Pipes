using System;
using System.Collections.Generic;

namespace Pipes.Core
{
    internal class PipelineRepository : IPipelineRepository
    {
        private static readonly IDictionary<string, Pipeline> pipelines = new Dictionary<string, Pipeline>();

        public IPipeline CreateNew(string name)
        {
            var pipeline = new Pipeline(name);
            pipelines.Add(pipeline.Name, pipeline);

            return pipeline;
        }

        public IPipeline Get(string name)
        {
            if (!pipelines.ContainsKey(name)) throw new InvalidOperationException($"Pipeline {name} doesn't exist in the repository");

            return pipelines[name];
        }
    }
}
