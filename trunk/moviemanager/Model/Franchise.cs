using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    class Franchise
    {
        private int _id;
        private String _name;

        public Franchise()
        {
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }
    }
}
