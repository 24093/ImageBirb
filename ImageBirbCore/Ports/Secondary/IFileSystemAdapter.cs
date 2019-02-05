﻿using System.Threading.Tasks;

namespace ImageBirb.Core.Ports.Secondary
{
    public interface IFileSystemAdapter
    {
        Task<byte[]> ReadBinaryFile(string filename);

        Task<T> ReadJsonFile<T>(string filename);

        Task WriteJsonFile<T>(string filename, T content);
    }
}