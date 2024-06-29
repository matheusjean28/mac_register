﻿// <auto-generated />
using DeviceContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MacSave.Migrations
{
    [DbContext(typeof(DeviceDb))]
    [Migration("20240629153601_CreateTablesAtNewEnvarioment")]
    partial class CreateTablesAtNewEnvarioment
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.10");

            modelBuilder.Entity("DeviceModel.DeviceCreate", b =>
                {
                    b.Property<string>("DeviceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Mac")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("RemoteAcess")
                        .HasColumnType("INTEGER");

                    b.HasKey("DeviceId");

                    b.ToTable("Devices");
                });

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

                    b.ToTable("MacstoDbs");
                });

            modelBuilder.Entity("Model.ProblemTreatWrapper.ProblemTreatWrapper", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("DeviceId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("DeviceId");

                    b.ToTable("Problems");
                });

            modelBuilder.Entity("Models.UsedAtWrapper.UsedAtWrapper.UsedAtWrapper", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("DeviceId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("DeviceId");

                    b.ToTable("UsedAtClient");
                });

            modelBuilder.Entity("ModelsFileToUpload.FileToUpload", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("Data")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("FilesUploads");
                });

            modelBuilder.Entity("Model.ProblemTreatWrapper.ProblemTreatWrapper", b =>
                {
                    b.HasOne("DeviceModel.DeviceCreate", "DeviceCreate")
                        .WithMany("Problems")
                        .HasForeignKey("DeviceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DeviceCreate");
                });

            modelBuilder.Entity("Models.UsedAtWrapper.UsedAtWrapper.UsedAtWrapper", b =>
                {
                    b.HasOne("DeviceModel.DeviceCreate", "DeviceCreate")
                        .WithMany("UsedAtClients")
                        .HasForeignKey("DeviceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DeviceCreate");
                });

            modelBuilder.Entity("DeviceModel.DeviceCreate", b =>
                {
                    b.Navigation("Problems");

                    b.Navigation("UsedAtClients");
                });
#pragma warning restore 612, 618
        }
    }
}