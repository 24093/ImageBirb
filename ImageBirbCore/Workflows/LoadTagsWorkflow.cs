using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageBirb.Core.Common;
using ImageBirb.Core.Ports.Secondary;
using ImageBirb.Core.Workflows.Results;

namespace ImageBirb.Core.Workflows
{
    internal class LoadTagsWorkflow : Workflow<TagsResult>
    {
        private readonly IDatabaseAdapter _databaseAdapter;

        public LoadTagsWorkflow(IDatabaseAdapter databaseAdapter)
        {
            _databaseAdapter = databaseAdapter;
        }

        protected override async Task<TagsResult> RunImpl()
        {
            var tags = new List<Tag>(await _databaseAdapter.GetTags());

            // Sort tags by count, highest value first.
            tags.Sort((x, y) => -1 * x.Count.CompareTo(y.Count));

            return new TagsResult(ResultState.Success, tags);
        }
    }
}
