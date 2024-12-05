using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwitterClone.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCommentToAddCreatedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Createdat",
                table: "Comments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Createdat",
                table: "Comments");
        }
    }
}
