//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class Video
    {
        public Video()
        {
            this.Images = new HashSet<Image>();
        }
    
        public string name { get; set; }
        public long posterId { get; set; }
        public long id { get; set; }
    
        public virtual Image Poster { get; set; }
        public virtual ICollection<Image> Images { get; set; }
        public virtual Episode Episode { get; set; }
    }
}