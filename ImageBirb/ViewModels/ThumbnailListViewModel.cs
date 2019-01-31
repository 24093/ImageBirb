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
    internal class ThumbnailListViewModel : WorkflowViewModel
    {
        public ObservableCollection<Image> Thumbnails { get; }

        public ICommand UpdateThumbnailsCommand { get; }

        public ICommand FilterThumbnailsByTagsCommand { get; }

        public ThumbnailListViewModel(IWorkflowAdapter workflowAdapter) 
            : base(workflowAdapter)
        {
            Thumbnails = new ObservableCollection<Image>();

            FilterThumbnailsByTagsCommand = new RelayCommand<IList>(async tags => await FilterThumbnailsByTags(tags));
            UpdateThumbnailsCommand = new RelayCommand(async () => await UpdateThumbnails());
        }

        private async Task UpdateThumbnails()
        {
            var result = await LoadThumbnails();

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
                result = await LoadThumbnailsByTags(tagsList);
            }
            else
            {
                result = await LoadThumbnails();
            }

            if (result.IsSuccess)
            {
                Thumbnails.ReplaceItems(result.Thumbnails);
            }
        }
    }
}