using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogApi.Migrations
{
    /// <inheritdoc />
    public partial class ChangePosts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Posts_BlogId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_PostLikes_Posts_BlogId",
                table: "PostLikes");

            migrationBuilder.RenameColumn(
                name: "BlogId",
                table: "PostLikes",
                newName: "PostId");

            migrationBuilder.RenameIndex(
                name: "IX_PostLikes_BlogId",
                table: "PostLikes",
                newName: "IX_PostLikes_PostId");

            migrationBuilder.RenameColumn(
                name: "BlogId",
                table: "Comments",
                newName: "PostId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_BlogId",
                table: "Comments",
                newName: "IX_Comments_PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Posts_PostId",
                table: "Comments",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostLikes_Posts_PostId",
                table: "PostLikes",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Posts_PostId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_PostLikes_Posts_PostId",
                table: "PostLikes");

            migrationBuilder.RenameColumn(
                name: "PostId",
                table: "PostLikes",
                newName: "BlogId");

            migrationBuilder.RenameIndex(
                name: "IX_PostLikes_PostId",
                table: "PostLikes",
                newName: "IX_PostLikes_BlogId");

            migrationBuilder.RenameColumn(
                name: "PostId",
                table: "Comments",
                newName: "BlogId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_PostId",
                table: "Comments",
                newName: "IX_Comments_BlogId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Posts_BlogId",
                table: "Comments",
                column: "BlogId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostLikes_Posts_BlogId",
                table: "PostLikes",
                column: "BlogId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
