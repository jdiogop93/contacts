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
            .HasMaxLength(200);

        builder.HasMany(p => p.Contacts)
            .WithMany(p => p.Groups)
            .UsingEntity(j => j.ToTable("ContactGroupsContacts"));

        builder.Property(x => x.Created);

        builder.Property(x => x.LastModified)
            .IsRequired(false);

        builder.Property(x => x.DisabledAt)
            .IsRequired(false);

        builder.Property(x => x.Active);
    }
}
