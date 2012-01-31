namespace Model
{
    public class Movie : Video
    {
        public override VideoTypeEnum VideoType
        {
            get { return VideoTypeEnum.Movie; }
        }

        public int FranchiseID { get; set; }

        public long Runtime { get; set; }

        public int IdTmdb { get; set; }
    }
}
