using System;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace ImageBirb.ViewModels
{
    /// <summary>
    /// Handles the state of the flyouts of the main window.
    /// </summary>
    internal class FlyoutViewModel : ViewModelBase
    {
        private bool _isTagListFlyoutOpen;

        private bool _isSelectedImageTagsFlyoutOpen;

        private readonly SelectedImageViewModel _selectedImageViewModel;
        
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

        public ICommand ToggleTagListCommand { get; }

        public ICommand ToggleSelectedImageTagsCommand { get; }

        public FlyoutViewModel(SelectedImageViewModel selectedImageViewModel)
        {
            _selectedImageViewModel = selectedImageViewModel;

            IsTagListFlyoutOpen = false;
            IsSelectedImageTagsFlyoutOpen = false;

            ToggleTagListCommand = new RelayCommand(ExecuteToggleTagListCommand);
            ToggleSelectedImageTagsCommand = new RelayCommand(ExecuteToggleSelectedImageTagsCommand, CanExecuteShowSelectedImageTagsCommand);
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