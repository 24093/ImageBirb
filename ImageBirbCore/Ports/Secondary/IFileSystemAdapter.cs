﻿using System.Threading.Tasks;

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
    }
}