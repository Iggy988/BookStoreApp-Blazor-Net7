using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookStoreApp.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedDefaultUserAndRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4485D0FC-B959-4718-9653-D10AF39F0E64", null, "User", "USER" },
                    { "BEAED54E-B72B-4EF6-BF58-8D9DC5CA51C5", null, "Administartor", "ADMINISTRATOR" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "06650E98-2BE4-4D25-9EFD-EBF1BEDC7863", 0, "fc73d5cb-8390-47ab-a813-b46e9da5238a", "user@bookstore.com", false, "System", "User", false, null, "USER@BOOKSTORE.COM", "user@BOOKSTORE.COM", "AQAAAAIAAYagAAAAENm8Rg+/Gfw3q2CrLUrTj3WsxKAcBQPRmlvNrdNYul1quFoy2mqJL6vURAq+xOEcSw==", null, false, "a6f91290-9be6-4bde-9645-7bfd6fa7ff09", false, "user@bookstore.com" },
                    { "792352C0-90B0-43E9-93C0-7B73E22422C5", 0, "9adad040-0763-4306-8c09-eb2ff9fa8540", "admin@bookstore.com", false, "System", "Admin", false, null, "ADMIN@BOOKSTORE.COM", "ADMIN@BOOKSTORE.COM", "AQAAAAIAAYagAAAAEJH76HjIiR3aYQxuvULMTR091O2wl07xF0xqktElspX924DchbPy8AVsEUsagsv0UQ==", null, false, "c587405e-daea-451b-b7fa-3042c7f55ffb", false, "admin@bookstore.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "4485D0FC-B959-4718-9653-D10AF39F0E64", "06650E98-2BE4-4D25-9EFD-EBF1BEDC7863" },
                    { "BEAED54E-B72B-4EF6-BF58-8D9DC5CA51C5", "792352C0-90B0-43E9-93C0-7B73E22422C5" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "4485D0FC-B959-4718-9653-D10AF39F0E64", "06650E98-2BE4-4D25-9EFD-EBF1BEDC7863" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "BEAED54E-B72B-4EF6-BF58-8D9DC5CA51C5", "792352C0-90B0-43E9-93C0-7B73E22422C5" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4485D0FC-B959-4718-9653-D10AF39F0E64");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "BEAED54E-B72B-4EF6-BF58-8D9DC5CA51C5");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "06650E98-2BE4-4D25-9EFD-EBF1BEDC7863");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "792352C0-90B0-43E9-93C0-7B73E22422C5");
        }
    }
}
