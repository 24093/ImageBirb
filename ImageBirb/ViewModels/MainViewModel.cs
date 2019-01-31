using GongSolutions.Wpf.DragDrop;
using ImageBirb.Core.Ports.Primary;
using System.Threading.Tasks;
using System.Windows;

namespace ImageBirb.ViewModels
{
    internal class MainViewModel : WorkflowViewModel, IDropTarget
    {
        
        public TagListViewModel TagListViewModel { get; }

        public ThumbnailListViewModel ThumbnailListViewModel { get; }
        
        public SelectedImageViewModel SelectedImageViewModel { get; }
        
        public MainViewModel(IWorkflowAdapter workflowAdapter)
            : base(workflowAdapter)
        {
            TagListViewModel = new TagListViewModel(workflowAdapter);
            ThumbnailListViewModel = new ThumbnailListViewModel(workflowAdapter);
            SelectedImageViewModel = new SelectedImageViewModel(workflowAdapter);

        }
        
        public async void DragOver(IDropInfo dropInfo)
        {
            await DragOverAsync(dropInfo);
        }

        public async Task DragOverAsync(IDropInfo dropInfo)
        {
            var filename = ((DataObject) dropInfo.Data).GetFileDropList()[0];

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

        public async void Drop(IDropInfo dropInfo)
        {
            await DropAsync(dropInfo);
        }

        public async Task DropAsync(IDropInfo dropInfo)
        {
            var fileList = ((DataObject) dropInfo.Data).GetFileDropList();

            foreach (var filename in fileList)
            {
                await AddImage(filename);
            }

            ThumbnailListViewModel.UpdateThumbnailsCommand.Execute(null);
        }
    }
}
