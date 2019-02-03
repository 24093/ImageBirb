using GongSolutions.Wpf.DragDrop;
using ImageBirb.Core.Ports.Primary;

namespace ImageBirb.ViewModels
{
    /// <summary>
    /// Root view model. Creates child view models.
    /// </summary>
    internal class MainViewModel : WorkflowViewModel, IDropTarget
    {
        private readonly DragDropViewModel _dragDropViewModel;

        public TagListViewModel TagListViewModel { get; }

        public ThumbnailListViewModel ThumbnailListViewModel { get; }
        
        public SelectedImageViewModel SelectedImageViewModel { get; }

        public FlyoutViewModel FlyoutViewModel { get; }
        
        public ImageManagementViewModel ImageManagementViewModel { get; }
        
        public MainViewModel(
            IWorkflowAdapter workflowAdapter,
            DragDropViewModel dragDropViewModel,
            TagListViewModel tagListViewModel,
            ThumbnailListViewModel thumbnailListViewModel,
            SelectedImageViewModel selectedImageViewModel,
            FlyoutViewModel flyoutViewModel,
            ImageManagementViewModel imageManagementViewModel)
            : base(workflowAdapter)
        {
            _dragDropViewModel = dragDropViewModel;
            TagListViewModel = tagListViewModel;
            ThumbnailListViewModel = thumbnailListViewModel;
            SelectedImageViewModel = selectedImageViewModel;
            FlyoutViewModel = flyoutViewModel;
            ImageManagementViewModel = imageManagementViewModel;
        }
        
        public void DragOver(IDropInfo dropInfo)
        {
            _dragDropViewModel.DragOver(dropInfo);
        }
        
        public void Drop(IDropInfo dropInfo)
        {
            _dragDropViewModel.Drop(dropInfo);
        }
    }
}

