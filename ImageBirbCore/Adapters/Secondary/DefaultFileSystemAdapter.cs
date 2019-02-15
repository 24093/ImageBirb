using ImageBirb.Core.Ports.Secondary;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImageBirb.Core.Adapters.Secondary
{
    internal class DefaultFileSystemAdapter : IFileSystemAdapter
    {
        public async Task<byte[]> ReadBinaryFile(string filename)
        {
            return await Task.Run(() => File.ReadAllBytes(filename));
        }

        public async Task<IList<string>> GetFilenamesFromDirectory(string directory, IList<string> extensions)
        {
            return await Task.Run(() =>
            {
                return new List<string>(Directory.EnumerateFiles(directory, "*.*", SearchOption.AllDirectories)
                    .Where(x => extensions.Any(x.ToLower().EndsWith)));
            });
        }
    }
}
