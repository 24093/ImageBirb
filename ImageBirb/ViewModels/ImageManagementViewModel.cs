using GalaSoft.MvvmLight.CommandWpf;
using ImageBirb.Core.Ports.Primary;
using Microsoft.Win32;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace ImageBirb.ViewModels
{
    internal class ImageManagementViewModel : WorkflowViewModel
    {
        private readonly SelectedImageViewModel _selectedImageViewModel;

        private readonly ThumbnailListViewModel _thumbnailListViewModel;

        private readonly DialogViewModel _dialogViewModel;

        public ProgressBarViewModel ProgressBarViewModel { get; }

        public ICommand AddImageFromFileCommand { get; }

        public ICommand RemoveImageCommand { get; }

        public ImageManagementViewModel(
            IWorkflowAdapter workflowAdapter, 
            SelectedImageViewModel selectedImageViewModel, 
            ThumbnailListViewModel thumbnailListViewModel,
            ProgressBarViewModel progressBarViewModel, 
            DialogViewModel dialogViewModel)
            : base(workflowAdapter)
        {
            _selectedImageViewModel = selectedImageViewModel;
            _thumbnailListViewModel = thumbnailListViewModel;
            ProgressBarViewModel = progressBarViewModel;
            _dialogViewModel = dialogViewModel;

            AddImageFromFileCommand = new RelayCommand(async () => await AddFilesFromOpenFileDialog());
            RemoveImageCommand = new RelayCommand<string>(async imageId => await RemoveSelectedImage(imageId), 
                imageId => _selectedImageViewModel.IsImageSelected);
        }

        private async Task RemoveSelectedImage(string imageId)
        {
            var dialogResult = await _dialogViewModel.ShowDialog("Delete image",
                "This will delete the currently selected image. Continue?",
                MessageDialogStyle.AffirmativeAndNegative);
            
            if (dialogResult == MessageDialogResult.Affirmative)
            {
                await RemoveImage(imageId);

                _selectedImageViewModel.ShowImageCommand.Execute(null);

                _thumbnailListViewModel.UpdateThumbnailsCommand.Execute(null);
            }
        }

        private async Task AddFilesFromOpenFileDialog()
        {
            var (dialogResult, fileNames) = _dialogViewModel.ShowOpenImageFilesDialog();

            if (dialogResult == true)
            {
                ProgressBarViewModel.Value = 0;
                ProgressBarViewModel.MaxValue = fileNames.Length;
                ProgressBarViewModel.Visibility = Visibility.Visible;

                foreach (var filename in fileNames)
                {
                    await AddImage(filename);
                    ProgressBarViewModel.Value++;
                }

                await Task.Delay(500);
                ProgressBarViewModel.Visibility = Visibility.Collapsed;
            }

            _thumbnailListViewModel.UpdateThumbnailsCommand.Execute(null);
        }
    }
}