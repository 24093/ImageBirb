using GalaSoft.MvvmLight.CommandWpf;
using ImageBirb.Common;
using ImageBirb.Core.BusinessObjects;
using ImageBirb.Core.Ports.Primary;
using System;
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

        public ICommand RandomCommand { get; }

        public Image SelectedThumbnail
        {
            get => _selectedThumbnail;
            set
            {
                Set(ref _selectedThumbnail, value);
                _selectedImageViewModel.ShowImageCommand.Execute(_selectedThumbnail);
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
            RandomCommand = new RelayCommand(ExecuteRandomCommand);
        }

        private void ExecuteRandomCommand()
        {
            var random = new Random();
            var index = random.Next(0, Thumbnails.Count - 1);

            SelectedThumbnail = Thumbnails[index];
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

            await RunAsyncDispatch(Workflows.LoadThumbnails(tagsList), 
                thumbnails => Thumbnails.ReplaceItems(thumbnails));
        }

        private async Task UpdateThumbnails()
        {
            await RunAsyncDispatch(Workflows.LoadThumbnails(null), 
                thumbnails => Thumbnails.ReplaceItems(thumbnails));
        }
    }
}