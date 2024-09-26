using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TxtTags.Common
{
    public class SortableObservableCollection<T> : ObservableCollection<T> where T : IComparable<T>
    {
        public SortableObservableCollection() { }
        public SortableObservableCollection(List<T> list)
            : base(list)
        {
        }

        public SortableObservableCollection(IEnumerable<T> collection)
            : base(collection)
        {
        }

        protected override void InsertItem(int index, T item)
        {
            int i = 0;
            bool found = false;
            for (i = 0; i < Items.Count; i++)
            {
                if (item.CompareTo(Items[i]) < 0)
                {
                    found = true;
                    break;
                }
            }

            if (!found) i = Count;

            base.InsertItem(i, item);
        }
        /// <summary>
        /// Sorts the items of the collection in ascending order according to a key.
        /// </summary>
        /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector"/>.</typeparam>
        /// <param name="keySelector">A function to extract a key from an item.</param>
        public void Sort<TKey>(Func<T, TKey> keySelector)
        {
            InternalSort(Items.OrderBy(keySelector));
        }

        /// <summary>
        /// Sorts the items of the collection in ascending order according to a key.
        /// </summary>
        /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector"/>.</typeparam>
        /// <param name="keySelector">A function to extract a key from an item.</param>
        /// <param name="comparer">An <see cref="IComparer{T}"/> to compare keys.</param>
        public void Sort<TKey>(Func<T, TKey> keySelector, IComparer<TKey> comparer)
        {
            InternalSort(Items.OrderBy(keySelector, comparer));
        }

        /// <summary>
        /// Moves the items of the collection so that their orders are the same as those of the items provided.
        /// </summary>
        /// <param name="sortedItems">An <see cref="IEnumerable{T}"/> to provide item orders.</param>
        private void InternalSort(IEnumerable<T> sortedItems)
        {
            var sortedItemsList = sortedItems.ToList();

            foreach (var item in sortedItemsList)
            {
                Move(IndexOf(item), sortedItemsList.IndexOf(item));
            }
        }
    }
}
