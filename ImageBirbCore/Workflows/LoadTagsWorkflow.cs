using ImageBirb.Core.Common;
using ImageBirb.Core.Workflows.Results;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImageBirb.Core.Ports.Secondary;

namespace ImageBirb.Core.Workflows
{
    internal class LoadTagsWorkflow : Workflow<TagsResult>
    {
        private readonly ITagManagementAdapter _tagManagementAdapter;

        public LoadTagsWorkflow(ITagManagementAdapter tagManagementAdapter)
        {
            _tagManagementAdapter = tagManagementAdapter;
        }

        protected override async Task<TagsResult> RunImpl()
        {
            var tags = new List<Tag>(await _tagManagementAdapter.GetTags());

            // Sort tags by count, highest value first.
            tags.Sort((x, y) => -1 * x.Count.CompareTo(y.Count));

            return new TagsResult(ResultState.Success, tags);
        }
    }
}
