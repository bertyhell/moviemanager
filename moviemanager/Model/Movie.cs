namespace Model
{
    public class Movie : Video
    {
        //TODO 100: implement inotifypropertychanged
        public override VideoTypeEnum VideoType
        {
            get { return VideoTypeEnum.Movie; }
        }

        public int FranchiseID { get; set; }


        public int IdTmdb { get; set; }
    }
}
