﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebCrawlerApp.Infrastructure.Data;

#nullable disable

namespace WebCrawlerApp.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("WebCrawlerApp.Core.Entities.CrawledData", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<double>("CrawlTime")
                        .HasColumnType("double");

                    b.Property<Guid>("ExecutionId")
                        .HasColumnType("char(36)");

                    b.Property<bool>("IsRestricted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Links")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("ExecutionId");

                    b.ToTable("CrawledDatas");
                });

            modelBuilder.Entity("WebCrawlerApp.Core.Entities.Execution", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int>("CrawledSitesCount")
                        .HasColumnType("int");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<Guid>("WebsiteId")
                        .HasColumnType("char(36)");

                    b.Property<string>("WebsiteLabel")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("WebsiteId");

                    b.ToTable("Executions");
                });

            modelBuilder.Entity("WebCrawlerApp.Core.Entities.Website", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("BoundaryRegExp")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("CrawlFrequency")
                        .HasColumnType("int");

                    b.Property<string>("CrawledData")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("LastExecutionTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Tags")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("longtext");

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
