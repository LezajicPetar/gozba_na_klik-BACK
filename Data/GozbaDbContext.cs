﻿using gozba_na_klik.Enums;
using gozba_na_klik.Model.Entities;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User: unique email + role kao string + hash lozinke

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
                e.Property(u => u.IsSuspended).HasDefaultValue(false).IsRequired();
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


            modelBuilder.Entity<WorkTime>(e =>
            {
                e.HasKey(w => w.Id);

                e.Property(w => w.DayOfWeek).IsRequired();

                e.Property(w => w.Start).HasColumnType("time without time zone");
                e.Property(w => w.End).HasColumnType("time without time zone");

                e.HasOne(w => w.User)
                    .WithMany(u => u.WorkTimes)
                    .HasForeignKey(w => w.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasIndex(w => new { w.UserId, w.DayOfWeek }).IsUnique();

                // Postgres sintaksa (bez []):
                e.ToTable("WorkTimes", tb =>
                    tb.HasCheckConstraint("CK_WorkTimes_DayOfWeek_Range", "\"DayOfWeek\" >= 0 AND \"DayOfWeek\" <= 6"));
            });

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
                    PasswordHash = "$2a$12$97Po1ExL9B3PTNSyDYBlmetfcdxQNuLWdRQ06l.A8eC0pJ9s6Zee2",
                    Role = Role.Admin
                },
                new User
                {
                    Id = 2,
                    FirstName = "Admin",
                    LastName = "Two",
                    Username = "Admin2",
                    Email = "admin2@gozba.com",
                    PasswordHash = "$2a$12$97Po1ExL9B3PTNSyDYBlmetfcdxQNuLWdRQ06l.A8eC0pJ9s6Zee2",
                    Role = Role.Admin
                },
                new User
                {
                    Id = 3,
                    FirstName = "Admin",
                    LastName = "Three",
                    Username = "Admin3",
                    Email = "admin3@gozba.com",
                    PasswordHash = "$2a$12$97Po1ExL9B3PTNSyDYBlmetfcdxQNuLWdRQ06l.A8eC0pJ9s6Zee2",
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
