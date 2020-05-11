using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace TheGuardian.DataAccess.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Hospitals",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(maxLength: 80, nullable: false),
                    Address = table.Column<string>(maxLength: 35, nullable: false),
                    City = table.Column<string>(maxLength: 25, nullable: false),
                    State = table.Column<string>(maxLength: 2, nullable: false),
                    Zip = table.Column<int>(nullable: false),
                    Phone = table.Column<string>(maxLength: 15, nullable: false),
                    Website = table.Column<string>(maxLength: 100, nullable: false),
                    AggMedicalStaffRating = table.Column<double>(nullable: false, defaultValue: 1.0),
                    AggClericalStaffRating = table.Column<double>(nullable: false, defaultValue: 1.0),
                    AggFacilityRating = table.Column<double>(nullable: false, defaultValue: 1.0),
                    AggOverallRating = table.Column<double>(nullable: false, defaultValue: 1.0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hospitals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(maxLength: 20, nullable: false),
                    LastName = table.Column<string>(maxLength: 20, nullable: false),
                    Email = table.Column<string>(maxLength: 35, nullable: false),
                    Password = table.Column<string>(maxLength: 16, nullable: false),
                    Address = table.Column<string>(maxLength: 35, nullable: false),
                    City = table.Column<string>(maxLength: 25, nullable: false),
                    State = table.Column<string>(maxLength: 2, nullable: false),
                    Zip = table.Column<int>(nullable: false),
                    AccessLevel = table.Column<bool>(nullable: false, defaultValue: false),
                    AccountVerified = table.Column<bool>(nullable: false, defaultValue: false),
                    AccountDate = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(nullable: false),
                    HospitalId = table.Column<int>(nullable: false),
                    MedicalStaffRating = table.Column<int>(nullable: false),
                    ClericalStaffRating = table.Column<int>(nullable: false),
                    FacilityRating = table.Column<int>(nullable: false),
                    OverallRating = table.Column<double>(nullable: false),
                    WrittenFeedback = table.Column<string>(maxLength: 500, nullable: false),
                    DateSubmitted = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()"),
                    DateAdmittance = table.Column<DateTime>(type: "timestamp", nullable: false),
                    Reason = table.Column<string>(maxLength: 12, nullable: false),
                    ReasonOther = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_Hospitals_HospitalId",
                        column: x => x.HospitalId,
                        principalTable: "Hospitals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Hospitals",
                columns: new[] { "Id", "Address", "AggClericalStaffRating", "AggFacilityRating", "AggMedicalStaffRating", "AggOverallRating", "City", "Name", "Phone", "State", "Website", "Zip" },
                values: new object[] { 1, "621 North Hall Street", 4.0, 4.0, 4.0, 4.0, "Dallas", "Baylor Scott & White Heart and Vascular Hospital", "(214) 820-0600", "TX", "http://www.baylorhearthospital.com/handler.cfm?event=practice,main", 75226 });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessLevel", "AccountVerified", "Address", "City", "Email", "FirstName", "LastName", "Password", "State", "Zip" },
                values: new object[] { 1, true, true, "1001 S Center St", "Arlington", "superadmin@gmail.com", "Super", "Admin", "R3vTra1n1ng", "TX", 76010 });

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "Id", "ClericalStaffRating", "DateAdmittance", "DateSubmitted", "FacilityRating", "HospitalId", "MedicalStaffRating", "OverallRating", "Reason", "ReasonOther", "UserId", "WrittenFeedback" },
                values: new object[] { 1, 5, new DateTime(2020, 5, 10, 20, 19, 23, 857, DateTimeKind.Local).AddTicks(2769), new DateTime(2020, 5, 10, 20, 19, 23, 859, DateTimeKind.Local).AddTicks(807), 5, 1, 5, 5.0, "Surgery", "", 1, "Extremely satisfactory surgery. Five stars." });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_HospitalId",
                table: "Reviews",
                column: "HospitalId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "Hospitals");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
