using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DSG.Common.Extensions
{
    public static class ObservableCollectionExtension
    {
        public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> range)
        {
            foreach(T item in range)
            {
                collection.Add(item);
            }
        }
    }
}
