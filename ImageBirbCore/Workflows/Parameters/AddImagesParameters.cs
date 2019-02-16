using System.Collections.Generic;

namespace ImageBirb.Core.Workflows.Parameters
{
    internal class AddImagesParameters : WorkflowParameters
    {
        public IList<string> FileNames { get; }

        public string Directory { get; }

        public bool AddFolder { get; }

        public AddImagesParameters(IList<string> fileNames)
        {
            FileNames = fileNames;
            AddFolder = false;
        }

        public AddImagesParameters(string directory)
        {
            Directory = directory;
            AddFolder = true;
        }
    }
}