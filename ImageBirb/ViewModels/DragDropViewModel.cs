using GongSolutions.Wpf.DragDrop;
using ImageBirb.Core.Ports.Primary;
using System.Threading.Tasks;
using System.Windows;

namespace ImageBirb.ViewModels
{
    /// <summary>
    /// Handles drag & drop for the main window.
    /// </summary>
    internal class DragDropViewModel : WorkflowViewModel
    {
        private readonly ThumbnailListViewModel _thumbnailListViewModel;

        private readonly ProgressBarViewModel _progressBarViewModel;
        
        public DragDropViewModel(IWorkflowAdapter workflows, ThumbnailListViewModel thumbnailListViewModel, ProgressBarViewModel progressBarViewModel)
            :base(workflows)
        {
            _thumbnailListViewModel = thumbnailListViewModel;
            _progressBarViewModel = progressBarViewModel;
        }

        public void DragOver(IDropInfo dropInfo)
        {
            var filename = ((DataObject)dropInfo.Data).GetFileDropList()[0];

            if (!string.IsNullOrEmpty(filename))
            {
                var result = Task.Run(async () => await Workflows.VerifyImageFile(filename)).Result;

                if (result.IsSuccess && result.IsBitmapImage)
                {
                    dropInfo.Effects = DragDropEffects.Copy;
                }
                else
                {
                    dropInfo.Effects = DragDropEffects.None;
                }
            }
        }

        public void Drop(IDropInfo dropInfo)
        {
            var fileList = ((DataObject)dropInfo.Data).GetFileDropList();

            // TODO: Fix display of progress bar
            _progressBarViewModel.Visibility = Visibility.Visible;
            _progressBarViewModel.Value = 0;
            _progressBarViewModel.MaxValue = fileList.Count;
        
            foreach (var filename in fileList)
            {
                var task = Task.Run(async () => await Workflows.AddImage(filename));
                task.Wait();

                _progressBarViewModel.Value++;
            }
            
            _progressBarViewModel.Visibility = Visibility.Collapsed;

            _thumbnailListViewModel.UpdateThumbnailsCommand.Exec(null);
        }
    }
}