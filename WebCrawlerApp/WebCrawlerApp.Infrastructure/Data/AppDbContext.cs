using WebCrawlerApp.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace WebCrawlerApp.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Website>? Websites { get; set; }
        public DbSet<Execution>? Executions { get; set; }
        public DbSet<CrawledData>? CrawledDatas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var splitStringConverter = new ValueConverter<ICollection<string>, string>(
                v => string.Join(';', v),
                v => v.Split(new[] { ';' }).ToList()
            );

            var splitStringComparer = new ValueComparer<ICollection<string>>(
                (c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => (ICollection<string>)c.ToList()
            );

            modelBuilder.Entity<CrawledData>()
                .Property(e => e.Links)
                .HasConversion(splitStringConverter)
                .Metadata.SetValueComparer(splitStringComparer);

            modelBuilder.Entity<Website>()
                .Property(e => e.Tags)
                .HasConversion(splitStringConverter)
                .Metadata.SetValueComparer(splitStringComparer);
        }
    }
}
