﻿// <auto-generated />
using System;
using LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20220617233958_AddProposalLinks")]
    partial class AddProposalLinks
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("cfs")
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Entities.Proposal", b =>
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

                    b.Property<string>("Flyer")
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("KnowledgeAnswer")
                        .HasColumnType("text");

                    b.Property<string>("LiveStreaming")
                        .HasColumnType("text");

                    b.Property<string>("Meetup")
                        .HasColumnType("text");

                    b.Property<string>("Streamyard")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UseCaseAnswer")
                        .HasColumnType("text");

                    b.Property<int?>("WebinarNumber")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Proposals");
                });

            modelBuilder.Entity("LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Entities.Speaker", b =>
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

            modelBuilder.Entity("LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Entities.UnavailableDate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("UnavailableDates");
                });

            modelBuilder.Entity("ProposalSpeaker", b =>
                {
                    b.Property<Guid>("ProposalsId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SpeakersId")
                        .HasColumnType("uuid");

                    b.HasKey("ProposalsId", "SpeakersId");

                    b.HasIndex("SpeakersId");

                    b.ToTable("ProposalSpeaker");
                });

            modelBuilder.Entity("ProposalSpeaker", b =>
                {
                    b.HasOne("LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Entities.Proposal", null)
                        .WithMany()
                        .HasForeignKey("ProposalsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LatinoNetOnline.Backend.Modules.CallForSpeakers.Core.Entities.Speaker", null)
                        .WithMany()
                        .HasForeignKey("SpeakersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
