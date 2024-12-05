using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwitterClone.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateQuoteCommentandLikeModelsToMakeThemOnTweetOnly : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Quotes_QuoteId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Quotes_Comments_CommentId",
                table: "Quotes");

            migrationBuilder.DropIndex(
                name: "IX_Quotes_CommentId",
                table: "Quotes");

            migrationBuilder.DropIndex(
                name: "IX_Comments_QuoteId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "CommentId",
                table: "Quotes");

            migrationBuilder.DropColumn(
                name: "QuoteId",
                table: "Comments");

            migrationBuilder.AlterColumn<int>(
                name: "QuoteId",
                table: "Likes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CommentId",
                table: "Likes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "TweetId",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "CommentQuote",
                columns: table => new
                {
                    CommentsId = table.Column<int>(type: "int", nullable: false),
                    QuotesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentQuote", x => new { x.CommentsId, x.QuotesId });
                    table.ForeignKey(
                        name: "FK_CommentQuote_Comments_CommentsId",
                        column: x => x.CommentsId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommentQuote_Quotes_QuotesId",
                        column: x => x.QuotesId,
                        principalTable: "Quotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommentQuote_QuotesId",
                table: "CommentQuote",
                column: "QuotesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentQuote");

            migrationBuilder.AddColumn<int>(
                name: "CommentId",
                table: "Quotes",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "QuoteId",
                table: "Likes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CommentId",
                table: "Likes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TweetId",
                table: "Comments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "QuoteId",
                table: "Comments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Quotes_CommentId",
                table: "Quotes",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_QuoteId",
                table: "Comments",
                column: "QuoteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Quotes_QuoteId",
                table: "Comments",
                column: "QuoteId",
                principalTable: "Quotes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Quotes_Comments_CommentId",
                table: "Quotes",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
