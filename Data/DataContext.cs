using System.Runtime.Intrinsics.X86;
using Microsoft.EntityFrameworkCore;
using test.Models;

namespace test.Data
{
    public class DataContext : DbContext
    {
        //private readonly IConfiguration _configuration;
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Category> Categories {get; set;}
        public DbSet<Country> Countries {get; set;}
        public DbSet<Owner> Owners {get; set;}
        public DbSet<Pokemon> Pokemons {get; set;}
        public DbSet<PokemonCategory> PokemonCategories {get; set;}
        public DbSet<PokemonOwner> PokemonOwners {get; set;}
        public DbSet<Review> Reviews {get; set;}
        public DbSet<Reviewer> Reviewers {get; set;}

        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
               => optionsBuilder.UseSqlServer(_configuration["DefaultConnection"]);*/

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PokemonCategory>(entity =>
            {
            entity.HasKey(pc => new {pc.PokemonId, pc.CategoryId} );
           entity
                .HasOne(p => p.Pokemon)
                .WithMany(pc => pc.PokemonCategories)
                .HasForeignKey(p => p.PokemonId);
            entity
                .HasOne(p => p.Category)
                .WithMany(pc => pc.PokemonCategories)
                .HasForeignKey(c => c.CategoryId);
            });

             modelBuilder.Entity<PokemonOwner>(entity =>
            {
            entity
                .HasKey(po => new {po.PokemonId, po.OwnerId});
            entity
                .HasOne(p => p.Pokemon)
                .WithMany(po => po.PokemonOwners)
                .HasForeignKey(p => p.PokemonId);
            entity
                .HasOne(p => p.Owner)
                .WithMany(po => po.PokemonOwners)
                .HasForeignKey(o => o.OwnerId);
            });
        }

    }
}