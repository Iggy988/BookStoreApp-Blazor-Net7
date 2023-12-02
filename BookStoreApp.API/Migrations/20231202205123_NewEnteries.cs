using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStoreApp.API.Migrations
{
    /// <inheritdoc />
    public partial class NewEnteries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Books",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "06650E98-2BE4-4D25-9EFD-EBF1BEDC7863",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ee5d3b44-ada8-4239-b01c-a708fdefd213", "AQAAAAIAAYagAAAAEHw3KSN+9ii8WjZ+ZkJ/+9aPZEs+DhMc12bcGhomyAOt20UDrUeoNdxszHzJpGZPXQ==", "7d991575-0664-4b4c-beda-a7a49e1c372b" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "792352C0-90B0-43E9-93C0-7B73E22422C5",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "af2310a6-67d5-4f1f-98d9-f4fc5afe4815", "AQAAAAIAAYagAAAAEHF8WAq/RzcAyimXbb/Kxc7/KfTpoqrHjIhfHAlYQei2O4JBxOTHuWjmKP9C1TUAWA==", "bd6e0a16-aa66-4981-af2d-f572037b0bc6" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Books",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "06650E98-2BE4-4D25-9EFD-EBF1BEDC7863",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fc73d5cb-8390-47ab-a813-b46e9da5238a", "AQAAAAIAAYagAAAAENm8Rg+/Gfw3q2CrLUrTj3WsxKAcBQPRmlvNrdNYul1quFoy2mqJL6vURAq+xOEcSw==", "a6f91290-9be6-4bde-9645-7bfd6fa7ff09" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "792352C0-90B0-43E9-93C0-7B73E22422C5",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9adad040-0763-4306-8c09-eb2ff9fa8540", "AQAAAAIAAYagAAAAEJH76HjIiR3aYQxuvULMTR091O2wl07xF0xqktElspX924DchbPy8AVsEUsagsv0UQ==", "c587405e-daea-451b-b7fa-3042c7f55ffb" });
        }
    }
}
