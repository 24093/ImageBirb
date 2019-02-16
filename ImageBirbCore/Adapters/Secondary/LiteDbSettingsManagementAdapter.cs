using ImageBirb.Core.Common;
using ImageBirb.Core.Ports.Secondary;
using LiteDB;
using System.Threading.Tasks;

namespace ImageBirb.Core.Adapters.Secondary
{
    internal class LiteDbSettingsManagementAdapter : ISettingsManagementAdapter
    {
        private readonly LiteCollection<Setting> _settingsCollection;

        public LiteDbSettingsManagementAdapter(LiteDbAdapter liteDbAdapter)
        {
            _settingsCollection = liteDbAdapter.SettingCollection;
        }

        public async Task UpdateSettings(Setting setting)
        {
            await Task.Run(() =>
            {
                _settingsCollection.Upsert(setting);
            });
        }

        public async Task<Setting> GetSetting(string key)
        {
            return await Task.Run(() =>
            {
                return _settingsCollection.FindOne(doc => doc.Key == key);
            });
        }

        public async Task<Setting> GetSetting(SettingType type)
        {
            return await GetSetting(type.ToString());
        }
    }
}