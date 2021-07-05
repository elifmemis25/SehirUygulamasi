using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SehirUygulamasi.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SehirUygulamasi.Data
{
    public class ApplicationDbContext : IdentityDbContext<CetUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }
            public DbSet<Category> Categories { get; set; }
        public DbSet<GezilecekSehirler> GezilecekSehirlers { get; set; }
        public DbSet<CetImage> CetImages { get; set; }
    }
    }

