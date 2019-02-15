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
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace ImageBirb.Core
{
    /// <summary>
    /// Construction root: Construct and wire the application elements.
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
            // Construct workflow host.
            IList<IWorkflow> workflows = new List<IWorkflow>();

            foreach (var workflowType in _workflowTypes)
            {
                workflows.Add((IWorkflow)_container.Resolve(workflowType));
            }
            
            var workflowHost = new WorkflowHost(workflows);

            // Resolve workflow adapter.
            WorkflowAdapter = _container.Resolve<IWorkflowAdapter>(new NamedParameter("workflows", workflowHost));
        }

        private void RegisterSecondaryAdapters(ContainerBuilder builder)
        {
            // Register default file system adapter.
            builder.RegisterType<DefaultFileSystemAdapter>()
                .As<IFileSystemAdapter>();

            // Register ImageMagick imaging adapter.
            builder.RegisterType<ImageMagickAdapter>()
                .As<IImagingAdapter>();

            // Register LiteDB database adapter as IDatabaseAdaper and as LiteDbAdapter.
            builder.RegisterType<LiteDbAdapter>()
                .SingleInstance()
                .As<IDatabaseAdapter>()
                .AsSelf()
                .WithParameter("databaseFilename", _coreSettings.DatabaseFilename);

            // Register LiteDB image management adapter.
            builder.RegisterType<LiteDbImageManagementAdapter>()
                .As<IImageManagementAdapter>();

            // Register LiteDB tag management adapter.
            builder.RegisterType<LiteDbTagManagementAdapter>()
                .As<ITagManagementAdapter>();
        }

        private void RegisterPrimaryAdapters(ContainerBuilder builder)
        {
            // Register the workflow facade.
            builder.RegisterType<WorkflowAdapter>()
                .As<IWorkflowAdapter>()
                .SingleInstance();
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
