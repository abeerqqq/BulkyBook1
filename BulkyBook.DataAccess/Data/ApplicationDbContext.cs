﻿
using Microsoft.EntityFrameworkCore;
using BulkyBook.Models;
using BulkyBook.Models.Models;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace BulkyBook.DataAccess
{

    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Category> categories { get; set; }
        public DbSet<CoverType> coverTypes { get; set; }

        public DbSet<Product> products { get; set; }
        public DbSet<ApplicationUser> applicationUsers { get; set; }
        public DbSet<Company> company { get; set; }
        public DbSet<ShoppingCart> shoppingCart { get; set; }
    }
}
