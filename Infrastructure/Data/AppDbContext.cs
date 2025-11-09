using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Tablolar
        public DbSet<Site> Sites { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Site>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.SiteName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.LoginUrl).IsRequired();
                entity.Property(e => e.StockUrl).IsRequired();
                entity.Property(e => e.UserName).HasMaxLength(200);
                entity.Property(e => e.Password).HasMaxLength(500);
                entity.Property(e => e.ExtraParamJson).HasColumnType("TEXT");
                entity.Property(e => e.ParserType).HasMaxLength(100);
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                // SQLite'da CURRENT_TIMESTAMP kullanılır
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
        }
    }
}
