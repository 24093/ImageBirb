using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;

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

        public (bool? DialogResult, string[] FileNames) ShowOpenImageFilesDialog()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Image Files(*.bmp; *.jpg; *.jpeg; *.png)| *.bmp; *.jpg; *.jpeg; *.png | All files(*.*) | *.*",
                Multiselect = true
            };

           var dialogResult = dialog.ShowDialog(_mainWindow);
           return (dialogResult, dialog.FileNames);
        }
    }
}