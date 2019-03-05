using ImageBirb.Common;
using ImageBirb.Core.BusinessObjects;
using ImageBirb.Core.Ports.Primary;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ImageBirb.ViewModels
{
    /// <summary>
    /// Handles the global tag list.
    /// </summary>
    internal class TagListViewModel : WorkflowViewModel
    {
        public ObservableCollection<Tag> Tags { get; }

        public ICommand UpdateTagsCommand { get; }

        public TagListViewModel(IWorkflowAdapter workflows) 
            : base(workflows)
        {
            Tags = new ObservableCollection<Tag>();

            UpdateTagsCommand = new RelayCommand(ExecuteUpdateTagsCommand);
        }

        private async void ExecuteUpdateTagsCommand()
        {
            await RunAsyncDispatch(Workflows.LoadTags(), tags => Tags.ReplaceItems(tags));
        }
    }
}
