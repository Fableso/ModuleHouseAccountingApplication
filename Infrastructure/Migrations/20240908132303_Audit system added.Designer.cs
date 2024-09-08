﻿// <auto-generated />
using System;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(MhDbContext))]
    [Migration("20240908132303_Audit system added")]
    partial class Auditsystemadded
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entities.Audit", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("ChangeDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Operation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RecordId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TableName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Audits");
                });

            modelBuilder.Entity("Domain.Entities.AuditEntry", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AuditId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FieldName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NewValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OldValue")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AuditId");

                    b.ToTable("AuditEntries");
                });

            modelBuilder.Entity("Domain.Entities.House", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Brigade")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("CurrentState")
                        .HasColumnType("int");

                    b.Property<double>("Length")
                        .HasColumnType("float");

                    b.Property<DateOnly?>("OfficialEndDate")
                        .HasColumnType("date");

                    b.Property<DateOnly>("OfficialStartDate")
                        .HasColumnType("date");

                    b.Property<DateOnly?>("RealEndDate")
                        .HasColumnType("date");

                    b.Property<DateOnly?>("RealStartDate")
                        .HasColumnType("date");

                    b.Property<int>("TopLeftCornerX")
                        .HasColumnType("int");

                    b.Property<int>("TopLeftCornerY")
                        .HasColumnType("int");

                    b.Property<double>("Width")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Houses");
                });

            modelBuilder.Entity("Domain.Entities.HousePost", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("HouseId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<long>("PostId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("HouseId");

                    b.HasIndex("PostId");

                    b.ToTable("HousePosts");
                });

            modelBuilder.Entity("Domain.Entities.HouseWeekInfo", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("HouseId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("OnTime")
                        .HasColumnType("bit");

                    b.Property<DateOnly>("StartDate")
                        .HasColumnType("date");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("HouseId");

                    b.ToTable("HouseWeekInfos");
                });

            modelBuilder.Entity("Domain.Entities.Post", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<double?>("Area")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("Domain.Entities.WeekMark", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<long>("HouseWeekInfoId")
                        .HasColumnType("bigint");

                    b.Property<int>("MarkType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("HouseWeekInfoId");

                    b.ToTable("WeekMarks");
                });

            modelBuilder.Entity("Domain.Entities.AuditEntry", b =>
                {
                    b.HasOne("Domain.Entities.Audit", "Audit")
                        .WithMany("Changes")
                        .HasForeignKey("AuditId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Audit");
                });

            modelBuilder.Entity("Domain.Entities.HousePost", b =>
                {
                    b.HasOne("Domain.Entities.House", "House")
                        .WithMany("Posts")
                        .HasForeignKey("HouseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Post", "Post")
                        .WithMany("Houses")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("House");

                    b.Navigation("Post");
                });

            modelBuilder.Entity("Domain.Entities.HouseWeekInfo", b =>
                {
                    b.HasOne("Domain.Entities.House", "House")
                        .WithMany()
                        .HasForeignKey("HouseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("House");
                });

            modelBuilder.Entity("Domain.Entities.WeekMark", b =>
                {
                    b.HasOne("Domain.Entities.HouseWeekInfo", "HouseWeekInfo")
                        .WithMany("WeekComments")
                        .HasForeignKey("HouseWeekInfoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("HouseWeekInfo");
                });

            modelBuilder.Entity("Domain.Entities.Audit", b =>
                {
                    b.Navigation("Changes");
                });

            modelBuilder.Entity("Domain.Entities.House", b =>
                {
                    b.Navigation("Posts");
                });

            modelBuilder.Entity("Domain.Entities.HouseWeekInfo", b =>
                {
                    b.Navigation("WeekComments");
                });

            modelBuilder.Entity("Domain.Entities.Post", b =>
                {
                    b.Navigation("Houses");
                });
#pragma warning restore 612, 618
        }
    }
}
