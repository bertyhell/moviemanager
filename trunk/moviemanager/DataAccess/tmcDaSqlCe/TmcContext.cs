using System;
using System.Collections.Generic;
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
            
            Database.SetInitializer(new CreateDatabaseIfNotExists<TmcContext>());
            Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EpisodeInfo>().ToTable("Episodes");
            modelBuilder.Entity<MovieInfo>().ToTable("Movies");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Video>().HasOptional(v => v.MovieInfo).WithRequired(m => m.Video).WillCascadeOnDelete(true);
            modelBuilder.Entity<Video>().HasOptional(v => v.EpisodeInfo).WithRequired(e => e.Video).WillCascadeOnDelete(true);
            modelBuilder.Entity<Video>().HasMany(v => v.Files).WithRequired(f => f.Video).WillCascadeOnDelete(true);
            ////...
            //modelBuilder.Entity<Parent>().HasMany(e => e.ParentDetails).WithOptional(s => s.Parent).WillCascadeOnDelete(true);
        }
        
        public DbSet<EpisodeInfo> Episodes { get; set; }
        public DbSet<Franchise> Franchises { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Serie> Series { get; set; }
    }
}
