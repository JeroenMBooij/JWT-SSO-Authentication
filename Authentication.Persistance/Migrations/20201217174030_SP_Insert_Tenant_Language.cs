using Microsoft.EntityFrameworkCore.Migrations;

namespace Authentication.Persistance.Migrations
{
    public partial class SP_Insert_Tenant_Language : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var spLanguage = @"CREATE PROCEDURE [dbo].[sp_insert_tenant_language]
                @TenantId nvarchar(max),
                @LanguageId nvarchar(max)
            AS
            BEGIN
                SET NOCOUNT ON;

                INSERT INTO dbo.LanguageEntityTenantEntity 
                VALUES (@LanguageId, @TenantId);

            END";


            migrationBuilder.Sql(spLanguage);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
