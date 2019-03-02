using ImageBirb.Core.BusinessObjects;
using System.Threading.Tasks;

namespace ImageBirb.Core.Ports.Secondary
{
    /// <summary>
    /// Database module that handles settings. 
    /// </summary>
    public interface ISettingsManagementAdapter
    {
        /// <summary>
        /// Store a setting.
        /// </summary>
        /// <param name="setting">The setting to be stored.</param>
        Task UpdateSetting(Setting setting);

        /// <summary>
        /// Get a setting by its key.
        /// </summary>
        /// <param name="key">The setting's key.</param>
        /// <returns>The setting or null if not found.</returns>
        Task<Setting> GetSetting(string key);
    }
}