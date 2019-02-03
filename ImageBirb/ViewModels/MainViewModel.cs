using System.Threading.Tasks;
using GalaSoft.MvvmLight.CommandWpf;
using GongSolutions.Wpf.DragDrop;
using ImageBirb.Core.Ports.Primary;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Input;

namespace ImageBirb.ViewModels
{
    /// <summary>
    /// Root view model. Creates and connects view models if necessary.
    /// </summary>
    internal class MainViewModel : WorkflowViewModel, IDropTarget
    {
        private DragDropViewModel _dragDropViewModel;

        public TagListViewModel TagListViewModel { get; }

        public ThumbnailListViewModel ThumbnailListViewModel { get; }
        
        public SelectedImageViewModel SelectedImageViewModel { get; }

        public FlyoutViewModel FlyoutViewModel { get; }

        public ICommand AddImageFromFileCommand { get; }

        public ICommand RemoveImageCommand { get; }

        public MainViewModel(IWorkflowAdapter workflowAdapter)
            : base(workflowAdapter)
        {
            TagListViewModel = new TagListViewModel(workflowAdapter);
            ThumbnailListViewModel = new ThumbnailListViewModel(workflowAdapter);
            SelectedImageViewModel = new SelectedImageViewModel(workflowAdapter);
            FlyoutViewModel = new FlyoutViewModel(() => SelectedImageViewModel.SelectedImage != null);

            AddImageFromFileCommand = new RelayCommand(async () => await AddFilesFromOpenFileDialog());
            RemoveImageCommand = new RelayCommand(ExecuteRemoveImageCommand);

            InitDragDrop(workflowAdapter);
        }

        private void ExecuteRemoveImageCommand()
        {
            throw new System.NotImplementedException();
        }

        private async Task AddFilesFromOpenFileDialog()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Image Files(*.bmp; *.jpg; *.jpeg; *.png)| *.bmp; *.jpg; *.jpeg; *.png | All files(*.*) | *.*",
                Multiselect = true
            };

            var dialogResult = dialog.ShowDialog(Application.Current.MainWindow);

            if (dialogResult == true)
            {
                foreach (var filename in dialog.FileNames)
                {
                    await AddImage(filename);
                }
            }

            ThumbnailListViewModel.UpdateThumbnailsCommand.Execute(null);
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

