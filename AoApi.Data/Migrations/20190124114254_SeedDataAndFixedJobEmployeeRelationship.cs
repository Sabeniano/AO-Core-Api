using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AoApi.Data.Migrations
{
    public partial class SeedDataAndFixedJobEmployeeRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Role_RoleId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Employees_JobId",
                table: "Employees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Role",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Jobs");

            migrationBuilder.RenameTable(
                name: "Role",
                newName: "Roles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Roles",
                table: "Roles",
                column: "Id");

            migrationBuilder.InsertData(
                table: "Jobs",
                columns: new[] { "Id", "CreatedOn", "Description", "JobTitle", "ModifiedOn" },
                values: new object[,]
                {
                    { new Guid("8068cbf6-c595-4733-9c24-8104e8454b4c"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Chief Executive Officer", "CEO", null },
                    { new Guid("6e0a64bc-7a50-4a6c-9125-8ccf6e54bf70"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Chief Information Officer", "CIO", null },
                    { new Guid("72163c34-3d32-4a78-9701-1f708053978f"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "IT Administrator", "Administrator", null },
                    { new Guid("e143ebff-a0bd-4107-889f-9bff26eda916"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "", "Sales Manager", null },
                    { new Guid("01c66e3e-8c25-4f5c-a2c5-512c79d09aa6"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "", "Accountant", null },
                    { new Guid("976a7a24-1c25-4a7f-97c6-1a019c5c148d"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "", "IT-Support", null },
                    { new Guid("0532f0df-c92d-4a10-9d1a-8a5935c541a2"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "", "Sales Assitant", null },
                    { new Guid("76c2fab8-7161-49b7-88c6-f3aaf484ea64"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "", "Janitor", null }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedOn", "ModifiedOn", "RoleTitle" },
                values: new object[,]
                {
                    { new Guid("b48d780f-44ad-408f-b5f6-81bdfe15e617"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Master Administrator" },
                    { new Guid("a3d1a284-6ce6-494a-a616-822239df2799"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Administrator" },
                    { new Guid("a105fa9d-8b3e-4a80-84e5-4a97c42ed931"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Employee" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Birthday", "City", "Country", "CreatedOn", "DeletedOn", "Email", "FirstName", "IsDeleted", "JobId", "LastName", "ModifiedOn", "PhoneNumber", "Street" },
                values: new object[,]
                {
                    { new Guid("9d90a452-9547-4d04-98ed-7d617e64ae1e"), new DateTimeOffset(new DateTime(1980, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)), "Copenhagen", "Denmark", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "MikkelHammer@gmail.com", "Mikkel", false, new Guid("8068cbf6-c595-4733-9c24-8104e8454b4c"), "Hammer", null, "29482948", "Telegrafvej 9" },
                    { new Guid("45c0c223-de18-4e6e-99ea-aed94e7469f1"), new DateTimeOffset(new DateTime(1980, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)), "Copenhagen", "Denmark", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "BalenDezai@gmail.com", "Balen", false, new Guid("8068cbf6-c595-4733-9c24-8104e8454b4c"), "Dezai", null, "29482949", "Telegrafvej 9" },
                    { new Guid("de9b842d-531a-4f17-ad69-0d3e11cb911d"), new DateTimeOffset(new DateTime(1997, 7, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)), "Copenhagen", "Denmark", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "JasonSabeniano@gmail.com", "Jason", false, new Guid("8068cbf6-c595-4733-9c24-8104e8454b4c"), "Sabeniano", null, "29482950", "Telegrafvej 9" },
                    { new Guid("195c5a46-96f9-446b-b4f7-864ab2dc49de"), new DateTimeOffset(new DateTime(1883, 7, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)), "Prague", "Czech republic", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "FranzKafka@gmail.com", "Franz", false, new Guid("72163c34-3d32-4a78-9701-1f708053978f"), "Kafka", null, "29482948", "Telegrafvej 9" },
                    { new Guid("174fd8d4-f72b-4059-a7ea-05e687026b0d"), new DateTimeOffset(new DateTime(1821, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)), "Moskva", "Russia", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "FjordorDostojevskij@gmail.com", "Fjodor", false, new Guid("0532f0df-c92d-4a10-9d1a-8a5935c541a2"), "Dostojevskij", null, "29482948", "Telegrafvej 9" },
                    { new Guid("163c03b3-a057-426d-afa3-1a2631a693e2"), new DateTimeOffset(new DateTime(1899, 7, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)), "Springfield", "USA", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "ErnestHemingway@gmail.com", "Ernest", false, new Guid("76c2fab8-7161-49b7-88c6-f3aaf484ea64"), "Hemingway", null, "29482948", "Telegrafvej 9" }
                });

            migrationBuilder.InsertData(
                table: "Schedules",
                columns: new[] { "Id", "CreatedOn", "EmployeeId", "EndHour", "IsHoliday", "IsWeekend", "ModifiedOn", "StartHour", "WorkDate" },
                values: new object[,]
                {
                    { new Guid("092ca7c5-ae83-4a52-a38b-cfc7c8e40e9a"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("9d90a452-9547-4d04-98ed-7d617e64ae1e"), new DateTimeOffset(new DateTime(2018, 12, 23, 12, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)), false, false, null, new DateTimeOffset(new DateTime(2018, 12, 23, 6, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)) },
                    { new Guid("5e0d5ad3-22b0-4bdc-808c-62b8f50d0796"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("45c0c223-de18-4e6e-99ea-aed94e7469f1"), new DateTimeOffset(new DateTime(2019, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)), true, true, null, new DateTimeOffset(new DateTime(2019, 1, 1, 6, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)), new DateTimeOffset(new DateTime(2019, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)) },
                    { new Guid("cf3c5f8e-94ee-494a-b0f1-4a48d9d8291f"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("de9b842d-531a-4f17-ad69-0d3e11cb911d"), new DateTimeOffset(new DateTime(2019, 1, 1, 18, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)), true, true, null, new DateTimeOffset(new DateTime(2019, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)), new DateTimeOffset(new DateTime(2019, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedOn", "DeletedOn", "Email", "EmployeeId", "IsDeleted", "ModifiedOn", "Password", "RoleId", "Username" },
                values: new object[,]
                {
                    { new Guid("a105fa9d-8b3e-4a80-84e5-4a97c42ed931"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "EU@gmail.com", new Guid("163c03b3-a057-426d-afa3-1a2631a693e2"), false, null, "EU1234", new Guid("a105fa9d-8b3e-4a80-84e5-4a97c42ed931"), "Employeeuser" },
                    { new Guid("a3d1a284-6ce6-494a-a616-822239df2799"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "AU@gmail.com", new Guid("195c5a46-96f9-446b-b4f7-864ab2dc49de"), false, null, "AU1234", new Guid("a3d1a284-6ce6-494a-a616-822239df2799"), "Administrativeuser" },
                    { new Guid("9a28ea13-7a24-4a4c-8394-37605ff69c82"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "SU@gmail.com", new Guid("de9b842d-531a-4f17-ad69-0d3e11cb911d"), false, null, "SU1234", new Guid("b48d780f-44ad-408f-b5f6-81bdfe15e617"), "Superuser" }
                });

            migrationBuilder.InsertData(
                table: "Wallets",
                columns: new[] { "Id", "CreatedOn", "EmployeeId", "ModifiedOn", "PaymentMethod", "Salary", "Wage" },
                values: new object[,]
                {
                    { new Guid("f2d86ec1-0735-4f47-8087-0c5c311f3b74"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("174fd8d4-f72b-4059-a7ea-05e687026b0d"), null, "Hourly", 400, 0 },
                    { new Guid("68accfc2-b922-4519-9bd2-20e235b6db2e"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("195c5a46-96f9-446b-b4f7-864ab2dc49de"), null, "Monthly", 600, 0 },
                    { new Guid("7f36e8e7-b5cd-43ef-a71d-8cfa2355d8ab"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("de9b842d-531a-4f17-ad69-0d3e11cb911d"), null, "Monthly", 0, 50000 },
                    { new Guid("ce442ad4-37a4-43f4-9a6d-5f7ab15df011"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("45c0c223-de18-4e6e-99ea-aed94e7469f1"), null, "Monthly", 0, 50000 },
                    { new Guid("303814ca-54f0-4fbb-955b-7ffd33b10b9d"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("9d90a452-9547-4d04-98ed-7d617e64ae1e"), null, "Monthly", 0, 50000 },
                    { new Guid("8ae640bb-1534-4e25-aa97-d85128d50aa8"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("163c03b3-a057-426d-afa3-1a2631a693e2"), null, "Hourly", 300, 0 }
                });

            migrationBuilder.InsertData(
                table: "Workhours",
                columns: new[] { "Id", "CreatedOn", "EmployeeId", "ModifiedOn", "TotalHoursThisPaycheck", "TotalOvertimeHoursThisPaycheck" },
                values: new object[,]
                {
                    { new Guid("9af65a2d-a8bd-410c-a3a2-61b8b2427f5e"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("de9b842d-531a-4f17-ad69-0d3e11cb911d"), null, 32, 0 },
                    { new Guid("1d0d61a3-9dff-4f0b-abc3-524b310d6fe4"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("45c0c223-de18-4e6e-99ea-aed94e7469f1"), null, 32, 0 },
                    { new Guid("ad0a45a1-59e0-47ac-9132-b3f4aea940f9"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("195c5a46-96f9-446b-b4f7-864ab2dc49de"), null, 32, 5 },
                    { new Guid("044b879d-1486-4bc5-9907-2a7e14c84f7c"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("9d90a452-9547-4d04-98ed-7d617e64ae1e"), null, 32, 0 },
                    { new Guid("40ac68bf-f764-4e0f-9197-dcf365c493af"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("174fd8d4-f72b-4059-a7ea-05e687026b0d"), null, 32, 5 },
                    { new Guid("1a7824d4-e3d3-4be7-bf6e-3a2c52583628"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("163c03b3-a057-426d-afa3-1a2631a693e2"), null, 32, 20 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_IsDeleted",
                table: "Users",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_IsDeleted",
                table: "Employees",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_JobId",
                table: "Employees",
                column: "JobId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_IsDeleted",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Employees_IsDeleted",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_JobId",
                table: "Employees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Roles",
                table: "Roles");

            migrationBuilder.DeleteData(
                table: "Jobs",
                keyColumn: "Id",
                keyValue: new Guid("01c66e3e-8c25-4f5c-a2c5-512c79d09aa6"));

            migrationBuilder.DeleteData(
                table: "Jobs",
                keyColumn: "Id",
                keyValue: new Guid("6e0a64bc-7a50-4a6c-9125-8ccf6e54bf70"));

            migrationBuilder.DeleteData(
                table: "Jobs",
                keyColumn: "Id",
                keyValue: new Guid("976a7a24-1c25-4a7f-97c6-1a019c5c148d"));

            migrationBuilder.DeleteData(
                table: "Jobs",
                keyColumn: "Id",
                keyValue: new Guid("e143ebff-a0bd-4107-889f-9bff26eda916"));

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "Id",
                keyValue: new Guid("092ca7c5-ae83-4a52-a38b-cfc7c8e40e9a"));

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "Id",
                keyValue: new Guid("5e0d5ad3-22b0-4bdc-808c-62b8f50d0796"));

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "Id",
                keyValue: new Guid("cf3c5f8e-94ee-494a-b0f1-4a48d9d8291f"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("9a28ea13-7a24-4a4c-8394-37605ff69c82"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a105fa9d-8b3e-4a80-84e5-4a97c42ed931"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a3d1a284-6ce6-494a-a616-822239df2799"));

            migrationBuilder.DeleteData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: new Guid("303814ca-54f0-4fbb-955b-7ffd33b10b9d"));

            migrationBuilder.DeleteData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: new Guid("68accfc2-b922-4519-9bd2-20e235b6db2e"));

            migrationBuilder.DeleteData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: new Guid("7f36e8e7-b5cd-43ef-a71d-8cfa2355d8ab"));

            migrationBuilder.DeleteData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: new Guid("8ae640bb-1534-4e25-aa97-d85128d50aa8"));

            migrationBuilder.DeleteData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: new Guid("ce442ad4-37a4-43f4-9a6d-5f7ab15df011"));

            migrationBuilder.DeleteData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: new Guid("f2d86ec1-0735-4f47-8087-0c5c311f3b74"));

            migrationBuilder.DeleteData(
                table: "Workhours",
                keyColumn: "Id",
                keyValue: new Guid("044b879d-1486-4bc5-9907-2a7e14c84f7c"));

            migrationBuilder.DeleteData(
                table: "Workhours",
                keyColumn: "Id",
                keyValue: new Guid("1a7824d4-e3d3-4be7-bf6e-3a2c52583628"));

            migrationBuilder.DeleteData(
                table: "Workhours",
                keyColumn: "Id",
                keyValue: new Guid("1d0d61a3-9dff-4f0b-abc3-524b310d6fe4"));

            migrationBuilder.DeleteData(
                table: "Workhours",
                keyColumn: "Id",
                keyValue: new Guid("40ac68bf-f764-4e0f-9197-dcf365c493af"));

            migrationBuilder.DeleteData(
                table: "Workhours",
                keyColumn: "Id",
                keyValue: new Guid("9af65a2d-a8bd-410c-a3a2-61b8b2427f5e"));

            migrationBuilder.DeleteData(
                table: "Workhours",
                keyColumn: "Id",
                keyValue: new Guid("ad0a45a1-59e0-47ac-9132-b3f4aea940f9"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("163c03b3-a057-426d-afa3-1a2631a693e2"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("174fd8d4-f72b-4059-a7ea-05e687026b0d"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("195c5a46-96f9-446b-b4f7-864ab2dc49de"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("45c0c223-de18-4e6e-99ea-aed94e7469f1"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("9d90a452-9547-4d04-98ed-7d617e64ae1e"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("de9b842d-531a-4f17-ad69-0d3e11cb911d"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a105fa9d-8b3e-4a80-84e5-4a97c42ed931"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a3d1a284-6ce6-494a-a616-822239df2799"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b48d780f-44ad-408f-b5f6-81bdfe15e617"));

            migrationBuilder.DeleteData(
                table: "Jobs",
                keyColumn: "Id",
                keyValue: new Guid("0532f0df-c92d-4a10-9d1a-8a5935c541a2"));

            migrationBuilder.DeleteData(
                table: "Jobs",
                keyColumn: "Id",
                keyValue: new Guid("72163c34-3d32-4a78-9701-1f708053978f"));

            migrationBuilder.DeleteData(
                table: "Jobs",
                keyColumn: "Id",
                keyValue: new Guid("76c2fab8-7161-49b7-88c6-f3aaf484ea64"));

            migrationBuilder.DeleteData(
                table: "Jobs",
                keyColumn: "Id",
                keyValue: new Guid("8068cbf6-c595-4733-9c24-8104e8454b4c"));

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "Role");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "Jobs",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Jobs",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Role",
                table: "Role",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_JobId",
                table: "Employees",
                column: "JobId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Role_RoleId",
                table: "Users",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
