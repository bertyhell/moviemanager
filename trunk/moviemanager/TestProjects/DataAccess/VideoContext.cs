using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using DataAccess.testmodels;

namespace DataAccess
{
    //public class SimpleVideo
    //{
    //    public string Name { get; set; }
    //    public int SimpleVideoId { get; set; }
    //    public virtual List<Sub> Subs { get; set; }
    //    public Sub MainSub { get; set; }
    //}

    //public class Sub
    //{
    //    public int SubId { get; set; }
    //    public string Language { get; set; }
    //}

    class VideoContext : DbContext
    {
        public DbSet<SimpleVideo> SimpleVideos { get; set; }
        public DbSet<Sub> Subs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            Database.SetInitializer(new DropCreateDatabaseAlways<VideoContext>());

            modelBuilder.Entity<SimpleVideo>().HasMany(v => v.Subs).WithOptional().WillCascadeOnDelete(true);
            modelBuilder.Entity<SimpleVideo>().HasOptional(v => v.MainSub).WithRequired().WillCascadeOnDelete(true);

            //modelBuilder.Entity<AdapterFrameCapability>().HasRequired(afc => afc.FromPressType).WithMany().HasForeignKey(afc => afc.FromPressTypeID).WillCascadeOnDelete(true);
            //modelBuilder.Entity<AdapterFrameCapability>().HasRequired(afc => afc.ToPressType).WithMany().HasForeignKey(afc => afc.ToPressTypeID).WillCascadeOnDelete(false);
            
            //modelBuilder.Entity<SimpleVideo>().HasMany(v => v.Subs).WithOptional().WillCascadeOnDelete(true);
            //modelBuilder.Entity<SimpleVideo>().HasOptional(v => v.MainSub).WithOptionalPrincipal().WillCascadeOnDelete(true);

            base.OnModelCreating(modelBuilder);
        }

        public VideoContext()
        {
            Database.DefaultConnectionFactory = new SqlCeConnectionFactory("System.Data.SqlServerCe.4.0");
        }
    }
}
