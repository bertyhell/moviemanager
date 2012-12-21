using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.Interfaces
{
    public interface IThumbnailInfoRetriever
    {
        ImageInfo Thumbnail { get; set; }
        DateTime Year { get; set; }
        string Name { get; set; }
    }
}
