
using Microsoft.EntityFrameworkCore;
using Sereno.Domain.Entities;

namespace Sereno.Infrastructure.Data;

public class SerenoDbContext : DbContext
{
    public SerenoDbContext(
        DbContextOptions<SerenoDbContext> options)
        : base(options)
    {
    }

    // ==================================================
    // TABELAS DO BANCO DE DADOS
    // ==================================================

    public DbSet<Totem> Totens => Set<Totem>();

    public DbSet<LeituraSensor> Leituras =>
        Set<LeituraSensor>();

    public DbSet<PontoRefresco> PontosRefresco =>
        Set<PontoRefresco>();

    public DbSet<Alerta> Alertas =>
        Set<Alerta>();

    public DbSet<PrevisaoCalor> PrevisoesCalor =>
        Set<PrevisaoCalor>();

    // ==================================================
    // MAPEAMENTO DAS ENTIDADES
    // ==================================================

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ==============================================
        // TOTEM
        // ==============================================

        modelBuilder.Entity<Totem>(entity =>
        {
            entity.HasKey(t => t.Id);

            entity.Property(t => t.Codigo)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasIndex(t => t.Codigo)
                .IsUnique();

            entity.HasOne(t => t.PontoRefresco)
                .WithMany(p => p.Totens)
                .HasForeignKey(t => t.PontoRefrescoId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // ==============================================
        // LEITURA DO SENSOR
        // ==============================================

        modelBuilder.Entity<LeituraSensor>(entity =>
        {
            entity.HasKey(l => l.Id);

            entity.HasOne(l => l.Totem)
                .WithMany(t => t.Leituras)
                .HasForeignKey(l => l.TotemId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(l => l.Nivel)
                .HasConversion<string>()
                .HasMaxLength(20);

            entity.HasIndex(l => l.ColetadaEm);
        });

        // ==============================================
        // PONTO DE REFRESCO
        // ==============================================

        modelBuilder.Entity<PontoRefresco>(entity =>
        {
            entity.HasKey(p => p.Id);

            entity.Property(p => p.Nome)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(p => p.Endereco)
                .IsRequired()
                .HasMaxLength(250);
        });

        // ==============================================
        // ALERTA
        // ==============================================

        modelBuilder.Entity<Alerta>(entity =>
        {
            entity.HasKey(a => a.Id);

            entity.Property(a => a.TotemCodigo)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(a => a.Origem)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(a => a.Nivel)
                .HasConversion<string>()
                .HasMaxLength(20);

            entity.Property(a => a.Mensagem)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(a => a.Ativo)
                .IsRequired();

            entity.HasOne<LeituraSensor>()
                .WithMany()
                .HasForeignKey(a => a.LeituraSensorId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(a => a.LeituraSensorId)
                .IsUnique();

            entity.HasIndex(a => new
            {
                a.Ativo,
                a.CriadoEm
            });

            entity.HasIndex(a => a.TotemCodigo);
        });

        // ==============================================
        // PREVISAO DE CALOR
        // ==============================================

        modelBuilder.Entity<PrevisaoCalor>(entity =>
        {
            entity.HasKey(p => p.Id);

            entity.Property(p => p.DataReferencia)
                .IsRequired()
                .HasColumnType("date");

            entity.HasIndex(p => p.DataReferencia)
                .IsUnique();

            entity.Property(p => p.Probabilidade)
                .IsRequired();

            entity.ToTable(t =>
                t.HasCheckConstraint(
                    "CK_PrevisoesCalor_Probabilidade",
                    "\"Probabilidade\" >= 0 AND " +
                    "\"Probabilidade\" <= 1"));

            entity.Property(p => p.Nivel)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(p => p.VersaoModelo)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(p => p.Mensagem)
                .HasMaxLength(500)
                .IsRequired();
        });
    }
}
