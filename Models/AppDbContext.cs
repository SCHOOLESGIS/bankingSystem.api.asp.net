using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using Microsoft.EntityFrameworkCore;
using System.Data.Entity;
using WebApplication2.Models;

namespace WebApplication2.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("name=DefaultConnection") { }
        public DbSet<Client> Clients { get; set; } 
        public DbSet<Compte> Comptes { get; set; }
        public DbSet<Livret> Livrets { get; set; }
        public DbSet<PEL> PELs { get; set; }

        // protected override void OnConfiguring (DbContextOptionsBuilder OptionBuilder)
        //{
        //    OptionBuilder.UseSqlServer("Server=.;Database=banking_system;Trusted_Connection=True;");
        //}
    }
}