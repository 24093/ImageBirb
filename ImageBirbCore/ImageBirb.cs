using System.Linq;
using System.Reflection;
using Autofac;
using ImageBirb.Core.Adapters.Primary;
using ImageBirb.Core.Adapters.Secondary;
using ImageBirb.Core.Ports.Primary;
using ImageBirb.Core.Ports.Secondary;
using ImageBirb.Core.Workflows;

namespace ImageBirb.Core
{
    public class ImageBirb
    {
        private readonly IContainer _container;

        public IWorkflowAdapter WorkflowAdapter { get; private set; }

        public ImageBirb()
        {
            var builder = new ContainerBuilder();

            RegisterWorkflows(builder);
            RegisterPrimaryAdapters(builder);
            RegisterSecondaryAdapters(builder);

            _container = builder.Build();

            ExposePrimaryAdapters();
        }

        private void ExposePrimaryAdapters()
        {
            WorkflowAdapter = _container.Resolve<IWorkflowAdapter>();
        }

        private static void RegisterSecondaryAdapters(ContainerBuilder builder)
        {
            builder.RegisterType<DefaultFileSystemAdapter>().As<IFileSystemAdapter>().SingleInstance();
            builder.RegisterType<ImageSharpAdapter>().As<IImagingAdapter>();
            builder.RegisterType<LiteDbAdapter>().As<IDatabaseAdapter>().SingleInstance()
                .WithParameter("databaseFilename", "ImageBirbDemo.db");
        }

        private static void RegisterPrimaryAdapters(ContainerBuilder builder)
        {
            builder.RegisterType<WorkflowAdapter>().As<IWorkflowAdapter>().SingleInstance();
        }

        private static void RegisterWorkflows(ContainerBuilder builder)
        {
            var workflowTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => typeof(IWorkflow).IsAssignableFrom(t) && !t.IsAbstract);

            foreach (var workflowType in workflowTypes)
            {
                builder.RegisterType(workflowType);
            }
        }
    }
}
