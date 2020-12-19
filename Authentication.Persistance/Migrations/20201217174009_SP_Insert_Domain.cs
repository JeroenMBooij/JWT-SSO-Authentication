using Microsoft.EntityFrameworkCore.Migrations;

namespace Authentication.Persistance.Migrations
{
    public partial class SP_Insert_Domain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var spDomains = @"CREATE PROCEDURE [dbo].[sp_insert_tenant_domain]
                @DomainId nvarchar(max),
                @TenantId nvarchar(max),
                @DomainUrl nvarchar(max),
                @DomainLogoLocation nvarchar(max),
                @Created datetime2(7),
                @LastModified datetime2(7)
            AS
            BEGIN
                SET NOCOUNT ON;

                INSERT INTO dbo.Domains 
                VALUES (@DomainId, @Url, @TenantId, @LogoLocation, @Created, @LastModified);

            END";


            migrationBuilder.Sql(spDomains);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
