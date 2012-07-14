namespace Model
{
    public class Movie : Video
    {
        public override VideoTypeEnum VideoType
        {
            get { return VideoTypeEnum.Movie; }
        }

        private int _franchiseID;
        public int FranchiseID
        {
            get { return _franchiseID; }
            set
            {
                _franchiseID = value;
                OnPropertyChanged("FranchiseID");
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
    }
}
