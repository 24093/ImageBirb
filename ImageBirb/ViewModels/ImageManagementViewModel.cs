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

        public ProgressBarViewModel ProgressBarViewModel { get; }

        public ICommand AddImageFromFileCommand { get; }

        public ICommand RemoveImageCommand { get; }

        public ImageManagementViewModel(
            IWorkflowAdapter workflowAdapter, 
            SelectedImageViewModel selectedImageViewModel, 
            ThumbnailListViewModel thumbnailListViewModel,
            ProgressBarViewModel progressBarViewModel)
            : base(workflowAdapter)
        {
            _selectedImageViewModel = selectedImageViewModel;
            _thumbnailListViewModel = thumbnailListViewModel;
            ProgressBarViewModel = progressBarViewModel;

            AddImageFromFileCommand = new RelayCommand(async () => await AddFilesFromOpenFileDialog());
            RemoveImageCommand = new RelayCommand<string>(async imageId => await RemoveSelectedImage(imageId), 
                imageId => _selectedImageViewModel.IsImageSelected);
        }

        private async Task RemoveSelectedImage(string imageId)
        {
            var mainWindow = Application.Current.MainWindow as MetroWindow;

            var dialogResult = await mainWindow.ShowMessageAsync("Delete image",
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
            var dialog = new OpenFileDialog
            {
                Filter = "Image Files(*.bmp; *.jpg; *.jpeg; *.png)| *.bmp; *.jpg; *.jpeg; *.png | All files(*.*) | *.*",
                Multiselect = true
            };

            var dialogResult = dialog.ShowDialog(Application.Current.MainWindow);

            if (dialogResult == true)
            {
                ProgressBarViewModel.Value = 0;
                ProgressBarViewModel.MaxValue = dialog.FileNames.Length;
                ProgressBarViewModel.Visibility = Visibility.Visible;

                foreach (var filename in dialog.FileNames)
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