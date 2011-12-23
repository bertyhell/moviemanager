using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class Movie : Video
    {
        private long _runtime;
        private int _franchise_id;

        public Movie() { }

        public override VideoTypeEnum VideoType
        {
            get { return VideoTypeEnum.Movie; }
        }

        public int Franchise_id
        {
            get { return _franchise_id; }
            set { _franchise_id = value; }
        }

        public long Runtime
        {
            get { return _runtime; }
            set { this._runtime = value; }
        }
    }
}
