using System;
using System.Collections.Generic;

namespace MovieManager.Common
{
    public class ItemsProvider<T> : IItemsProvider<T>
    {
        private readonly IList<T> _items;

        public ItemsProvider(IList<T> items)
        {
            _items = items;
        }

        public int FetchCount()
        {
            return _items.Count;
        }

        public IList<T> FetchRange(int startIndex, int count)
        {
            var RequestedItems = new List<T>();
            for (int I = startIndex; I < Math.Min(startIndex + count, FetchCount()); I++)
            {
                RequestedItems.Add(_items[I]);
            }
            return RequestedItems;
        }

        //public int FetchCount()
        //{
        //    return 1000;
        //}

        //public IList<object> FetchRange(int startIndex, int count)
        //{
        //    var RequestedItems = new List<object>();
        //    for (int I = startIndex; I < Math.Min(startIndex + count, FetchCount()); I++)
        //    {
        //        RequestedItems.Add(new { name = "test", id = I });
        //    }
        //    return RequestedItems;
        //}
    }
}
