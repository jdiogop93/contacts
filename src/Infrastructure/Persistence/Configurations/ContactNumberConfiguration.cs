using Contacts.Domain.Entities;
using Contacts.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Contacts.Infrastructure.Persistence.Configurations;

public class ContactNumberConfiguration : IEntityTypeConfiguration<ContactNumber>
{
    public void Configure(EntityTypeBuilder<ContactNumber> builder)
    {
        builder.HasOne(x => x.Contact)
            .WithMany(x => x.Numbers)
            .HasForeignKey(x => x.ContactId);

        builder.Property(t => t.CountryCode)
            .HasMaxLength(5)
            .IsRequired();

        builder.Property(t => t.PhoneNumber)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.Type)
            .HasMaxLength(10)
            .HasConversion(
                v => v.ToString(),
                v => (ContactNumberType)Enum.Parse(typeof(ContactNumberType), v));

    }
}
