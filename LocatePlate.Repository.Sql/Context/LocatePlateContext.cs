using LocatePlate.Model.Location;
using LocatePlate.Model.RestaurantDomain;
using LocatePlate.Model.Search;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LocatePlate.Repository.Context
{
    public class LocatePlateContext : IdentityDbContext<UserIdentity>
    {
        public LocatePlateContext(DbContextOptions<LocatePlateContext> options) : base(options)
        {
        }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Capacity> Capacities { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<MenuCategory> MenuCategories { get; set; }
        //public DbSet<Promotion> Promotions { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Timing> Timings { get; set; }
        public DbSet<CanadaCity> CanadaCities { get; set; }
        public DbSet<UserIdentity> UserIdentities { get; set; }
        public DbSet<BookingXCapacity> BookingXcapacities { get; set; }
        public DbSet<BookingXMenu> BookingXMenus { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<SearchResult> SearchRecords { get; set; }
        public DbSet<StringModel> StringRecords { get; set; }
        public DbSet<Ratings> Ratings { get; set; }
        public DbSet<Reviews> Reviews { get; set; }
        public DbSet<RestaurantStoreProcedure> RestaurantStoreProcedure { get; set; }
        public DbSet<CurrentLocation> CurrentLocation { get; set; }
        public DbSet<MenuReservation> MenuReservation { get; set; }
        public DbSet<BookingOrderNumber> BookingOrderNumber { get; set; }
        public DbSet<ContactUsModel> ContactUs { get; set; }
        public DbSet<PerformanceGraph> performanceGraphs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Booking>();
            modelBuilder.Entity<Capacity>();
            modelBuilder.Entity<Menu>();
            modelBuilder.Entity<MenuCategory>();
            //modelBuilder.Entity<Promotion>();
            modelBuilder.Entity<Restaurant>();
            modelBuilder.Entity<Timing>();
            modelBuilder.Entity<CanadaCity>();
            modelBuilder.Entity<UserIdentity>();
            modelBuilder.Entity<BookingXCapacity>();
            modelBuilder.Entity<BookingXMenu>();
            modelBuilder.Entity<Ratings>();
            modelBuilder.Entity<Reviews>();
            modelBuilder.Entity<MenuReservation>();
            modelBuilder.Entity<BookingOrderNumber>();


            ///  modelBuilder.Entity<SearchRecords>();
            //new UserMap(modelBuilder.Entity<User>());
            //new UserProfileMap(modelBuilder.Entity<UserProfile>());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            // optionsBuilder.UseSqlServer();
        }
    }
}