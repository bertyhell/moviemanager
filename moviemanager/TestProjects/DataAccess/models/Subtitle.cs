using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.models
{
    public class Subtitle
    {
        [Key]
        public int Id { get; set; }
        public String Path { get; set; }
    }
}
