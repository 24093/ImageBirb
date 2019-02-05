using ImageBirb.Core.Ports.Secondary;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace ImageBirb.Core.Adapters.Secondary
{
    internal class DefaultFileSystemAdapter : IFileSystemAdapter
    {
        public Task<byte[]> ReadBinaryFile(string filename)
        {
            return Task.Run(() => File.ReadAllBytes(filename));
        }

        public Task<T> ReadJsonFile<T>(string filename)
        {
            return Task.Run(() =>
            {
                var json = File.ReadAllText(filename);
                return JsonConvert.DeserializeObject<T>(json);
            });
        }

        public Task WriteJsonFile<T>(string filename, T content)
        {
            return Task.Run(() =>
            {
                var json = JsonConvert.SerializeObject(content);
                File.WriteAllText(filename, json);
            });
        }
    }
}
