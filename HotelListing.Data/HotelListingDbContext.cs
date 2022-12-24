using HotelListing.API.Data.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace HotelListing.API.Data
{
    //if not using Identity then it wuld have been DbContext rather than identityDbContext;
    public class HotelListingDbContext : IdentityDbContext<ApiUser>
    {
        public HotelListingDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Country> Countries { get; set; }


        //we used all theese to seed

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new CountryConfiguration());
            modelBuilder.ApplyConfiguration(new HotelConfiguration());

            modelBuilder.Entity<Hotel>();
        }


        //ADD A NEW DB CONNECTION IF WE CANT SCAFFOLD

        //probably because our context is in another Project
        //We are doing this because we cant scaffold after seperating projects, by creating a Factory
        //public class HotelListingDbContextFactory : IDesignTimeDbContextFactory<HotelListingDbContext>
        //{
        //    public HotelListingDbContext CreateDbContext(string[] args)
        //    {
        //             IConfiguration config = new ConfigurationBuilder()
        //                .SetBasePath(Directory.GetCurrentDirectory())
        //                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        //                .Build();
                    
        //        var optionsBuilder =  new DbContextOptionsBuilder<HotelListingDbContext>();
        //        var conn = config.GetConnectionString("HotelListingDbConnectionString");
        //        optionsBuilder.UseSqlServer(conn);
        //        return new HotelListingDbContext(optionsBuilder.Options)
        //    }
        //}
    }
}
