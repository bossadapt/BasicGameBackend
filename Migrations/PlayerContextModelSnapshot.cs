﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BasicGameBackend.Migrations
{
    [DbContext(typeof(PlayerContext))]
    partial class PlayerContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.2");

            modelBuilder.Entity("Map", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<double>("ATime")
                        .HasColumnType("REAL");

                    b.Property<double>("AuthorTime")
                        .HasColumnType("REAL");

                    b.Property<double>("BTime")
                        .HasColumnType("REAL");

                    b.Property<double>("SPlusTime")
                        .HasColumnType("REAL");

                    b.Property<double>("STime")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("Maps");

                    b.HasData(
                        new
                        {
                            Id = "pk_pylons",
                            ATime = 40.0,
                            AuthorTime = 31.75,
                            BTime = 50.0,
                            SPlusTime = 32.100000000000001,
                            STime = 33.5
                        },
                        new
                        {
                            Id = "movement_v2",
                            ATime = 30.0,
                            AuthorTime = 24.699999999999999,
                            BTime = 38.0,
                            SPlusTime = 25.5,
                            STime = 27.0
                        });
                });

            modelBuilder.Entity("Play", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("MapId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("PlayLength")
                        .HasColumnType("REAL");

                    b.Property<string>("PlayerId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PlayerUserName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("TimeSubmitted")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("MapId");

                    b.HasIndex("PlayerId");

                    b.HasIndex("PlayLength", "PlayerId");

                    b.ToTable("Plays");
                });

            modelBuilder.Entity("Player", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("AccountCreated")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastLoggedIn")
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("Play", b =>
                {
                    b.HasOne("Map", null)
                        .WithMany("Leaderboard")
                        .HasForeignKey("MapId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Player", null)
                        .WithMany("Plays")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Map", b =>
                {
                    b.Navigation("Leaderboard");
                });

            modelBuilder.Entity("Player", b =>
                {
                    b.Navigation("Plays");
                });
#pragma warning restore 612, 618
        }
    }
}
