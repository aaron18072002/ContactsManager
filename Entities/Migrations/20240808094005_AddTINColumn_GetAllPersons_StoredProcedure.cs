using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    public partial class AddTINColumn_GetAllPersons_StoredProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string getAllPersonsSP =
                @"
                    CREATE PROCEDURE [dbo].[GetAllPersons]
                    AS BEGIN
                        SELECT [PersonId]
                               ,[PersonName]
                               ,[Email]
                               ,[DateOfBirth]
                               ,[Gender]
                               ,[CountryId]
                               ,[Address]
                               ,[ReceiveNewsLetters]
                               ,[TIN]
                        FROM [dbo].[Persons]
                    END
                ";
            migrationBuilder.Sql(getAllPersonsSP);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string dropGetAllPersonsSP =
               @"
                    DROP PROCEDURE [dbo].[GetAllPersons]
                ";
            migrationBuilder.Sql(dropGetAllPersonsSP);
        }
    }
}
