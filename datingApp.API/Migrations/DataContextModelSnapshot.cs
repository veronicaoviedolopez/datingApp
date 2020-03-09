﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using datingApp.API.Data;

namespace datingApp.API.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0");

            modelBuilder.Entity("datingApp.API.Models.Photo", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("dateAdded")
                        .HasColumnType("TEXT");

                    b.Property<string>("description")
                        .HasColumnType("TEXT");

                    b.Property<bool>("isMain")
                        .HasColumnType("INTEGER");

                    b.Property<string>("publicId")
                        .HasColumnType("TEXT");

                    b.Property<string>("url")
                        .HasColumnType("TEXT");

                    b.Property<int>("userId")
                        .HasColumnType("INTEGER");

                    b.HasKey("id");

                    b.HasIndex("userId");

                    b.ToTable("Photos");
                });

            modelBuilder.Entity("datingApp.API.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("PasswordHash")
                        .HasColumnType("BLOB");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("BLOB");

                    b.Property<string>("Username")
                        .HasColumnType("TEXT");

                    b.Property<string>("city")
                        .HasColumnType("TEXT");

                    b.Property<string>("country")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("created")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("dateOfBirth")
                        .HasColumnType("TEXT");

                    b.Property<string>("gender")
                        .HasColumnType("TEXT");

                    b.Property<string>("interests")
                        .HasColumnType("TEXT");

                    b.Property<string>("introduction")
                        .HasColumnType("TEXT");

                    b.Property<string>("knownAs")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("lastActive")
                        .HasColumnType("TEXT");

                    b.Property<string>("lookingFor")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("datingApp.API.Models.Value", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Values");
                });

            modelBuilder.Entity("datingApp.API.Models.Photo", b =>
                {
                    b.HasOne("datingApp.API.Models.User", "user")
                        .WithMany("photos")
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
