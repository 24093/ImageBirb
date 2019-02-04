using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;

namespace ImageBirb.ViewModels
{
    internal class ViewModelLocator
    {
        public ViewModelLocator()
        {
            var imageBirb = new Core.ImageBirb();

            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            // Register backend adapter.
            SimpleIoc.Default.Register(() => imageBirb.WorkflowAdapter);

            // Register view models.
            SimpleIoc.Default.Register<DialogViewModel>();
            SimpleIoc.Default.Register<DragDropViewModel>();
            SimpleIoc.Default.Register<FlyoutViewModel>();
            SimpleIoc.Default.Register<ImageManagementViewModel>();
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<ProgressBarViewModel>();
            SimpleIoc.Default.Register<SelectedImageViewModel>();
            SimpleIoc.Default.Register<SettingsViewModel>();
            SimpleIoc.Default.Register<TagListViewModel>();
            SimpleIoc.Default.Register<ThumbnailListViewModel>();
        }

        public MainViewModel MainViewModel => ServiceLocator.Current.GetInstance<MainViewModel>();
    }
}
