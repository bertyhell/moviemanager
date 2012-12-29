using System;
using System.ComponentModel.DataAnnotations;

namespace Tmc.SystemFrameworks.Model
{
    public class Subtitle
    {
        [Key]
        public int Id { get; set; }
        public String Path { get; set; }
    }
}
