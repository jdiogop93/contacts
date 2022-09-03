using Contacts.Domain.Entities;
using Contacts.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Contacts.Infrastructure.Persistence.Configurations;

public class ContactGroupConfiguration : IEntityTypeConfiguration<ContactGroup>
{
    public void Configure(EntityTypeBuilder<ContactGroup> builder)
    {
        builder.ToTable("ContactGroups");

        builder.Property(t => t.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.HasMany(p => p.Contacts)
            .WithMany(p => p.Groups)
            .UsingEntity(j => j.ToTable("ContactGroupsContacts"));
    }
}
