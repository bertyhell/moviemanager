using System;
using System.Collections.Generic;

namespace Model
{
    public class Movie : Video, IEquatable<Video>, IEqualityComparer<Video>
    {
        public override VideoTypeEnum VideoType
        {
            get { return VideoTypeEnum.Movie; }
        }

        private int _franchiseId;
        public int FranchiseId
        {
            get { return _franchiseId; }
            set
            {
                _franchiseId = value;
                OnPropertyChanged("FranchiseId");
            }
        }

        private int _idTmdb;
        public int IdTmdb
        {
            get { return _idTmdb; }
            set
            {
                _idTmdb = value;
                OnPropertyChanged("IdTmdb");
            }
        }
        
        //equal videos if imdb IDs match or if names and release dates match
        public bool Equals(Video other)
        {
            return IdImdb == other.IdImdb || Name == other.Name && Release == other.Release;
        }

        public bool Equals(Video x, Video y)
        {
            return x.IdImdb == y.IdImdb || x.Name == y.Name && x.Release == y.Release;
        }

        public int GetHashCode(Video obj)
        {
            return (obj.IdImdb.GetHashCode() + obj.Name.GetHashCode() + obj.Release.GetHashCode()).GetHashCode();
        }
    }
}
