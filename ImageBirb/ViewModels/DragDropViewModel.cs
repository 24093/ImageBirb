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
        
        public DragDropViewModel(IWorkflowAdapter workflowAdapter, ThumbnailListViewModel thumbnailListViewModel)
            :base(workflowAdapter)
        {
            _thumbnailListViewModel = thumbnailListViewModel;
        }

        public async Task DragOverAsync(IDropInfo dropInfo)
        {
            var filename = ((DataObject)dropInfo.Data).GetFileDropList()[0];

            if (!string.IsNullOrEmpty(filename))
            {
                var result = await VerifyImageFile(filename);

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

        public async Task DropAsync(IDropInfo dropInfo)
        {
            var fileList = ((DataObject)dropInfo.Data).GetFileDropList();

            foreach (var filename in fileList)
            {
                await AddImage(filename);
            }

            if (_thumbnailListViewModel.UpdateThumbnailsCommand.CanExecute(null))
            {
                _thumbnailListViewModel.UpdateThumbnailsCommand.Execute(null);
            }        
        }
    }
}