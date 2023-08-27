using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Career.Migrations
{
    /// <inheritdoc />
    public partial class MsgTableUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "Seen",
                table: "Messages",
                newName: "AppliedJobId");

            migrationBuilder.AddColumn<DateTime>(
                name: "MsgDate",
                table: "Messages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Messages_AppliedJobId",
                table: "Messages",
                column: "AppliedJobId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AppliedJobs_AppliedJobId",
                table: "Messages",
                column: "AppliedJobId",
                principalTable: "AppliedJobs",
                principalColumn: "AppliedJobId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AppliedJobs_AppliedJobId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_AppliedJobId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "MsgDate",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "AppliedJobId",
                table: "Messages",
                newName: "Seen");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
