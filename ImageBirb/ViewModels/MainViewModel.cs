using GalaSoft.MvvmLight.CommandWpf;
using GongSolutions.Wpf.DragDrop;
using ImageBirb.Core.Common;
using ImageBirb.Core.Extensions;
using ImageBirb.Core.Ports.Primary;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ImageBirb.ViewModels
{
    internal class MainViewModel : WorkflowViewModel, IDropTarget
    {
        private Image _selectedImage;

        public ObservableCollection<Image> Thumbnails { get; }

        public ObservableCollection<Tag> Tags { get; }

        public Image SelectedImage
        {
            get => _selectedImage;
            private set => Set(ref _selectedImage, value);
        }

        public ICommand ShowImageCommand { get; }

        public ICommand AddTagCommand { get; }

        public ICommand RemoveTagCommand { get; }

        public ICommand UpdateThumbnailsCommand { get; }

        public MainViewModel(IWorkflowAdapter workflowAdapter)
            : base(workflowAdapter)
        {
            Thumbnails = new ObservableCollection<Image>();
            Tags = new ObservableCollection<Tag>();

            ShowImageCommand = new RelayCommand<Image>(async image => await UpdateImage(image));
            AddTagCommand = new RelayCommand<string>(async tagName => await AddTag(tagName));
            RemoveTagCommand = new RelayCommand<string>(async tagName => await RemoveTag(tagName));
            UpdateThumbnailsCommand = new RelayCommand(async () => await UpdateThumbnails());
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

            await UpdateThumbnails();
        }

        private async Task UpdateImage(Image image)
        {
            var result = await LoadImage(image.ImageId);

            if (result.IsSuccess)
            {
                SelectedImage = result.Image;
            }
        }

        private async Task UpdateThumbnails()
        {
            var result = await LoadThumbnails();

            if (result.IsSuccess)
            {
                Thumbnails.ReplaceItems(result.Thumbnails);
            }
        }

        private async Task UpdateTags()
        {
            var result = await LoadTags();

            if (result.IsSuccess)
            {
                Tags.ReplaceItems(result.Tags);
            }
        }
    }
}
