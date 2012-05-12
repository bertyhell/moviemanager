namespace Model
{
    public class Episode : Video
    {
        //TODO 100: implement inotifypropertychanged
        public override VideoTypeEnum VideoType
        {
            get { return VideoTypeEnum.Episode; }
        }

        public int EpisodeNumber { get; set; }
        
        public int Season { get; set; }

        public int SerieId { get; set; }
    }
}
