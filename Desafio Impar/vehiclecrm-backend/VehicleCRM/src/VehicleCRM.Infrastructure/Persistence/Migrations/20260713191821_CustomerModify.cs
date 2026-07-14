using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleCRM.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CustomerModify : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Customers_Email_Unique",
                table: "Customers",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Customers_Email_Unique",
                table: "Customers");
        }
    }
}
