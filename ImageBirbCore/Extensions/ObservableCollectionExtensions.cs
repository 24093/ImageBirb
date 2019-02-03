using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ImageBirb.Core.Extensions
{
    public static class ObservableCollectionExtensions
    {
        public static void ReplaceItems<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
        {
            try
            {
                collection.Clear();
                collection.AddRange(items);
            }
            catch (Exception e)
            {
                 throw;
            }
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
