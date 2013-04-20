using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.testmodels
{
    public class SimpleVideo
    {
        public string Name { get; set; }
        public int SimpleVideoId { get; set; }
        public virtual List<Sub> Subs { get; set; }
        public Sub MainSub { get; set; }
    }

    public class Sub
    {
        public int SubId { get; set; }
        public string Language { get; set; }

        ////public int VideoId { get; set; }
        //public virtual SimpleVideo Video { get; set; }
    }
}
