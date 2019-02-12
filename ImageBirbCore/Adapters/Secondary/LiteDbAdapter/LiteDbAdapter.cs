using ImageBirb.Core.Ports.Secondary.DatabaseAdapter;
using LiteDB;

namespace ImageBirb.Core.Adapters.Secondary.LiteDbAdapter
{
    internal class LiteDbAdapter : IDatabaseAdapter
    {
        private readonly LiteDatabase _db;

        public string ConnectionString { get; }

        public IImageManagement ImageManagement { get; }

        public ITagManagement TagManagement { get; }

        public LiteDbAdapter(string databaseFilename)
        {
            ConnectionString = databaseFilename;
            _db = new LiteDatabase(databaseFilename);

            ImageManagement = new LiteDbImageManagement(_db);
            TagManagement = new LiteDbTagManagement(_db);
        }
    }
}
