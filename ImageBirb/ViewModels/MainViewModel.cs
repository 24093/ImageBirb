using System.Windows;
using GongSolutions.Wpf.DragDrop;
using ImageBirb.Core.Ports.Primary;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace ImageBirb.ViewModels
{
    /// <summary>
    /// Root view model. Creates and connects view models if necessary.
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
        
        public async void DragOver(IDropInfo dropInfo)
        {
            await _dragDropViewModel.DragOverAsync(dropInfo);
        }
        
        public async void Drop(IDropInfo dropInfo)
        {
            await _dragDropViewModel.DropAsync(dropInfo);
        }
    }
}

