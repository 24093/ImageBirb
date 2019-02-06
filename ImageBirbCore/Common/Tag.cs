using System.Diagnostics;

namespace ImageBirb.Core.Common
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Tag
    {
        public string Name { get; set; }

        public int Count { get; set; }

        public Tag(string name, int count)
        {
            Name = name;
            Count = count;
        }

        private string DebuggerDisplay => Name;
    }
}
