using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using ImageBirb.Core.Common;
using ImageBirb.Core.Ports.Primary;

namespace ImageBirb.ViewModels
{
    internal class SelectedImageViewModel : WorkflowViewModel
    {
        private Image _selectedImage;

        public Image SelectedImage
        {
            get => _selectedImage;
            private set => Set(ref _selectedImage, value);
        }

        public ICommand AddTagCommand { get; }

        public ICommand RemoveTagCommand { get; }

        public ICommand ShowImageCommand { get; }

        public SelectedImageViewModel(IWorkflowAdapter workflowAdapter) 
            : base(workflowAdapter)
        {
            AddTagCommand = new RelayCommand<string>(async tagName => await AddTag(tagName));
            RemoveTagCommand = new RelayCommand<string>(async tagName => await RemoveTag(tagName));
            ShowImageCommand = new RelayCommand<Image>(async image => await UpdateImage(image));
        }

        private async Task UpdateImage(Image image)
        {
            if (image == null)
            {
                SelectedImage = null;
                return;
            }

            var result = await LoadImage(image.ImageId);

            if (result.IsSuccess)
            {
                SelectedImage = result.Image;
            }
        }

        private async Task RemoveTag(string tagName)
        {
            await RemoveTag(SelectedImage?.ImageId, tagName);
            await UpdateImage(SelectedImage);
        }

        private async Task AddTag(string tagName)
        {
            await AddTag(SelectedImage?.ImageId, tagName);
            await UpdateImage(SelectedImage);
        }
    }
}