using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGenderEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE \"Users\" ALTER COLUMN \"Gender\" TYPE integer USING CASE \"Gender\" WHEN 'Male' THEN 0 WHEN 'Female' THEN 1 ELSE NULL END");
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE \"Users\" ALTER COLUMN \"Gender\" TYPE text USING CASE \"Gender\" WHEN 0 THEN 'Male' WHEN 1 THEN 'Female' ELSE NULL END");
        }

    }
}
