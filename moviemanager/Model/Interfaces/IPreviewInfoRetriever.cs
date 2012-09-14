using System;

namespace Model.Interfaces
{
    public interface IPreviewInfoRetriever
    {
        ImageInfo Thumbnail { get; set; }
        Uri Poster { get; set; }
        DateTime Year { get; set; }
        string Name { get; set; }
    }
}
