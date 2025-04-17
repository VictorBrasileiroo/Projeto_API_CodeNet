using System;
using CodeNet.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeNet.Infrastructure.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<UserModel> Usuarios { get; set; }
        public DbSet<GrupoModel> Grupos { get; set; }
        public DbSet<GrupoMembroModel> GrupoMembros { get; set; }
        public DbSet<MensagemModel> Mensagens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<GrupoMembroModel>()
                .HasKey(gm => gm.Id);

            modelBuilder.Entity<GrupoMembroModel>()
                .HasOne(gm => gm.User)
                .WithMany(u => u.Grupos)
                .HasForeignKey(gm => gm.IdUser)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GrupoMembroModel>()
                .HasOne(gm => gm.Grupo)
                .WithMany(g => g.Membros)
                .HasForeignKey(gm => gm.IdGrupo)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MensagemModel>()
                .HasOne(m => m.User)
                .WithMany(u => u.Mensagens)
                .HasForeignKey(m => m.IdUser)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MensagemModel>()
                .HasOne(m => m.Grupo)
                .WithMany(g => g.Mensagens)
                .HasForeignKey(m => m.IdGrupo)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
