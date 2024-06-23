using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Autoflow.Portal.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_users_Password",
                table: "users",
                column: "Password",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_Username",
                table: "users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_users_Password",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_Username",
                table: "users");
        }
    }
}
