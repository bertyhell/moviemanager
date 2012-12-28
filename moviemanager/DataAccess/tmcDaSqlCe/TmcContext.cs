using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Model;

namespace Tmc.DataAccess.SqlCe
{
    public class TmcContext : DbContext
    {
        public TmcContext()
            : base("name=TmcContext")
        {
        }
        
        public DbSet<Episode> Episodes { get; set; }
        public DbSet<Franchise> Franchises { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Serie> Series { get; set; }
        public DbSet<Video> Videos { get; set; }
    }
}
