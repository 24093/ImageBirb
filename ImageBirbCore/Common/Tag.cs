namespace ImageBirb.Core.Common
{
    public class Tag
    {
        public string Name { get; set; }

        public int Count { get; set; }

        public Tag()
        {
        }

        public Tag(string name, int count)
            : this()
        {
            Name = name;
            Count = count;
        }
    }
}
