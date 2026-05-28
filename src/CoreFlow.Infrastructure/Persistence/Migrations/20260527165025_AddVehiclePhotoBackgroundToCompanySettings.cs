using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreFlow.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddVehiclePhotoBackgroundToCompanySettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VehiclePhotoBackgroundUrl",
                table: "CompanySettings",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VehiclePhotoBackgroundUrl",
                table: "CompanySettings");
        }
    }
}
