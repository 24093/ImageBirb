using System.IO;
using System.Threading.Tasks;
using ImageBirb.Core.Ports.Secondary;

namespace ImageBirb.Core.Adapters.Secondary
{
    public class DefaultFileSystemAdapter : IFileSystemAdapter
    {
        public Task<byte[]> ReadBinaryFile(string filename)
        {
            return Task.Run(() => File.ReadAllBytes(filename));
        }
    }
}
