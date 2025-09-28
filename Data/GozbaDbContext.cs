using gozba_na_klik.Enums;
using gozba_na_klik.Model;
using Microsoft.EntityFrameworkCore;

namespace gozba_na_klik.Data
{
    public class GozbaDbContext : DbContext
    {
        public GozbaDbContext(DbContextOptions<GozbaDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Allergen> Allergens { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<WorkTime> WorkTimes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
           .HasIndex(u => u.Username)
           .IsUnique();

            modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();


            //potencijalno treba povezati i restoran sa worktime
            modelBuilder.Entity<Restaurant>()
                .HasOne(o => o.Owner)
                .WithMany(u => u.Restaurants)
                .HasForeignKey(r => r.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<WorkTime>()
                .HasOne(u => u.User)
                .WithMany(w => w.WorkTimes)
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            //potencijalno treba povezati adresu sa restoranom
            modelBuilder.Entity<Address>()
                .HasOne(u => u.User)
                .WithMany(a => a.Addresses)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserAllergen>()
                   .HasOne(ua => ua.User)
                   .WithMany(u => u.UserAllergens)
                   .HasForeignKey(ua => ua.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            #region SEED DATA
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    FirstName = "Admin",
                    LastName = "One",
                    Username = "Admin1",
                    Email = "admin1@gozba.com",
                    Password = "admin123",
                    Role = Role.Admin
                },
                new User
                {
                    Id = 2,
                    FirstName = "Admin",
                    LastName = "Two",
                    Username = "Admin2",
                    Email = "admin2@gozba.com",
                    Password = "admin123",
                    Role = Role.Admin
                },
                new User
                {
                    Id = 3,
                    FirstName = "Admin",
                    LastName = "Three",
                    Username = "Admin3",
                    Email = "admin3@gozba.com",
                    Password = "admin123",
                    Role = Role.Admin
                });

            modelBuilder.Entity<Allergen>().HasData(
                new Allergen { Id = 1, Name = "Gluten" },
                new Allergen { Id = 2, Name = "Peanuts" },
                new Allergen { Id = 3, Name = "Lactose" },
                new Allergen { Id = 4, Name = "Eggs" },
                new Allergen { Id = 5, Name = "Soy" },
                new Allergen { Id = 6, Name = "Nuts" },
                new Allergen { Id = 7, Name = "Fish" },
                new Allergen { Id = 8, Name = "Shellfish" }
            );
            #endregion
        }
    }
}
