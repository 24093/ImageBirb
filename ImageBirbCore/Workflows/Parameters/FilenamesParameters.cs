using System.Collections.Generic;

namespace ImageBirb.Core.Workflows.Parameters
{
   internal class FilenamesParameters : WorkflowParameters
    {
        public IList<string> Filenames { get; }

        public string Directory { get; }

        public FilenamesParameters(IList<string> filenames)
        {
            Filenames = filenames;
        }

        public FilenamesParameters(string directory)
        {
            Directory = directory;
        }
    }
}