using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwitterClone.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateContentInCommentConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Comments",
                type: "nvarchar(280)",
                maxLength: 280,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "Comments");
        }
    }
}
