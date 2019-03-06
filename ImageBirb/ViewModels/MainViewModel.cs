using System.ComponentModel;
using System.Windows.Threading;
using ImageBirb.Core.Ports.Primary;

namespace ImageBirb.ViewModels
{
    /// <summary>
    /// Root view model. Creates child view models.
    /// </summary>
    internal class MainViewModel : WorkflowViewModel
    {
        public TagListViewModel TagListViewModel { get; }

        public ThumbnailListViewModel ThumbnailListViewModel { get; }
        
        public SelectedImageViewModel SelectedImageViewModel { get; }

        public FlyoutViewModel FlyoutViewModel { get; }
        
        public ImageManagementViewModel ImageManagementViewModel { get; }

        public SettingsViewModel SettingsViewModel { get; }

        public MainViewModel(
            IWorkflowAdapter workflows,
            TagListViewModel tagListViewModel,
            ThumbnailListViewModel thumbnailListViewModel,
            SelectedImageViewModel selectedImageViewModel,
            FlyoutViewModel flyoutViewModel,
            ImageManagementViewModel imageManagementViewModel,
            SettingsViewModel settingsViewModel)
            : base(workflows)
        {
            TagListViewModel = tagListViewModel;
            ThumbnailListViewModel = thumbnailListViewModel;
            SelectedImageViewModel = selectedImageViewModel;
            FlyoutViewModel = flyoutViewModel;
            ImageManagementViewModel = imageManagementViewModel;
            SettingsViewModel = settingsViewModel;

            SelectedImageViewModel.PropertyChanged += SelectedImageViewModelOnPropertyChanged;
        }

        private void SelectedImageViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedImageViewModel.IsImageSelected))
            {
                Dispatcher.CurrentDispatcher.Invoke(() =>
                    FlyoutViewModel.ToggleSelectedImageTagsCommandRaiseCanExecuteChanged());
            }
        }
    }
}

