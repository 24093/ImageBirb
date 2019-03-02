using ImageBirb.Core.BusinessObjects;
using ImageBirb.Core.Ports.Secondary;
using LiteDB;

namespace ImageBirb.Core.Adapters
{
    public class LiteDbAdapter : IDatabaseAdapter
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

            MapTypes();
        }

        private void MapTypes()
        {
            var mapper = BsonMapper.Global;

            mapper.Entity<Setting>()
                .Id(x => x.Key);

            mapper.Entity<Image>()
                .Id(x => x.ImageId)
                .Ignore(x => x.ImageData)
                .Ignore(x => x.ThumbnailData);
        }
    }
}
