﻿// <auto-generated />
using System;
using EpiBot.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Epibot.Migrations
{
    [DbContext(typeof(EpiBotContext))]
    [Migration("20220127041301_RemoveReminderSeeding")]
    partial class RemoveReminderSeeding
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.13");

            modelBuilder.Entity("EpiBot.Models.Byline", b =>
                {
                    b.Property<int>("BylineId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("BylineId");

                    b.ToTable("Bylines");

                    b.HasData(
                        new
                        {
                            BylineId = 1,
                            Email = "hannah@corgibyte.com",
                            Name = "Hannah Young"
                        },
                        new
                        {
                            BylineId = 2,
                            Email = "abminnick@gmail.com",
                            Name = "Aaron Minnick"
                        });
                });

            modelBuilder.Entity("EpiBot.Models.LoginReminderClient", b =>
                {
                    b.Property<int>("LoginReminderClientId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("LastAMReminder")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("LastPMReminder")
                        .HasColumnType("datetime(6)");

                    b.Property<ulong>("UserId")
                        .HasColumnType("bigint unsigned");

                    b.HasKey("LoginReminderClientId");

                    b.ToTable("LoginReminderClients");
                });
#pragma warning restore 612, 618
        }
    }
}
