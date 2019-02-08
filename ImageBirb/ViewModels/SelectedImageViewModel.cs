using GalaSoft.MvvmLight.CommandWpf;
using ImageBirb.Common;
using ImageBirb.Core.Common;
using ImageBirb.Core.Ports.Primary;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ImageBirb.ViewModels
{
    /// <summary>
    /// Handles the selected image and its tags.
    /// </summary>
    internal class SelectedImageViewModel : WorkflowViewModel
    {
        private Image _selectedImage;

        private double _scaleX;

        private double _scaleY;

        private readonly TagListViewModel _tagListViewModel;

        public Image SelectedImage
        {
            get => _selectedImage;
            private set
            {
                Set(ref _selectedImage, value);
                RaisePropertyChanged(nameof(IsImageSelected));
                RaisePropertyChanged(nameof(ZoomControlVisibility));
            }
        }

        public bool IsImageSelected => SelectedImage != null;

        public Visibility ZoomControlVisibility => IsImageSelected ? Visibility.Visible : Visibility.Collapsed;

        public double ScaleX
        {
            get => _scaleX;
            set => Set(ref _scaleX, value);
        }

        public double ScaleY
        {
            get => _scaleY;
            set => Set(ref _scaleY, value);
        }
        public ICommand AddTagCommand { get; }

        public ICommand RemoveTagCommand { get; }

        public ICommand ShowImageCommand { get; }

        public ICommand ZoomInCommand { get; }

        public ICommand ZoomOutCommand { get; }
        
        public SelectedImageViewModel(IWorkflowAdapter workflows, TagListViewModel tagListViewModel) 
            : base(workflows)
        {
            _tagListViewModel = tagListViewModel;
            
            AddTagCommand = new RelayCommand<string>(ExecuteAddTagCommand, CanExecuteAddTagCommand);
            RemoveTagCommand = new RelayCommand<string>(ExecuteRemoveTagCommand);
            ShowImageCommand = new RelayCommand<Image>(ExecuteShowImageCommand);
            ZoomInCommand = new RelayCommand(ExecuteZoomInCommand);
            ZoomOutCommand = new RelayCommand(ExecuteZoomOutCommand);

            ScaleX = 1;
            ScaleY = 1;
        }

        private void ExecuteZoomOutCommand()
        {
            ScaleX -= 0.1;
            ScaleY -= 0.1;
        }

        private void ExecuteZoomInCommand()
        {
            ScaleX += 0.1;
            ScaleY += 0.1;
        }

        private async void ExecuteShowImageCommand(Image image)
        {
            await UpdateImage(image);
        }

        private async void ExecuteRemoveTagCommand(string tagName)
        {
            await RunAsync(Workflows.RemoveTag(SelectedImage?.ImageId, tagName));
            await UpdateImage(SelectedImage);
            UpdateTags();
        }

        private async void ExecuteAddTagCommand(string tagName)
        {
            await RunAsync(Workflows.AddTag(SelectedImage?.ImageId, tagName));
            await UpdateImage(SelectedImage);
            UpdateTags();
        }

        private bool CanExecuteAddTagCommand(string tagName)
        {
            return IsImageSelected && 
                   !string.IsNullOrEmpty(tagName) && 
                   !SelectedImage.Tags.Contains(tagName);
        }

        private async Task UpdateImage(Image image)
        {
            if (image == null)
            {
                SelectedImage = null;
                return;
            }

            await RunAsync(Workflows.LoadImage(image.ImageId),
                r => SelectedImage = r.Image);
        }

        private void UpdateTags()
        {
            _tagListViewModel.UpdateTagsCommand.Exec();
        }
    }
}