using System.Threading.Tasks;
using ImageBirb.Core.Ports.Secondary;
using ImageBirb.Core.Workflows.Results;

namespace ImageBirb.Core.Workflows
{
    internal class ReadConnectionStringWorkflow : Workflow<ConnectionStringResult>
    {
        private readonly IDatabaseAdapter _databaseAdapter;

        public ReadConnectionStringWorkflow(IDatabaseAdapter databaseAdapter)
        {
            _databaseAdapter = databaseAdapter;
        }

        protected override async Task<ConnectionStringResult> Run()
        {
            var connectionString = _databaseAdapter.ConnectionString;
            return new ConnectionStringResult(ResultState.Success, connectionString);
        }
    }
}