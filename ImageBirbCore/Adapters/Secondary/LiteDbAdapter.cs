using ImageBirb.Core.Common;
using ImageBirb.Core.Ports.Secondary;
using LiteDB;

namespace ImageBirb.Core.Adapters.Secondary
{
    internal class LiteDbAdapter : IDatabaseAdapter
    {
        private readonly LiteDatabase _liteDatabase;

        public LiteStorage FileStorage => _liteDatabase.FileStorage;

        public LiteCollection<Image> ImageCollection => _liteDatabase.GetCollection<Image>();

        public LiteCollection<Setting> SettingCollection => _liteDatabase.GetCollection<Setting>();

        public string ConnectionString { get; }

        public LiteDbAdapter(string databaseFilename)
        {
            ConnectionString = databaseFilename;
            _liteDatabase = new LiteDatabase(databaseFilename);
        }
    }
}
