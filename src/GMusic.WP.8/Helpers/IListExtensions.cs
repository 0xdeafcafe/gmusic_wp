using System;
using System.Collections.Generic;

namespace GMusic.WP._8.Helpers
{
    public static class ListExtensions
    {
        public enum Destination
        {
            Start,
            End
        }
        public static void Move<T>(this IList<T> list, Destination dest)
        {
            T item;
            var newIndex = 0;
            var oldIndex = 0;

            switch(dest)
            {
                case Destination.Start:
                    oldIndex = 0;
                    item = list[oldIndex];
                    list.RemoveAt(oldIndex);

                    newIndex = list.Count - 1;
                    if (newIndex > oldIndex) newIndex--;
                    list.Insert(newIndex, item);
                    break;
                case Destination.End:
                    oldIndex = list.Count - 1;
                    item = list[oldIndex];
                    list.RemoveAt(oldIndex);

                    newIndex = 0;
                    if (newIndex > oldIndex) newIndex--;
                    list.Insert(newIndex, item);
                    break;

                default:
                    break;
            }
        }
        public static void Move<T>(this IList<T> list, int originalIndex, int destinationIndex)
        {
            var item = list[originalIndex];
            list.RemoveAt(originalIndex);

            if (destinationIndex > originalIndex) destinationIndex--;
            list.Insert(destinationIndex, item);
        }

        ///<summary>Finds the index of the first item matching an expression in an enumerable.</summary>
        ///<param name="items">The enumerable to search.</param>
        ///<param name="predicate">The expression to test the items against.</param>
        ///<returns>The index of the first matching item, or -1 if no items match.</returns>
        public static int FindIndex<T>(this IEnumerable<T> items, Func<T, bool> predicate)
        {
            if (items == null) throw new ArgumentNullException("items");
            if (predicate == null) throw new ArgumentNullException("predicate");

            var retVal = 0;
            foreach (var item in items)
            {
                if (predicate(item)) return retVal;
                retVal++;
            }
            return -1;
        }

        ///<summary>Finds the index of the first occurence of an item in an enumerable.</summary>
        ///<param name="items">The enumerable to search.</param>
        ///<param name="item">The item to find.</param>
        ///<returns>The index of the first matching item, or -1 if the item was not found.</returns>
        public static int IndexOf<T>(this IEnumerable<T> items, T item) { return items.FindIndex(i => EqualityComparer<T>.Default.Equals(item, i)); }
    }
}
