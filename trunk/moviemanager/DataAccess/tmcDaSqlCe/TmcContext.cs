using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Tmc.SystemFrameworks.Model;

namespace Tmc.DataAccess.SqlCe
{
    public class TmcContext : DbContext
    {
        public TmcContext(string connectionString)
            : base(connectionString)
        {
            Database.DefaultConnectionFactory = new SqlCeConnectionFactory("System.Data.SqlServerCe.4.0");
#if DEBUG
			//if (Database.Exists() && !Database.CompatibleWithModel(true))
			//	Database.Delete();
			Database.SetInitializer(new DropCreateDatabaseIfModelChanges<TmcContext>());
#elif !DEBUG
            Database.SetInitializer(new CreateDatabaseIfNotExists<TmcContext>());
#endif
			Configuration.LazyLoadingEnabled = false;

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EpisodeInfo>().ToTable("Episodes");
            modelBuilder.Entity<MovieInfo>().ToTable("Movies");
            modelBuilder.Entity<ImageInfo>().ToTable("Images");

            modelBuilder.Entity<Video>().HasOptional(v => v.MovieInfo).WithRequired().WillCascadeOnDelete(true);
            modelBuilder.Entity<Video>().HasOptional(v => v.EpisodeInfo).WithRequired().WillCascadeOnDelete(true);
            modelBuilder.Entity<Video>().HasMany(v => v.Files).WithRequired().WillCascadeOnDelete(true);
			modelBuilder.Entity<Video>().HasMany(v => v.Images).WithRequired().WillCascadeOnDelete(true);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<EpisodeInfo> Episodes { get; set; }
        public DbSet<Franchise> Franchises { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Serie> Series { get; set; }
        public DbSet<Genre> Genres { get; set; }
    }
}
