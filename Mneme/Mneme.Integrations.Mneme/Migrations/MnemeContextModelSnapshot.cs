﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mneme.Integrations.Mneme.Database;

#nullable disable

namespace Mneme.Integrations.Mneme.Migrations
{
    [DbContext(typeof(MnemeContext))]
    partial class MnemeContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.2");

            modelBuilder.Entity("Mneme.Integrations.Mneme.Contract.MnemeNote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Content")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreationTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("IntegrationId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Path")
                        .HasColumnType("TEXT");

                    b.Property<int?>("SourceId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("IntegrationId")
                        .IsUnique();

                    b.HasIndex("SourceId");

                    b.ToTable("MnemeNotes");
                });

            modelBuilder.Entity("Mneme.Integrations.Mneme.Contract.MnemeSource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Active")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreationTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Details")
                        .HasColumnType("TEXT");

                    b.Property<string>("IntegrationId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("IntegrationId")
                        .IsUnique();

                    b.ToTable("MnemeSources");
                });

            modelBuilder.Entity("Mneme.Integrations.Mneme.Contract.MnemeNote", b =>
                {
                    b.HasOne("Mneme.Integrations.Mneme.Contract.MnemeSource", "Source")
                        .WithMany()
                        .HasForeignKey("SourceId");

                    b.Navigation("Source");
                });
#pragma warning restore 612, 618
        }
    }
}
