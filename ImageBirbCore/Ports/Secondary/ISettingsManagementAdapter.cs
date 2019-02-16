using System.Threading.Tasks;
using ImageBirb.Core.Common;

namespace ImageBirb.Core.Ports.Secondary
{
    public interface ISettingsManagementAdapter
    {
        Task UpdateSettings(Setting setting);

        Task<Setting> GetSetting(string key);
    }
}