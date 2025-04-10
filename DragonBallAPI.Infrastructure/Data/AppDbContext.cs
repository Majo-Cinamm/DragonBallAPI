using DragonBallAPI.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DragonBallAPI.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Character> Characters { get; set; }
        public DbSet<Transformation> Transformations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de entidad Character
            modelBuilder.Entity<Character>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(25).IsRequired();
                entity.Property(e => e.Ki).HasMaxLength(35);
                entity.Property(e => e.Race).HasMaxLength(25);
                entity.Property(e => e.Gender).HasMaxLength(20);
                entity.Property(e => e.Description).HasColumnType("varchar(max)");
                entity.Property(e => e.Affiliation).HasMaxLength(35);
            });

            // Configuración de entidad Transformation
            modelBuilder.Entity<Transformation>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(25).IsRequired();
                entity.Property(e => e.Ki).HasMaxLength(35);

                // Relación con Character
                entity.HasOne(e => e.Character)
                      .WithMany(c => c.Transformations)
                      .HasForeignKey(e => e.CharacterId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
