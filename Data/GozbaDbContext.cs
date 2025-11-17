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
        public DbSet<UserAllergen> UserAllergens { get; set; }
        public DbSet<RestaurantWorkTime> RestaurantWorkTimes { get; set; }
        public DbSet<RestaurantExceptionDate> RestaurantExceptionDates { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region PORUDZBINA
            modelBuilder.Entity<Order>()
                .Property(o => o.Subtotal)
                .HasColumnType("numeric(12,2)");

            modelBuilder.Entity<Order>()
                .Property(o => o.Total)
                .HasColumnType("numeric(12,2)");

            modelBuilder.Entity<Order>()
                .Property(o => o.DeliveryFee)
                .HasColumnType("numeric(12,2)");

            modelBuilder.Entity<OrderItem>()
                .Property(i => i.Price)
                .HasColumnType("numeric(12,2)");

            modelBuilder.Entity<Order>()
                .Property(o => o.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Order>()
               .HasOne(o => o.Restaurant)
               .WithMany()
               .HasForeignKey(o => o.RestaurantId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany()
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Courier)
                .WithMany()
                .HasForeignKey(o => o.CourierId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Address)
                .WithMany()
                .HasForeignKey(o => o.AddressId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.Items)
                .WithOne(i => i.Order)
                .HasForeignKey(i => i.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region REVIEW
            modelBuilder.Entity<Review>()
                .HasOne(rv => rv.Restaurant)
                .WithMany(r => r.Reviews)
                .HasForeignKey(rv => rv.RestaurantId)
                .OnDelete(DeleteBehavior.Cascade);  

            modelBuilder.Entity<Review>()
                .HasOne(rv => rv.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(rv => rv.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            modelBuilder.Entity<MenuItem>()
                .HasOne(mi => mi.Restaurant)
                .WithMany(r => r.Menu)
                .HasForeignKey(mi => mi.RestaurantId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<User>(e =>
            {
                e.Property(u => u.FirstName).HasMaxLength(35).IsRequired();
                e.Property(u => u.LastName).HasMaxLength(35).IsRequired();

                e.Property(u => u.Email).HasMaxLength(50).IsRequired();
                e.HasIndex(u => u.Email).IsUnique();

                e.Property(u => u.Role)
                    .HasConversion<string>()
                    .HasMaxLength(32)
                    .IsRequired();

                e.Property(u => u.PasswordHash).IsRequired();
            });

            modelBuilder.Entity<UserAllergen>()
                .HasKey(ua => new { ua.UserId, ua.AllergenId });

            //potencijalno treba povezati i restoran sa worktime
            modelBuilder.Entity<Restaurant>()
                .HasOne(o => o.Owner)
                .WithMany(u => u.Restaurants)
                .HasForeignKey(r => r.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Restaurant>(e =>
            {
                e.Property(r => r.Name).HasMaxLength(120).IsRequired();
                e.Property(r => r.Description).HasMaxLength(4000);
                e.Property(r => r.Phone).HasMaxLength(32);
                e.Property(r => r.Photo).HasMaxLength(512);
            });
            // radno vreme i neradni dani
            modelBuilder.Entity<RestaurantWorkTime>()
                .HasOne(w => w.Restaurant)
                .WithMany(r => r.WorkTimes)
                .HasForeignKey(w => w.RestaurantId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RestaurantExceptionDate>()
                .HasOne(e => e.Restaurant)
                .WithMany(r => r.ExceptionDates)
                .HasForeignKey(e => e.RestaurantId)
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
                .HasKey(ua => new { ua.UserId, ua.AllergenId });

            modelBuilder.Entity<UserAllergen>()
                .HasOne(ua => ua.User)
                .WithMany(u => u.UserAllergens)
                .HasForeignKey(ua => ua.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserAllergen>()
                .HasOne(ua => ua.Allergen)
                .WithMany(a => a.UserAllergens)
                .HasForeignKey(ua => ua.AllergenId)
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
                    PasswordHash = "$2a$11$VdTkF.NE1aw8uZmfFO51OuxlW9qrvbx7W8g3iKw6aHcuC1vHfMJt6\r\n",
                    Role = Role.Admin
                },
                new User
                {
                    Id = 2,
                    FirstName = "Admin",
                    LastName = "Two",
                    Username = "Admin2",
                    Email = "admin2@gozba.com",
                    PasswordHash = "$2a$11$VdTkF.NE1aw8uZmfFO51OuxlW9qrvbx7W8g3iKw6aHcuC1vHfMJt6\r\n",
                    Role = Role.Admin
                },
                new User
                {
                    Id = 3,
                    FirstName = "Admin",
                    LastName = "Three",
                    Username = "Admin3",
                    Email = "admin3@gozba.com",
                    PasswordHash = "$2a$11$VdTkF.NE1aw8uZmfFO51OuxlW9qrvbx7W8g3iKw6aHcuC1vHfMJt6\r\n",
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
