using GalaSoft.MvvmLight.CommandWpf;
using ImageBirb.Core.Ports.Primary;
using MahApps.Metro.Controls.Dialogs;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ImageBirb.ViewModels
{
    /// <summary>
    /// Handles image management tasks like adding and deleting images.
    /// </summary>
    internal class ImageManagementViewModel : WorkflowViewModel
    {
        private readonly SelectedImageViewModel _selectedImageViewModel;

        private readonly ThumbnailListViewModel _thumbnailListViewModel;

        private readonly DialogViewModel _dialogViewModel;

        public ProgressBarViewModel ProgressBarViewModel { get; }

        public ICommand AddImageFromFileCommand { get; }

        public ICommand RemoveImageCommand { get; }

        public ImageManagementViewModel(
            IWorkflowAdapter workflows, 
            SelectedImageViewModel selectedImageViewModel, 
            ThumbnailListViewModel thumbnailListViewModel,
            ProgressBarViewModel progressBarViewModel, 
            DialogViewModel dialogViewModel)
            : base(workflows)
        {
            _selectedImageViewModel = selectedImageViewModel;
            _thumbnailListViewModel = thumbnailListViewModel;
            ProgressBarViewModel = progressBarViewModel;
            _dialogViewModel = dialogViewModel;

            AddImageFromFileCommand = new RelayCommand(ExecuteAddImageFromFileCommand);
            RemoveImageCommand = new RelayCommand<string>(ExecuteRemoveImageCommand, CanExecuteRemoveImageCommand);
        }

        private bool CanExecuteRemoveImageCommand(string imageId)
        {
            return _selectedImageViewModel.IsImageSelected;
        }

        private async void ExecuteRemoveImageCommand(string imageId)
        {
            var dialogResult = await _dialogViewModel.ShowDialog("Delete image",
                "This will delete the currently selected image. Continue?",
                MessageDialogStyle.AffirmativeAndNegative);

            if (dialogResult == MessageDialogResult.Affirmative)
            {
                await Workflows.RemoveImage(imageId);

                _selectedImageViewModel.ShowImageCommand.Exec(null);

                _thumbnailListViewModel.UpdateThumbnailsCommand.Exec(null);
            }
        }

        private async void ExecuteAddImageFromFileCommand()
        {
            var (dialogResult, fileNames) = _dialogViewModel.ShowOpenImageFilesDialog();

            if (dialogResult == true)
            {
                ProgressBarViewModel.Value = 0;
                ProgressBarViewModel.MaxValue = fileNames.Length;
                ProgressBarViewModel.Visibility = Visibility.Visible;

                foreach (var filename in fileNames)
                {
                    await RunAsync(Workflows.AddImage(filename), r => ProgressBarViewModel.Value++);
                }

                await Task.Delay(500);
                ProgressBarViewModel.Visibility = Visibility.Collapsed;
            }

            _thumbnailListViewModel.UpdateThumbnailsCommand.Exec(null);
        }
    }
}