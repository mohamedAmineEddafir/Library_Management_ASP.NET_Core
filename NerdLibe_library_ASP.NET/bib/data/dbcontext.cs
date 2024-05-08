using bib.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace bib.data
{
    public class Dbcontext : DbContext
    {
        public Dbcontext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Livre> Livre { get; set; }
        public DbSet<Evente> Evente { get; set; }
        public DbSet<Demande> Demande { get; set; }  
        public DbSet<Utilisateur> Utilisateur { get; set; }
        public DbSet<Emprunt> Emprunt { get; set;}

    }
}
