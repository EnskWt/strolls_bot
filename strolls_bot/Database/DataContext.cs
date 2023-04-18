using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace strolls_bot.Database;

public partial class DataContext : DbContext
{
    public DataContext()
    {
    }

    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TrackLocation> TrackLocations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=DESKTOP-IC2JQNM;Database=Track;Trusted_Connection=True;Encrypt=false;"); // EF связан с базой данных MS SQL. Данный класс был сгенерирован автоматически инструментом EntityFramework.Tools, по этому связь происходит именно с этой локальной БД. Для работы бота нужно поменять эту строку, указав собственный SQL сервер.

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TrackLocation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_TackLocation_id");

            entity.ToTable("TrackLocation");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DateEvent).HasColumnType("datetime");
            entity.Property(e => e.DateTrack)
                .HasColumnType("datetime")
                .HasColumnName("date_track");
            entity.Property(e => e.Imei)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("IMEI");
            entity.Property(e => e.Latitude)
                .HasColumnType("decimal(12, 9)")
                .HasColumnName("latitude");
            entity.Property(e => e.Longitude)
                .HasColumnType("decimal(12, 9)")
                .HasColumnName("longitude");
            entity.Property(e => e.TypeSource).HasDefaultValueSql("((1))");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
