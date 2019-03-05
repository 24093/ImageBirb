using System.Threading.Tasks;
using System.Windows.Input;

namespace ImageBirb.ViewModels
{
    /// <summary>
    /// Handles the state of the flyouts of the main window.
    /// Contains no backend logic.
    /// </summary>
    internal class FlyoutViewModel : ViewModelBase
    {
        private bool _isTagListFlyoutOpen;

        private bool _isSelectedImageTagsFlyoutOpen;

        private bool _isSettingsFlyoutOpen;

        private readonly SelectedImageViewModel _selectedImageViewModel;

        private readonly SettingsViewModel _settingsViewModel;
        
        public bool IsTagListFlyoutOpen
        {
            get => _isTagListFlyoutOpen;
            set => Set(ref _isTagListFlyoutOpen, value);
        }

        public bool IsSelectedImageTagsFlyoutOpen
        {
            get => _isSelectedImageTagsFlyoutOpen;
            set => Set(ref _isSelectedImageTagsFlyoutOpen, value);
        }

        public bool IsSettingsFlyoutOpen
        {
            get => _isSettingsFlyoutOpen;
            set => Set(ref _isSettingsFlyoutOpen, value);
        }

        public ICommand ToggleTagListCommand { get; }

        public ICommand ToggleSelectedImageTagsCommand { get; }

        public ICommand ToggleSettingsCommand { get; }

        public FlyoutViewModel(SelectedImageViewModel selectedImageViewModel, SettingsViewModel settingsViewModel)
        {
            _selectedImageViewModel = selectedImageViewModel;
            _settingsViewModel = settingsViewModel;

            IsTagListFlyoutOpen = false;
            IsSelectedImageTagsFlyoutOpen = false;
            IsSettingsFlyoutOpen = false;

            ToggleTagListCommand = new RelayCommand(ExecuteToggleTagListCommand);
            ToggleSelectedImageTagsCommand = new RelayCommand(ExecuteToggleSelectedImageTagsCommand, CanExecuteShowSelectedImageTagsCommand);
            ToggleSettingsCommand = new RelayCommand(async () => await ExecuteToggleSettingsCommand());
        }

        private async Task ExecuteToggleSettingsCommand()
        {
            if (!IsSettingsFlyoutOpen)
            {
                await _settingsViewModel.ReadSettings();
            }

            IsSettingsFlyoutOpen = !IsSettingsFlyoutOpen;
        }

        private void ExecuteToggleSelectedImageTagsCommand()
        {
            IsSelectedImageTagsFlyoutOpen = !IsSelectedImageTagsFlyoutOpen;
        }

        private void ExecuteToggleTagListCommand()
        {
            IsTagListFlyoutOpen = !IsTagListFlyoutOpen;
        }

        private bool CanExecuteShowSelectedImageTagsCommand()
        {
            return _selectedImageViewModel.IsImageSelected;
        }

    }
}