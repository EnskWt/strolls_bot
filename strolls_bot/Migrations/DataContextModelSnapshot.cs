﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using strolls_bot.Database;



namespace strolls_bot.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("strolls_bot.Models.TrackLocation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateEvent")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("DateTrack")
                        .HasColumnType("datetime")
                        .HasColumnName("date_track");

                    b.Property<string>("Imei")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("IMEI");

                    b.Property<decimal>("Latitude")
                        .HasColumnType("decimal(12, 9)")
                        .HasColumnName("latitude");

                    b.Property<decimal>("Longitude")
                        .HasColumnType("decimal(12, 9)")
                        .HasColumnName("longitude");

                    b.Property<double>("TotalDistance")
                        .HasColumnType("float");

                    b.Property<double>("TotalTime")
                        .HasColumnType("float");

                    b.Property<int>("TypeSource")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValueSql("((1))");

                    b.HasKey("Id")
                        .HasName("PK_TackLocation_id");

                    b.ToTable("TrackLocation", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}