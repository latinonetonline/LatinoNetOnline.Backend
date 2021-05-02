﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using LatinoNetOnline.Backend.Modules.CallForProposals.Core.Data;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace LatinoNetOnline.Backend.Modules.CallForProposals.Core.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("csp")
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.5")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("LatinoNetOnline.Backend.Modules.CallForProposals.Core.Entities.Proposal", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AudienceAnswer")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("EventDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("KnowledgeAnswer")
                        .HasColumnType("text");

                    b.Property<Guid>("SpeakerId")
                        .HasColumnType("uuid");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UseCaseAnswer")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("SpeakerId");

                    b.ToTable("Proposals");
                });

            modelBuilder.Entity("LatinoNetOnline.Backend.Modules.CallForProposals.Core.Entities.Speaker", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Twitter")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Speakers");
                });

            modelBuilder.Entity("LatinoNetOnline.Backend.Modules.CallForProposals.Core.Entities.Proposal", b =>
                {
                    b.HasOne("LatinoNetOnline.Backend.Modules.CallForProposals.Core.Entities.Speaker", "Speaker")
                        .WithMany("Proposals")
                        .HasForeignKey("SpeakerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Speaker");
                });

            modelBuilder.Entity("LatinoNetOnline.Backend.Modules.CallForProposals.Core.Entities.Speaker", b =>
                {
                    b.Navigation("Proposals");
                });
#pragma warning restore 612, 618
        }
    }
}
