using System;
using System.Data;
using System.Threading.Tasks;
using HouseOfSoulSounds.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HouseOfSoulSounds.Models.Domain.Entities;
using Microsoft.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using HouseOfSoulSounds.Models.Domain.Repositories.EntityFramework;
using HouseOfSoulSounds.Models.Identity;


namespace HouseOfSoulSounds.Models.Domain
{
    public class EFAppDbContext : IdentityDbContext<User>
    {
        public EFAppDbContext(DbContextOptions<EFAppDbContext> options) : base(options) { }

        public DbSet<TextField> TextFields { get; set; }
        public DbSet<InstrumentItem> InstrumentsItems { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Catalog> Catalogs { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = "C3BD297D-2AEC-4582-B679-FDA3AA5164D3",
                Name = Config.RoleAdmin,
                NormalizedName = Config.RoleAdmin.ToUpper()
            });

            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = "DB25A8AF-4316-4FF6-BCB3-3A6CCE7CFE53",
                Name = Config.RoleModerator,
                NormalizedName = Config.RoleModerator.ToUpper()
            });

            modelBuilder.Entity<User>().HasData(new User
            {
                Id = "A8B0919E-FA64-4F08-89C5-A37B5F003C00",
                UserName = Config.Admin,
                NormalizedUserName = Config.Admin,
                Email = "admin@email.com",
                NormalizedEmail = "ADMIN@EMAIL.COM",
                EmailConfirmed = true,
                PasswordHash = new PasswordHasher<User>().HashPassword(null, "password"),
                SecurityStamp = string.Empty
            });

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = "C3BD297D-2AEC-4582-B679-FDA3AA5164D3",
                UserId = "A8B0919E-FA64-4F08-89C5-A37B5F003C00"
            });

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = "DB25A8AF-4316-4FF6-BCB3-3A6CCE7CFE53",
                UserId = "A8B0919E-FA64-4F08-89C5-A37B5F003C00"
            });

            modelBuilder.Entity<TextField>().HasData(new TextField { 
                Id = new Guid("A543BCFD-B9EE-4584-A729-54D639A29535"), 
                CodeWord = "HomePage", 
                Title = "Главная"
            });

            modelBuilder.Entity<TextField>().HasData(new TextField
            {
                Id = new Guid("1CBFB4CB-D7C9-4C36-8187-D1A411C643B7"), 
                CodeWord = "InstrumentsPage", 
                Title = "Каталоги"
            });

            modelBuilder.Entity<TextField>().HasData(new TextField
            {
                Id = new Guid("7698042D-A1DB-4190-BB09-CC8954954CED"), 
                CodeWord = "ContactsPage", 
                Title = "Контакты"
            });
        }
    }
}
