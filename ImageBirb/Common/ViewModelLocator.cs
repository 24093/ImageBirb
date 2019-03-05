using Autofac;
using ImageBirb.Core.BusinessObjects;
using ImageBirb.ViewModels;

namespace ImageBirb.Common
{
    internal class ViewModelLocator
    {
        private readonly IContainer _container;

        public ViewModelLocator()
        {
            var databaseFilename = AppSettings.Get("DatabaseFilename", "ImageBirb.db");
            var coreSettings = new CoreSettings {DatabaseFilename = databaseFilename};
            var imageBirb = new Core.ImageBirb(coreSettings);

            var builder = new ContainerBuilder();

            // Register backend adapter.
            builder.RegisterInstance(imageBirb.WorkflowAdapter).SingleInstance();
            
            // Register view models.
            builder.RegisterType<DialogViewModel>().SingleInstance();
            builder.RegisterType<FlyoutViewModel>().SingleInstance();
            builder.RegisterType<ImageManagementViewModel>().SingleInstance();
            builder.RegisterType<MainViewModel>().SingleInstance();
            builder.RegisterType<SelectedImageViewModel>().SingleInstance();
            builder.RegisterType<SettingsViewModel>().SingleInstance();
            builder.RegisterType<TagListViewModel>().SingleInstance();
            builder.RegisterType<ThumbnailListViewModel>().SingleInstance();

            _container = builder.Build();
        }

        public MainViewModel MainViewModel => _container.Resolve<MainViewModel>();
    }
}
