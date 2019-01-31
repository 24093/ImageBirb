using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using ImageBirb.Core.Common;
using ImageBirb.Core.Extensions;
using ImageBirb.Core.Ports.Primary;

namespace ImageBirb.ViewModels
{
    internal class TagListViewModel : WorkflowViewModel
    {
        public ObservableCollection<Tag> Tags { get; }

        public ICommand UpdateTagsCommand { get; }

        public TagListViewModel(IWorkflowAdapter workflowAdapter) 
            : base(workflowAdapter)
        {
            Tags = new ObservableCollection<Tag>();

            UpdateTagsCommand = new RelayCommand(async () => await UpdateTags());
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