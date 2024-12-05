using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwitterClone.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTweetAndquoteModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TweetId",
                table: "Quotes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Quotes_TweetId",
                table: "Quotes",
                column: "TweetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Quotes_Tweets_TweetId",
                table: "Quotes",
                column: "TweetId",
                principalTable: "Tweets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quotes_Tweets_TweetId",
                table: "Quotes");

            migrationBuilder.DropIndex(
                name: "IX_Quotes_TweetId",
                table: "Quotes");

            migrationBuilder.DropColumn(
                name: "TweetId",
                table: "Quotes");
        }
    }
}
