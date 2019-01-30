using System.Threading.Tasks;

namespace ImageBirb.Core.Ports.Secondary
{
    public interface IFileSystemAdapter
    {
        Task<byte[]> ReadBinaryFile(string filename);
    }
}