using GalaSoft.MvvmLight.CommandWpf;
using ImageBirb.Core.Common;
using ImageBirb.Core.Extensions;
using ImageBirb.Core.Ports.Primary;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ImageBirb.Common;

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
            await RunAsyncDispatch(Workflows.LoadTags(), r => Tags.ReplaceItems(r.Tags));
        }
    }
}
