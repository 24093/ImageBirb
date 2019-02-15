using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using ImageBirb.Core.Common;
using ImageBirb.ViewModels;

namespace ImageBirb.Common
{
    internal class ViewModelLocator
    {
        public ViewModelLocator()
        {
            var databaseFilename = AppSettings.Get("DatabaseFilename", "ImageBirb.db");
            var coreSettings = new CoreSettings {DatabaseFilename = databaseFilename};
            var imageBirb = new Core.ImageBirb(coreSettings);

            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            // Register backend adapter.
            SimpleIoc.Default.Register(() => imageBirb.WorkflowAdapter);

            // Register view models.
            SimpleIoc.Default.Register<DialogViewModel>();
            SimpleIoc.Default.Register<FlyoutViewModel>();
            SimpleIoc.Default.Register<ImageManagementViewModel>();
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<SelectedImageViewModel>();
            SimpleIoc.Default.Register<SettingsViewModel>();
            SimpleIoc.Default.Register<TagListViewModel>();
            SimpleIoc.Default.Register<ThumbnailListViewModel>();
        }

        public MainViewModel MainViewModel => ServiceLocator.Current.GetInstance<MainViewModel>();
    }
}
