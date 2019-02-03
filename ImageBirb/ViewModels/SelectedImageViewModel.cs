using GalaSoft.MvvmLight.CommandWpf;
using ImageBirb.Core.Common;
using ImageBirb.Core.Extensions;
using ImageBirb.Core.Ports.Primary;
using System.Collections.ObjectModel;
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

        private TagListViewModel _tagListViewModel;

        public Image SelectedImage
        {
            get => _selectedImage;
            private set => Set(ref _selectedImage, value);
        }

        public ObservableCollection<string> SelectedImageTags { get; }

        public bool IsImageSelected => SelectedImage != null;

        public ICommand AddTagCommand { get; }

        public ICommand RemoveTagCommand { get; }

        public ICommand ShowImageCommand { get; }
        
        public SelectedImageViewModel(IWorkflowAdapter workflowAdapter, TagListViewModel tagListViewModel) 
            : base(workflowAdapter)
        {
            _tagListViewModel = tagListViewModel;

            SelectedImageTags = new ObservableCollection<string>();

            AddTagCommand = new RelayCommand<string>(async tagName => await AddTag(tagName), CanExecuteAddTagCommand);
            RemoveTagCommand = new RelayCommand<string>(async tagName => await RemoveTag(tagName));
            ShowImageCommand = new RelayCommand<Image>(async image => await UpdateImage(image));
        }

        private bool CanExecuteAddTagCommand(string tagName)
        {
            return (!string.IsNullOrEmpty(tagName) && !SelectedImageTags.Contains(tagName));
        }

        private async Task UpdateImage(Image image)
        {
            if (image == null)
            {
                SelectedImage = null;
                SelectedImageTags.Clear();
                return;
            }

            var result = await LoadImage(image.ImageId);

            if (result.IsSuccess)
            {
                SelectedImage = result.Image;
                SelectedImageTags.ReplaceItems(SelectedImage.Tags);
            }
        }

        private void UpdateTags()
        {
            if (_tagListViewModel.UpdateTagsCommand.CanExecute(null))
            {
                _tagListViewModel.UpdateTagsCommand.Execute(null);
            }
        }

        private async Task RemoveTag(string tagName)
        {
            await RemoveTag(SelectedImage?.ImageId, tagName);
            await UpdateImage(SelectedImage);
            UpdateTags();
        }

        private async Task AddTag(string tagName)
        {
            await AddTag(SelectedImage?.ImageId, tagName);
            await UpdateImage(SelectedImage);
            UpdateTags();
        }
    }
}