using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tmc.SystemFrameworks.Model
{
    public class MovieInfo: INotifyPropertyChanged// : Video, IEquatable<Video>, IEqualityComparer<Video>
    {

        private int _franchiseId;
        private int _idTmdb;


        [Key]
        public int Id { get; set; }

        public int FranchiseId
        {
            get { return _franchiseId; }
            set
            {
                _franchiseId = value;
                OnPropertyChanged("FranchiseId");
            }
        }

        public int IdTmdb
        {
            get { return _idTmdb; }
            set
            {
                _idTmdb = value;
                OnPropertyChanged("IdTmdb");
            }
        }

        //public virtual Video Video { get; set; }
        
        //equal videos if imdb IDs match or if names and release dates match
        //public bool Equals(Video other)
        //{
        //    return IdImdb == other.IdImdb || Name == other.Name && Release == other.Release;
        //}

        //public bool Equals(Video x, Video y)
        //{
        //    return x.IdImdb == y.IdImdb || x.Name == y.Name && x.Release == y.Release;
        //}

        //public int GetHashCode(Video obj)
        //{
        //    return (obj.IdImdb.GetHashCode() + obj.Name.GetHashCode() + obj.Release.GetHashCode()).GetHashCode();
        //}

        protected void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
