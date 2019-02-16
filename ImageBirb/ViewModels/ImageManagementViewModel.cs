using System.Collections.Generic;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.CommandWpf;
using ImageBirb.Common;
using ImageBirb.Core.Common;
using ImageBirb.Core.Ports.Primary;
using MahApps.Metro.Controls.Dialogs;
using System.Windows.Input;
using ImageBirb.Controls;
using Microsoft.WindowsAPICodePack.Dialogs;

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
            var dialogResult = await _dialogViewModel.ShowDialog("Delete image",
                "This will delete the currently selected image. Continue?",
                MessageDialogStyle.AffirmativeAndNegative);

            if (dialogResult == MessageDialogResult.Affirmative)
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
                CommonFileDialogResult dialogResult;
                string directory = null;
                IList<string> files = null;

                if (addingFolder)
                {
                    var result = _dialogViewModel.ShowSelectDirectoryDialog();
                    dialogResult = result.DialogResult;
                    directory = result.DirectoryName;
                }
                else
                {
                    var result = _dialogViewModel.ShowOpenImageFilesDialog();
                    dialogResult = result.DialogResult;
                    files = result.FileNames;
                }

                if (dialogResult == CommonFileDialogResult.Ok)
                {
                    _progressDialogController =
                        await _dialogViewModel.ShowProgressDialog("Adding images...", string.Empty);

                    if (addingFolder)
                    {
                        await RunAsync(Workflows.AddImages(directory));
                    }
                    else
                    {
                        await RunAsync(Workflows.AddImages(files));
                    }
                }
            });
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