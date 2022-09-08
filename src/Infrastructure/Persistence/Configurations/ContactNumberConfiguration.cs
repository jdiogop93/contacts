using Contacts.Domain.Entities;
using Contacts.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Contacts.Infrastructure.Persistence.Configurations;

public class ContactNumberConfiguration : IEntityTypeConfiguration<ContactNumber>
{
    public void Configure(EntityTypeBuilder<ContactNumber> builder)
    {
        builder.ToTable("ContactNumbers");

        builder.HasOne(x => x.Contact)
            .WithMany(x => x.Numbers)
            .HasForeignKey(x => x.ContactId);

        builder.Property(t => t.CountryCode)
            .HasMaxLength(5);

        builder.Property(t => t.PhoneNumber)
            .HasMaxLength(20);

        builder.Property(x => x.Type)
            .HasMaxLength(10)
            .HasConversion(
                v => v.ToString(),
                v => (ContactNumberType)Enum.Parse(typeof(ContactNumberType), v));

        builder.Property(t => t.Default);
    }
}
