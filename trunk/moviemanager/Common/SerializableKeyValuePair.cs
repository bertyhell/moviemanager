using System;
using System.Xml.Serialization;

namespace MovieManager.Common
{
    [Serializable]
    [XmlType(TypeName = "Pair")]
    public class Pair<TK, TV>
    {
        public Pair(){}

        public Pair(TK key, TV value)
        {
            Key = key;
            Value = value;
        }

        public TK Key
        { get; set; }

        public TV Value
        { get; set; }
    }
}
