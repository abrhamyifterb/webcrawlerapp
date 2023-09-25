﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebCrawlerApp.Infrastructure.Data;

#nullable disable

namespace WebCrawlerApp.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230925174404_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WebCrawlerApp.Core.Entities.CrawledData", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("CrawlTime")
                        .HasColumnType("float");

                    b.Property<Guid>("ExecutionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsRestricted")
                        .HasColumnType("bit");

                    b.Property<string>("Links")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ExecutionId");

                    b.ToTable("CrawledDatas");
                });

            modelBuilder.Entity("WebCrawlerApp.Core.Entities.Execution", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("CrawledSitesCount")
                        .HasColumnType("int");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<Guid>("WebsiteId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("WebsiteLabel")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("WebsiteId");

                    b.ToTable("Executions");
                });

            modelBuilder.Entity("WebCrawlerApp.Core.Entities.Website", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("BoundaryRegExp")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CrawlFrequency")
                        .HasColumnType("int");

                    b.Property<string>("CrawledData")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastExecutionTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Tags")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Websites");
                });

            modelBuilder.Entity("WebCrawlerApp.Core.Entities.CrawledData", b =>
                {
                    b.HasOne("WebCrawlerApp.Core.Entities.Execution", "Execution")
                        .WithMany()
                        .HasForeignKey("ExecutionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Execution");
                });

            modelBuilder.Entity("WebCrawlerApp.Core.Entities.Execution", b =>
                {
                    b.HasOne("WebCrawlerApp.Core.Entities.Website", "Website")
                        .WithMany("Executions")
                        .HasForeignKey("WebsiteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Website");
                });

            modelBuilder.Entity("WebCrawlerApp.Core.Entities.Website", b =>
                {
                    b.Navigation("Executions");
                });
#pragma warning restore 612, 618
        }
    }
}