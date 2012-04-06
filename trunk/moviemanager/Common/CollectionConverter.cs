﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Common
{
    public static class CollectionConverter<T>
    {
        public static List<T> ConvertObservableCollection(ObservableCollection<T> observableCollection)
        {
            return observableCollection.ToList();
        }

        public static ObservableCollection<T> ConvertList(List<T> list)
        {
            ObservableCollection<T> Collection = new ObservableCollection<T>();
            foreach (T S in list)
            {
                Collection.Add(S);
            }
            return Collection;
        } 
    }
}
