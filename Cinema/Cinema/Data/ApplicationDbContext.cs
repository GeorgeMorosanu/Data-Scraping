using System;
using System.Collections.Generic;
using System.Text;
using Cinema.Areas.Identity.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        private DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}
