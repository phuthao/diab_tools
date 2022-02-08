using System.ComponentModel.DataAnnotations.Schema;
using DiaB.IdentityServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DiaB.IdentityServer.Database
{
    public class DiabDbContext : DbContext
    {
        public DbSet<DiabSetting> DiabSettings { get; set; }

        public DiabDbContext(DbContextOptions<DiabDbContext> options) : base(options)
        {
        }
    }
}