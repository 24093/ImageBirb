using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImageBirb.Core.Ports.Secondary
{
    /// <summary>
    /// This adapter handles the access to the local file system.
    /// </summary>
    public interface IFileSystemAdapter
    {
        /// <summary>
        /// Read a binary file.
        /// </summary>
        /// <param name="filename">Full file name.</param>
        /// <returns>Binary content of the file.</returns>
        Task<byte[]> ReadBinaryFile(string filename);

        /// <summary>
        /// Get all filenames from a directory.
        /// </summary>
        /// <param name="directory">Directory to search for (including subdirectories).</param>
        /// <param name="extensions">Extensions of files to consider. Use lower case.</param>
        /// <returns></returns>
        Task<IList<string>> GetFilenamesFromDirectory(string directory, IList<string> extensions);
    }
}