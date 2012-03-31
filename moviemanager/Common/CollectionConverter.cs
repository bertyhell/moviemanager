using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Common
{
    public static class CollectionConverter<T>
    {
        public static List<T> ConvertObservableCollection(ObservableCollection<T> observableCollection)
        {
            List<T> MyList = new List<T>();
            foreach (T S in observableCollection)
            {
                    MyList.Add(S);
            }
            return MyList;
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
