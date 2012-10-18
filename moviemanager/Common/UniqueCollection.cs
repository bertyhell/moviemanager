using System.Collections.Generic;

namespace Common
{

    /// <summary>

    /// Unique Collection
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class UniqueCollection<T> : List<T>
    {
        /// <summary>
        /// Method that adds a new item if item is unique based on specified condition
        /// </summary>
        /// <param name="item"></param>
        /// <returns>Returns true if item is added to the collection</returns>
        public new void Add(T item)
        {
            if(!Contains(item)) base.Add(item);
        }

        public new void AddRange(IEnumerable<T> items)
        {
            foreach (T Item in items)
            {
                Add(Item);
            }
        }
    }
}
