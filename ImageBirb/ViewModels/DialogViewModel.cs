using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace ImageBirb.ViewModels
{
    /// <summary>
    /// Handles all sorts of dialogs presented to the user.
    /// </summary>
    internal class DialogViewModel
    {
        private MetroWindow _mainWindow => (MetroWindow) Application.Current.MainWindow;

        public async Task<MessageDialogResult> ShowDialog(string title, string message,
            MessageDialogStyle style = MessageDialogStyle.Affirmative, MetroDialogSettings settings = null)
        {
            return await _mainWindow.ShowMessageAsync(title, message, style, settings);
        }

        public async Task<ProgressDialogController> ShowProgressDialog(string title, string message, bool isCancelable = false,
            MetroDialogSettings settings = null)
        {
            return await _mainWindow.ShowProgressAsync(title, message, isCancelable, settings);
        }

        public (CommonFileDialogResult DialogResult, IList<string> FileNames) ShowOpenFileDialog(bool multiselect, IList<CommonFileDialogFilter> filters)
        {
            var dialog = new CommonOpenFileDialog { Multiselect = multiselect };

            foreach (var filter in filters)
            {
                dialog.Filters.Add(filter);
            }

            var dialogResult = dialog.ShowDialog(_mainWindow);
            var filenames = dialogResult == CommonFileDialogResult.Ok ? new List<string>(dialog.FileNames) : null;
            return (dialogResult, filenames);
        }

        public (CommonFileDialogResult DialogResult, IList<string> FileNames) ShowOpenImageFilesDialog()
        {
            return ShowOpenFileDialog(true, new List<CommonFileDialogFilter>
            {
                new CommonFileDialogFilter("Image files", "*.bmp; *.jpg; *.jpeg; *.png"),
                new CommonFileDialogFilter("All files", "*.*")
            });
        }

        public (CommonFileDialogResult DialogResult, string DirectoryName) ShowSelectDirectoryDialog()
        {
            var dialog = new CommonOpenFileDialog {IsFolderPicker = true};

            var dialogResult = dialog.ShowDialog();
            var directoryName = dialogResult == CommonFileDialogResult.Ok ? dialog.FileName : null;
            return (dialogResult, directoryName);
        }
    }
}