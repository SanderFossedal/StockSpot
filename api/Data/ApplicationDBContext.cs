using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    //IdentityDbContext is a class that is used to configure the identity system of the application
    //It is a bridge between the database and the model
    public class ApplicationDBContext : IdentityDbContext<AppUser>
    {
        //DbContextOptions is a class that is used to configure the DbContext
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
            
        }

        //DbSet is a class that represents a table in the database, so this DbSet represents the Stock table
        public DbSet<Stock> Stocks { get; set; }
        //this DbSet represents the Comment table
        public DbSet<Comment> Comments { get; set; }

        public DbSet<Portfolio> Portfolios { get; set; }




        /// This seeding process is done to ensure that essential roles such as 'Admin' and 'User' 
        /// are automatically created in the database when it is initialized or updated. 
        /// By predefining these roles:
        /// 1. The application has a consistent set of roles available for assigning permissions and authorizations.
        /// 2. Developers and administrators can immediately use these roles without needing to manually add them each time.
        /// 3. It provides a clear and predefined structure for role-based access control (RBAC) within the application, enhancing security and simplifying management.
        /// Seeding roles like this is a common practice in applications that utilize identity and role management to enforce security and access controls.

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Portfolio>(x => x.HasKey(p => new { p.AppUserId, p.StockId }));

            builder.Entity<Portfolio>()
                .HasOne(u => u.AppUser)
                .WithMany(u => u.Portfolios)
                .HasForeignKey(p => p.AppUserId);

            builder.Entity<Portfolio>()
                .HasOne(u => u.Stock)
                .WithMany(u => u.Portfolios)
                .HasForeignKey(p => p.StockId);
            

            List<IdentityRole> roles = new List<IdentityRole> {
                new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Name = "User", NormalizedName = "USER" }
            };
            
            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}