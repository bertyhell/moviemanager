using System;

namespace Model
{
    public class ImageInfo
    {
        private Uri _uri;
        private string _tag;
        private string _name;
        private string _description;
        private Type _type;

        public Uri Uri { get { return _uri; } set { _uri = value; } }
        public string Tag { get { return _tag; } set { _tag = value; } }
        public string Name { get { return _name; } set { _name = value; } }
        public string Description { get { return _description; } set { _description = value; } }
        public Type Type { get { return _type; } set { _type = value; } }
    }
}