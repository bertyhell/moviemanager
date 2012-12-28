using System;
using System.Collections.Generic;

namespace Model.Interfaces
{
    public interface IPreviewInfoRetriever
    {
        int Id { get; set; }
        List<ImageInfo> Images { get; set; }
        ImageInfo Thumbnail { get; set; }
        ImageInfo Poster { get; set; }
        DateTime Year { get; set; }
        string Name { get; set; }
    }
}
