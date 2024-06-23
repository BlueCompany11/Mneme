﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mneme.Integrations.GoogleBooks.Database;

#nullable disable

namespace Mneme.Integrations.GoogleBooks.Migrations
{
    [DbContext(typeof(GoogleBooksContext))]
    [Migration("20240613082555_init")]
    partial class init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.2");

            modelBuilder.Entity("Mneme.Integrations.GoogleBooks.Contract.GoogleBooksPreelaboration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Content")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("IntegrationId")
                        .HasColumnType("TEXT");

                    b.Property<string>("NoteType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Path")
                        .HasColumnType("TEXT");

                    b.Property<int>("SourceId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("IntegrationId")
                        .IsUnique();

                    b.HasIndex("SourceId");

                    b.ToTable("GoogleBooksPreelaborations");
                });

            modelBuilder.Entity("Mneme.Integrations.GoogleBooks.Contract.GoogleBooksSource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Active")
                        .HasColumnType("INTEGER");

                    b.Property<string>("IntegrationId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("IntegrationId")
                        .IsUnique();

                    b.ToTable("GoogleBooksSources");
                });

            modelBuilder.Entity("Mneme.Integrations.GoogleBooks.Contract.GoogleBooksPreelaboration", b =>
                {
                    b.HasOne("Mneme.Integrations.GoogleBooks.Contract.GoogleBooksSource", "Source")
                        .WithMany()
                        .HasForeignKey("SourceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Source");
                });
#pragma warning restore 612, 618
        }
    }
}
