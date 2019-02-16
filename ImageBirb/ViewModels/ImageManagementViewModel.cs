using GalaSoft.MvvmLight.CommandWpf;
using ImageBirb.Common;
using ImageBirb.Controls;
using ImageBirb.Core.Common;
using ImageBirb.Core.Ports.Primary;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Threading.Tasks;
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

        private ProgressDialogController _progressDialogController;

        public ICommand AddImageFromFileCommand { get; }

        public ICommand RemoveImageCommand { get; }

        public ImageManagementViewModel(
            IWorkflowAdapter workflows,
            SelectedImageViewModel selectedImageViewModel,
            ThumbnailListViewModel thumbnailListViewModel,
            DialogViewModel dialogViewModel)
            : base(workflows)
        {
            _selectedImageViewModel = selectedImageViewModel;
            _thumbnailListViewModel = thumbnailListViewModel;
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
            var isOk = await _dialogViewModel.ShowOkCancelDialog("Delete image",
                "This will delete the currently selected image. Continue?");

            if (isOk)
            {
                await Workflows.RemoveImage(imageId);

                _selectedImageViewModel.ShowImageCommand.Exec();

                _thumbnailListViewModel.UpdateThumbnailsCommand.Exec();
            }
        }

        private async void ExecuteAddImageFromFileCommand()
        {
            await RunAsyncDispatch(Workflows.ReadSetting(SettingType.AddFolders.ToString()), async r =>
            {
                var addingFolder = r.Setting.AsBool();
                
                if (addingFolder)
                {
                    await AddImagesFromFolder();
                }
                else
                {
                    await AddImagesFromFiles();
                }
            });
        }

        private async Task AddImagesFromFolder()
        {
            var result = _dialogViewModel.ShowSelectDirectoryDialog();
       
            if (result.IsOk)
            {
                _progressDialogController = await _dialogViewModel.ShowProgressDialog("Adding images...", string.Empty);
                await RunAsync(Workflows.AddImages(result.DirectoryName));
            }
        }

        private async Task AddImagesFromFiles()
        {
            var result = _dialogViewModel.ShowOpenImageFilesDialog();
           
            if (result.IsOk)
            {
                _progressDialogController = await _dialogViewModel.ShowProgressDialog("Adding images...", string.Empty);
                await RunAsync(Workflows.AddImages(result.FileNames));
            }
        }

        protected override async void WorkflowsOnProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.Type == ProgressType.ImageAdded)
            {
                await HandleImageAddedProgressChanged(e);
            }
        }

        private async Task HandleImageAddedProgressChanged(ProgressChangedEventArgs e)
        {
            var progress = (double) e.Progress / e.Max;

            _progressDialogController.SetProgress(progress);
            _progressDialogController.SetMessage(e.Progress + " of " + e.Max);

            if (e.Progress == e.Max)
            {
                await _progressDialogController.CloseAsync();

                _thumbnailListViewModel.UpdateThumbnailsCommand.Exec();
            }
        }
    }
}