using ImageBirb.Common;
using ImageBirb.Core.BusinessObjects;
using ImageBirb.Core.Ports.Primary;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImageBirb.ViewModels
{
    /// <summary>
    /// Handles the selected image and its tags.
    /// </summary>
    internal class SelectedImageViewModel : WorkflowViewModel
    {
        private Image _selectedImage;

        private readonly TagListViewModel _tagListViewModel;

        public Image SelectedImage
        {
            get => _selectedImage;
            private set
            {
                Set(ref _selectedImage, value);
                OnPropertyChanged(nameof(IsImageSelected));
            }
        }

        public bool IsImageSelected => SelectedImage != null;

        public ICommand AddTagCommand { get; }

        public ICommand RemoveTagCommand { get; }

        public ICommand ShowImageCommand { get; }
        
        public SelectedImageViewModel(IWorkflowAdapter workflows, TagListViewModel tagListViewModel) 
            : base(workflows)
        {
            _tagListViewModel = tagListViewModel;
            
            AddTagCommand = new RelayCommand<string>(ExecuteAddTagCommand, CanExecuteAddTagCommand);
            RemoveTagCommand = new RelayCommand<string>(ExecuteRemoveTagCommand);
            ShowImageCommand = new RelayCommand<Image>(ExecuteShowImageCommand);
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
                i => SelectedImage = i);
        }

        private void UpdateTags()
        {
            _tagListViewModel.UpdateTagsCommand.Execute(null);
        }
    }
}