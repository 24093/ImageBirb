namespace ImageBirb.Core.Common
{
    public class Tag
    {
        public string Name { get; set; }

        public int Count { get; set; }

        public Tag(string name, int count)
        {
            Name = name;
            Count = count;
        }
    }
}
