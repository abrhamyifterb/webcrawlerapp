using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebCrawlerApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigrationtwo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WebsiteLabel",
                table: "Executions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WebsiteLabel",
                table: "Executions");
        }
    }
}
