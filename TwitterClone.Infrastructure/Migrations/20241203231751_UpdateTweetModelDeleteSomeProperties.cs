using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwitterClone.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTweetModelDeleteSomeProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfBookmarks",
                table: "Tweets");

            migrationBuilder.DropColumn(
                name: "NumberOfComments",
                table: "Tweets");

            migrationBuilder.DropColumn(
                name: "NumberOfLikes",
                table: "Tweets");

            migrationBuilder.DropColumn(
                name: "NumberOfRetweets",
                table: "Tweets");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfBookmarks",
                table: "Tweets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfComments",
                table: "Tweets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfLikes",
                table: "Tweets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfRetweets",
                table: "Tweets",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
