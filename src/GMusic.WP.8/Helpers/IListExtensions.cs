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
    }
}
