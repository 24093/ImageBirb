using GongSolutions.Wpf.DragDrop;
using ImageBirb.Core.Ports.Primary;

namespace ImageBirb.ViewModels
{
    internal class MainViewModel : WorkflowViewModel, IDropTarget
    {
        private DragDropViewModel _dragDropViewModel;

        public TagListViewModel TagListViewModel { get; }

        public ThumbnailListViewModel ThumbnailListViewModel { get; }
        
        public SelectedImageViewModel SelectedImageViewModel { get; }
        
        public MainViewModel(IWorkflowAdapter workflowAdapter)
            : base(workflowAdapter)
        {
            TagListViewModel = new TagListViewModel(workflowAdapter);
            ThumbnailListViewModel = new ThumbnailListViewModel(workflowAdapter);
            SelectedImageViewModel = new SelectedImageViewModel(workflowAdapter);

            InitDragDrop(workflowAdapter);
        }

        #region Drag & Drop

        private void InitDragDrop(IWorkflowAdapter workflowAdapter)
        {
            _dragDropViewModel = new DragDropViewModel(workflowAdapter, () =>
            {
                if (ThumbnailListViewModel.UpdateThumbnailsCommand.CanExecute(null))
                {
                    ThumbnailListViewModel.UpdateThumbnailsCommand.Execute(null);
                }
            });
        }
        
        public async void DragOver(IDropInfo dropInfo)
        {
            await _dragDropViewModel.DragOverAsync(dropInfo);
        }

        
        public async void Drop(IDropInfo dropInfo)
        {
            await _dragDropViewModel.DropAsync(dropInfo);
        }

        #endregion
    }
}
