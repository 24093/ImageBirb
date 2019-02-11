using ImageBirb.Core.Ports.Secondary;
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
    }
}
