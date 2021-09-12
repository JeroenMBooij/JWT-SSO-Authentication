using AuthenticationServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Authentication.Persistance.Seeds
{
    public static class LanguageSeeder
    {
        public static void LanguageSeed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LanguageEntity>()
               .HasData(
                new LanguageEntity() { Id = Guid.Parse("8f36b26c-05df-4b9b-ac26-4485c1f3c3f0"), Name = "Nederlands", Code = "nl", RfcCode3066 = "nl-NL", Created = new DateTime(2021, 06, 06) },
                new LanguageEntity() { Id = Guid.Parse("f25f58b9-2f69-488d-b7e0-d77d5214b536"), Name = "English", Code = "us", RfcCode3066 = "us-US", Created = new DateTime(2021, 06, 06) }
            );
        }
    }
}
