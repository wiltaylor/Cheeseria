using System.IO;
using Cheeseria.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Cheeseria.Data
{
    /// <summary>
    /// Cheese Context
    /// </summary>
    public class CheeseContext: DbContext
    {
        /// <summary>
        /// Used by unit tests to prevent test data from being created.
        /// </summary>
        public static bool SkipSeedData = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public CheeseContext(DbContextOptions options) : base(options)
        {
        }

        /// <summary>
        /// Seed data for database. Will populate with test data by default.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (SkipSeedData)
                return;

            modelBuilder.Entity<Cheese>().HasData(new Cheese
            {
                Id = 1,
                Colour = "cream",
                Name = "Brie",
                Published = true,
                Image = File.ReadAllBytes("Assets/brie.jpg"),
                CostInCentsPerKg = 1000
            });

            modelBuilder.Entity<Cheese>().HasData(new Cheese
            {
                Id = 2,
                Colour = "white",
                Name = "Buffalo mozzarella",
                Published = true,
                Image = File.ReadAllBytes("Assets/buffalo.jpg"),
                CostInCentsPerKg = 2000
            });

            modelBuilder.Entity<Cheese>().HasData(new Cheese
            {
                Id = 3,
                Colour = "pale yellow",
                Name = "Cheddar",
                Published = true,
                Image = File.ReadAllBytes("Assets/cheddar.jpg"),
                CostInCentsPerKg = 1050
            });

            modelBuilder.Entity<Cheese>().HasData(new Cheese
            {
                Id = 4,
                Colour = "white",
                Name = "Halloumi",
                Published = true,
                Image = File.ReadAllBytes("Assets/halloumi.jpg"),
                CostInCentsPerKg = 4000
            });

            modelBuilder.Entity<Cheese>().HasData(new Cheese
            {
                Id = 5,
                Colour = "blue",
                Name = "Stilton",
                Published = true,
                Image = File.ReadAllBytes("Assets/stilton.jpg"),
                CostInCentsPerKg = 500
            });
        }


        public DbSet<Cheese> Cheeses { get; set; }
        
    }
}
