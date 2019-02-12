using ImageBirb.Core.Ports.Secondary;
using ImageBirb.Core.Workflows;
using ImageBirb.Core.Workflows.Results;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace ImageBirbCoreUnitTests.WorkflowTests
{
    public class ReadConnectionStringWorkflowTests
    {
        private readonly string _connectionString;

        private readonly Mock<IDatabaseAdapter> _databaseAdapter;

        public ReadConnectionStringWorkflowTests()
        {
            _connectionString = "http://www.mudda.de";

            _databaseAdapter = new Mock<IDatabaseAdapter>();
            _databaseAdapter.SetupGet(x => x.ConnectionString).Returns(_connectionString);
        }

        [Fact]
        public async Task SuccessfullyReadsConnectionString()
        {
            // arrange
            var workflow = new ReadConnectionStringWorkflow(_databaseAdapter.Object);

            // act
            var result = await workflow.Run();

            // assert
            Assert.Equal(ResultState.Success, result.State);
            Assert.Equal(_connectionString, result.ConnectionString);
        }
    }
}
