using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ImageBirb.Common
{
    public static class ObservableCollectionExtensions
    {
        public static void ReplaceItems<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
        {
            collection.Clear();
            collection.AddRange(items);
        }

        public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                collection.Add(item);
            }
        }
    }
}
