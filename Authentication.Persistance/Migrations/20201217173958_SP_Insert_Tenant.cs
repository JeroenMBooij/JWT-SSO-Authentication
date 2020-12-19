using Microsoft.EntityFrameworkCore.Migrations;

namespace Authentication.Persistance.Migrations
{
    public partial class SP_Insert_Tenant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var spTenant = @"CREATE PROCEDURE [dbo].[sp_insert_tenant_with_1:1_attributes]
                @TenantId nvarchar(max),
                @TenantEmail nvarchar(max),
                @TenantFirstname nvarchar(max),
                @TenantMiddlename nvarchar(max),
                @TenantLastname nvarchar(max),
                @TenantPasswordhash nvarchar(max),
                @Created datetime2(7),
                @LastModified datetime2(7),
                @DashboardModel nvarchar(max),
                @UserSchemaDataModel nvarchar(max),
                @UserSchemaTrackModel nvarchar(max),
                @JwtConfigurationNbf datetimeoffset(7),
                @JwtConfigurationExp datetimeoffset(7)
            AS
            BEGIN
                SET NOCOUNT ON;

                INSERT INTO dbo.Tenants 
                VALUES (@TenantId, @TenantEmail, @TenantPasswordhash, @TenantFirstname, @TenantMiddlename, @TenantLastname, @Created, @LastModified);
                
                INSERT INTO dbo.Dashboards
                Values (@TenantId, @DashboardModel, @Created, @LastModified)

                INSERT INTO dbo.UserSchemas
                VALUES (@TenantId, @UserSchemaDataModel, @UserSchemaTrackModel, @Created, @LastModified)

                INSERT INTO dbo.JwtConfigurations
                VALUES (@TenantId, @Nbf, @Exp, @Created, @LastModified)
            END";

            migrationBuilder.Sql(spTenant);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
