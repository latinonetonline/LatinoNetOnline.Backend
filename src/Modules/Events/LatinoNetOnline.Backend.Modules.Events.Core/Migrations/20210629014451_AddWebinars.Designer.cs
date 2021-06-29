﻿// <auto-generated />
using System;
using LatinoNetOnline.Backend.Modules.Events.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Migrations
{
    [DbContext(typeof(EventDbContext))]
    [Migration("20210629014451_AddWebinars")]
    partial class AddWebinars
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("events")
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("LatinoNetOnline.Backend.Modules.Events.Core.Entities.Webinar", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Flyer")
                        .HasColumnType("text");

                    b.Property<string>("LiveStreaming")
                        .HasColumnType("text");

                    b.Property<long>("MeetupId")
                        .HasColumnType("bigint");

                    b.Property<Guid>("ProposalId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Time")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Webinars");
                });
#pragma warning restore 612, 618
        }
    }
}
