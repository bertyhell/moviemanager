using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tmc.SystemFrameworks.Model
{
    public class ImageInfo
    {
        [Key]
        public int Id { get; set; }

        public String UriString
        {
            get { return Uri.AbsoluteUri; }
            set {Uri = new Uri(value) ; }
        }
        [NotMapped]
        public Uri Uri { get; set; }
        [NotMapped]
        public string Tag { get; set; }
        [NotMapped]
        public string Name { get; set; }
        [NotMapped]
        public string Description { get; set; }
        [NotMapped]
        public Type Type { get; set; }

        //public virtual Video Video { get; set; }
    }
}