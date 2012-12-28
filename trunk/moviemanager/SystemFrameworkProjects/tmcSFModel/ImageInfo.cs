using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
    public class ImageInfo
    {
        [Key]
        public int Id { get; set; }
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
    }
}