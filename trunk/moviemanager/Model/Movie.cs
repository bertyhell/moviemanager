using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class Movie : Video
    {
        private long _runtime;
        private int _franchiseID;

        public Movie() { }

        public override VideoTypeEnum VideoType
        {
            get { return VideoTypeEnum.Movie; }
        }

        public int Franchise_id
        {
            get { return _franchiseID; }
            set { _franchiseID = value; }
        }

        public long Runtime
        {
            get { return _runtime; }
            set { this._runtime = value; }
        }
    }
}
