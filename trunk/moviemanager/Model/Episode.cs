namespace Model
{
    public class Episode : Video
    {
        public override VideoTypeEnum VideoType
        {
            get { return VideoTypeEnum.Episode; }
        }

        public int EpisodeNumber { get; set; }

        public long Runtime { get; set; }

        public int Season { get; set; }

        public int SerieId { get; set; }
    }
}
