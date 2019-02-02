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

        private readonly Func<bool> _isImageSelected;
        
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

        public ICommand ShowTagListCommand { get; }

        public ICommand ShowSelectedImageTagsCommand { get; }

        public FlyoutViewModel(Func<bool> isImageSelected)
        {
            _isImageSelected = isImageSelected;

            IsTagListFlyoutOpen = false;
            IsSelectedImageTagsFlyoutOpen = false;

            ShowTagListCommand = new RelayCommand(ExecuteShotTagListCommand);
            ShowSelectedImageTagsCommand = new RelayCommand(ExecuteShowSelectedImageTagsCommand, CanExecuteShowSelectedImageTagsCommand);
        }

        private void ExecuteShowSelectedImageTagsCommand()
        {
            IsSelectedImageTagsFlyoutOpen = !IsSelectedImageTagsFlyoutOpen;
        }

        private void ExecuteShotTagListCommand()
        {
            IsTagListFlyoutOpen = !IsTagListFlyoutOpen;
        }

        private bool CanExecuteShowSelectedImageTagsCommand()
        {
            return _isImageSelected();
        }

    }
}