using Autofac;
using ImageBirb.Core.Adapters.Primary;
using ImageBirb.Core.Adapters.Secondary;
using ImageBirb.Core.Common;
using ImageBirb.Core.Ports.Primary;
using ImageBirb.Core.Ports.Secondary;
using ImageBirb.Core.Workflows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ImageBirbCoreUnitTests")]
namespace ImageBirb.Core
{
    /// <summary>
    /// Construct and wire the application elements.
    /// </summary>
    public class ImageBirb
    {
        private readonly IContainer _container;

        private readonly CoreSettings _coreSettings;

        private readonly List<Type> _workflowTypes;

        public IWorkflowAdapter WorkflowAdapter { get; private set; }

        public ImageBirb(CoreSettings coreSettings)
        {
            _coreSettings = coreSettings;

            _workflowTypes = new List<Type>();

            var builder = new ContainerBuilder();

            RegisterWorkflows(builder);
            RegisterPrimaryAdapters(builder);
            RegisterSecondaryAdapters(builder);

            _container = builder.Build();

            ExposePrimaryAdapters();
        }

        private void ExposePrimaryAdapters()
        {
            // Construct workflow collection.
            IList<IWorkflow> workflows = new List<IWorkflow>();

            foreach (var workflowType in _workflowTypes)
            {
                workflows.Add((IWorkflow)_container.Resolve(workflowType));
            }
            
            var workflowCollection = new WorkflowHost(workflows);

            // Resolve workflow adapter.
            WorkflowAdapter = _container.Resolve<IWorkflowAdapter>(new NamedParameter("workflows", workflowCollection));
        }

        private void RegisterSecondaryAdapters(ContainerBuilder builder)
        {
            builder.RegisterType<DefaultFileSystemAdapter>().As<IFileSystemAdapter>();
            builder.RegisterType<ImageSharpAdapter>().As<IImagingAdapter>();
            builder.RegisterType<LiteDbAdapter>().As<IDatabaseAdapter>().SingleInstance()
                .WithParameter("databaseFilename", _coreSettings.DatabaseFilename);
        }

        private void RegisterPrimaryAdapters(ContainerBuilder builder)
        {
            builder.RegisterType<WorkflowAdapter>().As<IWorkflowAdapter>().SingleInstance();
        }

        private void RegisterWorkflows(ContainerBuilder builder)
        {
            // Register all available workflows.
            var workflowTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => typeof(IWorkflow).IsAssignableFrom(t) && !t.IsAbstract);

            foreach (var workflowType in workflowTypes)
            {
                builder.RegisterType(workflowType);
                _workflowTypes.Add(workflowType);
            }
        }
    }
}
