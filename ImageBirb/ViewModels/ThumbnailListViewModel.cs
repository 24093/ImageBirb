using GalaSoft.MvvmLight.CommandWpf;
using ImageBirb.Core.Common;
using ImageBirb.Core.Extensions;
using ImageBirb.Core.Ports.Primary;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

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
                _selectedImageViewModel.ShowImageCommand.Exec(_selectedThumbnail);
            }
        }

        public ThumbnailListViewModel(IWorkflowAdapter workflows, SelectedImageViewModel selectedImageViewModel) 
            : base(workflows)
        {
            _selectedImageViewModel = selectedImageViewModel;

            Thumbnails = new ObservableCollection<Image>();

            FilterThumbnailsByTagsCommand = new RelayCommand<IList>(ExecuteFilterThumbnailsByTagsCommand);
            UpdateThumbnailsCommand = new RelayCommand(ExecuteUpdateThumbnailsCommand);

            NextCommand = new RelayCommand(ExecuteNextCommand);
            PreviousCommand = new RelayCommand(ExecutePreviousCommand);
        }

        private void ExecutePreviousCommand()
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

        private void ExecuteNextCommand()
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

        private async void ExecuteUpdateThumbnailsCommand()
        {
            await UpdateThumbnails();
        }

        private async void ExecuteFilterThumbnailsByTagsCommand(IList tags)
        {
            var tagsList = tags.Cast<Tag>().Select(x => x.Name).ToList();
            var workflow = tagsList.Any() ? Workflows.LoadThumbnailsByTags(tagsList) : Workflows.LoadThumbnails();

            await RunAsyncDispatch(workflow, r => Thumbnails.ReplaceItems(r.Thumbnails));
        }

        private async Task UpdateThumbnails()
        {
            await RunAsyncDispatch(Workflows.LoadThumbnails(), r => Thumbnails.ReplaceItems(r.Thumbnails));
        }
    }
}