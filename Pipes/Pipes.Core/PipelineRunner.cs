namespace Pipes.Core
{
    public class PipelineRunner : IPipelineRunner
    {
        private readonly IInternalPipelineRunner internalPipelineRunner;
        private readonly IPipelineContext context;
        private readonly IPipeline pipeline;


        internal PipelineRunner(string name,
            IPipelineRepository pipelineRepository,
            IInternalPipelineRunner internalPipelineRunner,
            IPipelineContext context)
        {
            this.internalPipelineRunner = internalPipelineRunner;
            this.context = context;

            pipeline = pipelineRepository.Get(name);
        }

        public PipelineRunner(string name) : this(name, 
            new PipelineRepository(), 
            new InternalPipelineRunner(), 
            new PipelineContext())
        {
        }

        public T Run<T>(object initialValues = null)
        {
            return (T)InternalRun(initialValues);
        }

        public void Run(object initialValues = null)
        {
            InternalRun(initialValues);
        }

        private object InternalRun(object initialValues = null)
        {
            context.SetValue(initialValues);

            return internalPipelineRunner.Run(pipeline, context);
        }

        public IPipelineRunner Version(int version)
        {
            context.MaxVersion = version;

            return this;
        }
    }
}