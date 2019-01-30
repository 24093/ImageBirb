using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;

namespace ImageBirb.ViewModels
{
    internal class ViewModelLocator
    {
        private readonly Core.ImageBirb _imageBirb;
            
        public ViewModelLocator()
        {
            _imageBirb = new Core.ImageBirb();

            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register(() => _imageBirb.WorkflowAdapter);
            SimpleIoc.Default.Register<MainViewModel>();
        }

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();
    }
}