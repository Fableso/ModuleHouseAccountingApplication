using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeauthoraddedtotheAuditstablecs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChangedById",
                table: "Audits",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Audits_ChangedById",
                table: "Audits",
                column: "ChangedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Audits_AspNetUsers_ChangedById",
                table: "Audits",
                column: "ChangedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Audits_AspNetUsers_ChangedById",
                table: "Audits");

            migrationBuilder.DropIndex(
                name: "IX_Audits_ChangedById",
                table: "Audits");

            migrationBuilder.DropColumn(
                name: "ChangedById",
                table: "Audits");
        }
    }
}
