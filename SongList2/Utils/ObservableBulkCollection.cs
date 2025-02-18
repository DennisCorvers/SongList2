using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace SongList2.Utils
{
    public class ObservableBulkCollection<T> : ObservableCollection<T>
    {
        public ObservableBulkCollection()
            : base() { }

        public ObservableBulkCollection(IEnumerable<T> collection)
            : base(collection) { }

        public void AddRange(IEnumerable<T> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            var initialCount = Count;

            foreach (var item in items)
            {
                Items.Add(item);
            }

            if (Count != initialCount)
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        public void RemoveRange(IEnumerable<T> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            var initialCount = Count;
            var itemsToRemove = new List<T>(items);

            foreach (var item in itemsToRemove)
            {
                Items.Remove(item);
            }

            if (Count != initialCount)
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }
    }
}
