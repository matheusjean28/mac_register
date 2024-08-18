﻿// <auto-generated />
using System;
using DeviceContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MacSave.Migrations
{
    [DbContext(typeof(DeviceDb))]
    [Migration("20240727151812_RenameDeviceTables")]
    partial class RenameDeviceTables
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

                    b.Property<string>("DeviceName")
                        .IsRequired()
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

                    b.HasIndex("DeviceName");

                    b.ToTable("Devices");
                });

            modelBuilder.Entity("MacSave.Models.Categories.Maker", b =>
                {
                    b.Property<string>("MakerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("MakerName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("MakerId");

                    b.ToTable("Makers");
                });

            modelBuilder.Entity("MacSave.Models.Categories.Models_of_Devices.DeviceCategory", b =>
                {
                    b.Property<string>("DeviceCategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("DeviceCategoryName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("MakerId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("OperationMode")
                        .HasColumnType("INTEGER");

                    b.HasKey("DeviceCategoryId");

                    b.HasIndex("MakerId");

                    b.ToTable("DeviceCategories");
                });

            modelBuilder.Entity("MacSave.Models.SinalHistory.SinalHistory", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("DeviceId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SinalRX")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SinalTX")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("DeviceId");

                    b.ToTable("Histories");
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

                    b.Property<string>("DeviceId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ProblemDescription")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ProblemName")
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

            modelBuilder.Entity("DeviceModel.DeviceCreate", b =>
                {
                    b.HasOne("MacSave.Models.Categories.Models_of_Devices.DeviceCategory", "DeviceCategory")
                        .WithMany("Devices")
                        .HasForeignKey("DeviceName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DeviceCategory");
                });

            modelBuilder.Entity("MacSave.Models.Categories.Models_of_Devices.DeviceCategory", b =>
                {
                    b.HasOne("MacSave.Models.Categories.Maker", null)
                        .WithMany("DeviceCategories")
                        .HasForeignKey("MakerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MacSave.Models.SinalHistory.SinalHistory", b =>
                {
                    b.HasOne("DeviceModel.DeviceCreate", "DeviceCreate")
                        .WithMany("SinalHistory")
                        .HasForeignKey("DeviceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DeviceCreate");
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

                    b.Navigation("SinalHistory");

                    b.Navigation("UsedAtClients");
                });

            modelBuilder.Entity("MacSave.Models.Categories.Maker", b =>
                {
                    b.Navigation("DeviceCategories");
                });

            modelBuilder.Entity("MacSave.Models.Categories.Models_of_Devices.DeviceCategory", b =>
                {
                    b.Navigation("Devices");
                });
#pragma warning restore 612, 618
        }
    }
}
