using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using ImageBirb.Core.Common;
using ImageBirb.Core.Extensions;
using ImageBirb.Core.Ports.Primary;
using ImageBirb.Core.Workflows.Results;

namespace ImageBirb.ViewModels
{
    /// <summary>
    /// Handles the thumbnails list.
    /// </summary>
    internal class ThumbnailListViewModel : WorkflowViewModel
    {
        private readonly SelectedImageViewModel _selectedImageViewModel;

        private Image _selectedThumbnail;

        public ObservableCollection<Image> Thumbnails { get; }

        public ICommand UpdateThumbnailsCommand { get; }

        public ICommand FilterThumbnailsByTagsCommand { get; }

        public ICommand NextCommand { get; }

        public ICommand PreviousCommand { get; }

        public Image SelectedThumbnail
        {
            get => _selectedThumbnail;
            set
            {
                Set(ref _selectedThumbnail, value);

                if (_selectedImageViewModel.ShowImageCommand.CanExecute(_selectedThumbnail))
                {
                    _selectedImageViewModel.ShowImageCommand.Execute(_selectedThumbnail);
                }
            }
        }

        public ThumbnailListViewModel(IWorkflowAdapter workflowAdapter, SelectedImageViewModel selectedImageViewModel) 
            : base(workflowAdapter)
        {
            _selectedImageViewModel = selectedImageViewModel;

            Thumbnails = new ObservableCollection<Image>();

            FilterThumbnailsByTagsCommand = new RelayCommand<IList>(async tags => await FilterThumbnailsByTags(tags));
            UpdateThumbnailsCommand = new RelayCommand(async () => await UpdateThumbnails());

            NextCommand = new RelayCommand(ShowNextImage);
            PreviousCommand = new RelayCommand(ShowPreviousImage);
        }

        private async Task UpdateThumbnails()
        {
            var result = await WorkflowAdapter.LoadThumbnails();

            if (result.IsSuccess)
            {
                Thumbnails.ReplaceItems(result.Thumbnails);
            }
        }

        private async Task FilterThumbnailsByTags(IList tags)
        {
            var tagsList = tags.Cast<Tag>().Select(x => x.Name).ToList();

            ThumbnailsResult result;

            if (tagsList.Any())
            {
                result = await WorkflowAdapter.LoadThumbnailsByTags(tagsList);
            }
            else
            {
                result = await WorkflowAdapter.LoadThumbnails();
            }

            if (result.IsSuccess)
            {
                Thumbnails.ReplaceItems(result.Thumbnails);
            }
        }

        private void ShowNextImage()
        {
            if (SelectedThumbnail == null)
            {
                SelectedThumbnail = Thumbnails.FirstOrDefault();
            }
            else
            {
                var index = Thumbnails.IndexOf(SelectedThumbnail);

                if (index < Thumbnails.Count - 1)
                {
                    SelectedThumbnail = Thumbnails[++index];
                }
                else
                {
                    SelectedThumbnail = Thumbnails.FirstOrDefault();
                }
            }
        }

        private void ShowPreviousImage()
        {
            if (SelectedThumbnail == null)
            {
                SelectedThumbnail = Thumbnails.LastOrDefault();
            }
            else
            {
                var index = Thumbnails.IndexOf(SelectedThumbnail);

                if (index > 0)
                {
                    SelectedThumbnail = Thumbnails[--index];
                }
                else
                {
                    SelectedThumbnail = Thumbnails.LastOrDefault();
                }
            }
        }
    }
}