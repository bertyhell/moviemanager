using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    class Serie
    {
        private int _id;
        private String _name;

        public Serie() { }

        public int Id { get { return _id; } set { _id = value; } }
        public string Name { get { return _name; } set { _name = value; } }
    }
}
