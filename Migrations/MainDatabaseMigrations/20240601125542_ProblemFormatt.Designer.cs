﻿// <auto-generated />
using System;
using MainDatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MacSave.Migrations.MainDatabaseMigrations
{
    [DbContext(typeof(MainDatabase))]
    [Migration("20240601125542_ProblemFormatt")]
    partial class ProblemFormatt
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.10");

            modelBuilder.Entity("MacToDatabaseModel.MacToDatabase", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Mac")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<bool>("RemoteAccess")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("DevicesToMain");
                });

            modelBuilder.Entity("Model.ProblemTreatWrapper.ProblemTreatWrapper", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("MacToDatabaseId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("MacToDatabaseId");

                    b.ToTable("ProblemTreatWrapper");
                });

            modelBuilder.Entity("Model.ProblemTreatWrapper.ProblemTreatWrapper", b =>
                {
                    b.HasOne("MacToDatabaseModel.MacToDatabase", null)
                        .WithMany("Problems")
                        .HasForeignKey("MacToDatabaseId");
                });

            modelBuilder.Entity("MacToDatabaseModel.MacToDatabase", b =>
                {
                    b.Navigation("Problems");
                });
#pragma warning restore 612, 618
        }
    }
}
