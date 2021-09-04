using AuthenticationServer.Common.Constants.StoredProcedures;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Authentication.Persistance.Migrations
{
    public partial class stored_procedures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var spDomains = @$"CREATE PROCEDURE [dbo].[{SPName.InsertApplication}]
                @Id nvarchar(max), 
                @Name nvarchar(max),
                @Url nvarchar(max),
                @AdminId nvarchar(max),
                @LogoLocation nvarchar(max),
                @Created datetime2(7),
                @LastModified datetime2(7)
            AS
            BEGIN
                SET NOCOUNT ON;

                INSERT INTO dbo.Applications
                VALUES (@Id, @Name, @Url, @AdminId, @LogoLocation, @Created, @LastModified);

            END";


            migrationBuilder.Sql(spDomains);

            var spTenant = $@"CREATE PROCEDURE [dbo].[{SPName.InsertAccount}]
                @Id nvarchar(max),
                @Email nvarchar(256),
                @NormalizedEmail nvarchar(256),
                @EmailConfirmed bit,
                @Firstname nvarchar(max),
                @Middlename nvarchar(max),
                @Lastname nvarchar(max),
                @AdminId nvarchar(max),
                @UserName nvarchar(256),
                @NormalizedUserName nvarchar(256),
                @SecurityStamp nvarchar(max),
                @ConcurrencyStamp nvarchar(max),
                @PhoneNumber nvarchar(max),  
                @PhoneNumberConfirmed nvarchar(max),  
                @TwoFactorEnabled nvarchar(max),  
                @LockoutEnd Datetimeoffset(7), 
                @PasswordHash nvarchar(max),
                @LockoutEnabled bit, 
                @AccessFailedCount int,
                @Created datetime2(7),
                @LastModified datetime2(7)

            AS
            BEGIN
                SET NOCOUNT ON;

                INSERT INTO dbo.ApplicationsUsers 
                VALUES (@Id, @Firstname, @Middlename, @Lastname, @AdminId, @UserName, @NormalizedUserName,
                        @Email, @NormalizedEmail, @EmailConfirmed, @PasswordHash, 
                        @SecurityStamp, @ConcurrencyStamp, @PhoneNumber, @PhoneNumberConfirmed,
                        @TwoFactorEnabled, @LockOutEnd, @LockoutEnabled, @AccessFailedCount, @Created, @LastModified);
                
            END";

            migrationBuilder.Sql(spTenant);

            var spLanguage = @$"CREATE PROCEDURE [dbo].[{SPName.InsertAccountLanguage}]
                @UserId nvarchar(max),
                @LanguageId nvarchar(max)
            AS
            BEGIN
                SET NOCOUNT ON;

                INSERT INTO dbo.ApplicationUserEntityLanguageEntity
                VALUES (@LanguageId, @UserId);

            END";


            migrationBuilder.Sql(spLanguage);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
