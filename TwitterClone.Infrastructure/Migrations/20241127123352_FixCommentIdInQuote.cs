using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwitterClone.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixCommentIdInQuote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CommentId",
                table: "Quotes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Quotes_CommentId",
                table: "Quotes",
                column: "CommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Quotes_Comments_CommentId",
                table: "Quotes",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quotes_Comments_CommentId",
                table: "Quotes");

            migrationBuilder.DropIndex(
                name: "IX_Quotes_CommentId",
                table: "Quotes");

            migrationBuilder.DropColumn(
                name: "CommentId",
                table: "Quotes");
        }
    }
}
