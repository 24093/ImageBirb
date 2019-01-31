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
            SimpleIoc.Default.Register(() => imageBirb.WorkflowAdapter);
            SimpleIoc.Default.Register<MainViewModel>();
        }

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();
    }
}