using System.Globalization;
using ImageBirb.Core.Common;
using ImageBirb.Core.Ports.Secondary;
using LiteDB;
using System.Threading.Tasks;

namespace ImageBirb.Core.Adapters.Secondary
{
    internal class LiteDbSettingsManagementAdapter : ISettingsManagementAdapter
    {
        private readonly object lockObject = new object();

        private readonly LiteCollection<Setting> _settingsCollection;

        public LiteDbSettingsManagementAdapter(LiteDbAdapter liteDbAdapter)
        {
            _settingsCollection = liteDbAdapter.SettingCollection;

            CreateDefaultSettings();
        }

        public async Task UpdateSetting(Setting setting)
        {
            await Task.Run(() =>
            {
                lock (lockObject)
                {
                    _settingsCollection.Upsert(setting);
                }
            });
        }

        public async Task<Setting> GetSetting(string key)
        {
            return await Task.Run(() =>
            {
                lock (lockObject)
                {
                    return _settingsCollection.FindOne(doc => doc.Key == key);
                }
            });
        }

        public async Task<Setting> GetSetting(SettingType type)
        {
            return await GetSetting(type.ToString());
        }
        
        private void CreateDefaultSettings()
        {
            lock (lockObject)
            {
                Task.Run(async () =>
                {
                    await CheckAndSetDefault(SettingType.AddFolders, false);
                    await CheckAndSetDefault(SettingType.ImageStorage, ImageStorageType.LinkToSource);
                    await CheckAndSetDefault(SettingType.IgnoreSimilarImages, true);
                    await CheckAndSetDefault(SettingType.SimilarityThreshold, 0.9);
                });
            }
        }

        private async Task CheckAndSetDefault<T>(SettingType type, T value)
        {
            if (await GetSetting(type) == null)
            {
                await UpdateSetting(new Setting
                {
                    Key = type.ToString(),
                    Value = value.ToString()
                });
            }
        }

        private async Task CheckAndSetDefault(SettingType type, double value)
        {
            if (await GetSetting(type) == null)
            {
                await UpdateSetting(new Setting
                {
                    Key = type.ToString(),
                    Value = value.ToString(CultureInfo.InvariantCulture)
                });
            }
        }
    }
}