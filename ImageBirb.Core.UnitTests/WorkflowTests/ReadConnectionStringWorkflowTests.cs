using System.Threading.Tasks;
using ImageBirb.Core.Ports.Secondary;
using ImageBirb.Core.Workflows;
using Moq;
using Xunit;

namespace ImageBirb.Core.UnitTests.WorkflowTests
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
            Assert.True(result.IsSuccess);
            Assert.Equal(_connectionString, result.ConnectionString);
        }
    }
}
