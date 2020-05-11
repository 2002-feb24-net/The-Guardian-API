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
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Address = table.Column<string>(maxLength: 60, nullable: false),
                    City = table.Column<string>(maxLength: 25, nullable: false),
                    State = table.Column<string>(maxLength: 2, nullable: false),
                    Zip = table.Column<int>(nullable: false),
                    Phone = table.Column<string>(maxLength: 20, nullable: false),
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
                columns: new[] { "Id", "Address", "City", "Name", "Phone", "State", "Website", "Zip" },
                values: new object[,]
                {
                    { 1, "621 North Hall Street", "Dallas", "Baylor Scott & White Heart and Vascular Hospital", "(214) 820-0600", "TX", "http://www.baylorhearthospital.com/handler.cfm?event=practice,main", 75226 },
                    { 29, "5900 Altamesa Boulevard", "Fort Worth", "USMD Hospital at Fort Worth", "(817) 433-9100", "TX", "http://www.usmdfortworth.com/", 76132 },
                    { 28, "6100 Harris Parkway", "Fort Worth", "Texas Health Southwest", "(817) 433-5000", "TX", "http://www.texashealth.org/southwestfw/", 76132 },
                    { 27, "1301 Pennsylvania Avenue", "Fort Worth", "Texas Health Fort Worth", "(817) 250-2000", "TX", "http://www.texashealth.org/fortworth/", 76104 },
                    { 26, "10864 Texas Health Trail", "Fort Worth", "Texas Health Alliance", "(682) 212-2000", "TX", "http://www.texashealth.org/alliance/", 76244 },
                    { 25, "900 Eighth Avenue", "Fort Worth", "Medical City Fort Worth", "(817) 877-5292", "TX", "http://medicalcityfortworth.com/", 76104 },
                    { 24, "3101 North Tarrant Pkwy", "Fort Worth", "Medical City Alliance", "(817) 639-1000", "TX", "http://medicalcityalliance.com/", 76177 },
                    { 23, "1500 South Main Street", "Fort Worth", "John Peter Smith Hospital", "(817) 921-3431", "TX", "http://www.jpshealthnet.org/", 76104 },
                    { 22, "1800 Park Place Avenue", "Fort Worth", "Baylor Scott & White Surgical Hospital Fort Worth", "(682) 703-5600", "TX", "http://www.bshfw.com/", 76110 },
                    { 21, "1400 Eighth Avenue", "Fort Worth", "Baylor Scott & White All Saints Medical Center", "(817) 926-2544", "TX", "http://www.bswhealth.com/locations/fort-worth/Pages/default.aspx", 76104 },
                    { 20, "801 West Interstate 20	Arlington", "Arlington", "USMD Hospital at Arlington", "(817) 472-3400", "TX", "http://www.usmdarlington.com/", 76017 },
                    { 19, "811 Wright Street", "Arlington", "Texas Health Heart & Vascular Hospital Arlington", "(817) 960-3500", "TX", "http://www.texashealthheartandvascular.org/", 76012 },
                    { 18, "800 West Randol Mill Road", "Arlington", "Texas Health Arlington Medical Hospital", "(817) 960-6100", "TX", "http://www.texashealth.org/arlington/", 76012 },
                    { 17, "3301 Matlock Rd.", "Arlington", "Medical City Arlington", "(817) 465-3241", "TX", "http://medicalcityarlington.com/", 76015 },
                    { 30, "3200 North Tarrant Parkway", "Fort Worth", "Wise Health Surgical Hospital at Parkway", "(817) 502-7300", "TX", "http://www.wisehealthsurgicalhospital.com/parkway/", 76177 },
                    { 16, "707 Highlander Boulevard", "Arlington", "Baylor Scott & White Orthopedic and Spine Hospital Arlington", "(855) 416-7846", "TX", "http://baylorarlington.com/", 76015 },
                    { 14, "6201 Harry Hines Boulevard", "Dallas", "William P. Clements Jr. University Hospital", "(214) 633-5555", "TX", "http://utswmed.org/locations/clements/william-p-clements-jr-university-hospital/", 75390 },
                    { 13, "7115 Greenville Avenue", "Dallas", "Texas Institute for Surgery", "(214) 647-5300", "TX", "http://www.texasinstituteforsurgery.com/", 75231 },
                    { 12, "8200 Walnut Hill Lane", "Dallas", "Texas Health Dallas", "(214) 345-6789", "TX", "http://www.texashealth.org/dallas/", 75231 },
                    { 11, "5200 Harry Hines Boulevard", "Dallas", "Parkland Hospital", "(214) 590-8000", "TX", "http://www.parklandhospital.com/", 75235 },
                    { 10, "9301 North Central Expressway Suite 100", "Dallas", "North Central Surgical Center", "(214) 265-2810", "TX", "http://www.northcentralsurgical.com/", 75231 },
                    { 9, "1441 North Beckley Avenue", "Dallas", "Methodist Dallas Medical Center", "(214) 947-8181", "TX", "http://www.methodisthealthsystem.org/methodist-dallas-medical-center/?L=true", 75203 },
                    { 8, "3500 West Wheatland Road", "Dallas", "Methodist Charlton Medical Center", "(214) 947-7777", "TX", "http://www.methodisthealthsystem.org/methodist-charlton-medical-center/", 75237 },
                    { 7, "7777 Forest Lane", "Dallas", "Medical City Dallas", "(972) 566-700", "TX", "http://medicalcityhospital.com/", 75230 },
                    { 6, "4500 South Lancaster Road", "Dallas", "Dallas VA Medical Center", "(214) 742-8387", "TX", "http://www.northtexas.va.gov/", 75216 },
                    { 5, "7 Medical Parkway", "Dallas", "Dallas Medical Center", "(972) 888-7000", "TX", "http://www.dallasmedcenter.com/", 75234 },
                    { 4, "9440 Poppy Drive", "Dallas", "City Hospital at White Rock", "(214) 324-6100", "TX", "http://cityhospital.co/", 75218 },
                    { 3, "3500 Gaston Street", "Dallas", "Baylor University Medical Center", "(214) 820-0111", "TX", "http://www.bswhealth.com/locations/dallas/Pages/default.aspx", 75246 },
                    { 2, "2727 East Lemmon Ave.", "Dallas", "Baylor Scott & White Medical Center Uptown", "(214) 443-3000", "TX", "http://www.bmcuptown.com/", 75204 },
                    { 15, "5151 Harry Hines Boulevard", "Dallas", "Zale Lipshy Pavilion-William P. Clements Jr. University Hospital", "(214) 645-5555", "TX", "http://utswmed.org/locations/zale-lipshy-pavilion/zale-lipshy-pavilion/", 75390 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessLevel", "AccountVerified", "Address", "City", "Email", "FirstName", "LastName", "Password", "State", "Zip" },
                values: new object[] { 1, true, true, "1001 S Center St", "Arlington", "superadmin@gmail.com", "Super", "Admin", "R3vTra1n1ng", "TX", 76010 });

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
