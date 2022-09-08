using Contacts.Domain.Entities;
using Contacts.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Contacts.Infrastructure.Persistence.Configurations;

public class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.ToTable("Contacts");

        builder.OwnsOne(x => x.Photo, cb =>
        {
            cb.Property(p => p.Bytes)
                .HasColumnName(nameof(Media.Bytes));

            cb.Property(p => p.FileName)
                .HasMaxLength(200)
                .HasColumnName(nameof(Media.FileName));

            cb.Property(p => p.FileExtension)
                .HasMaxLength(200)
                .HasColumnName(nameof(Media.FileExtension));

            cb.Property(p => p.Size)
                .HasColumnName(nameof(Media.Size));
        });

        builder.Property(t => t.FirstName)
            .HasMaxLength(200);

        builder.Property(t => t.LastName)
            .HasMaxLength(200);

        builder.Property(t => t.Initials)
            .HasMaxLength(5);

        builder.OwnsOne(x => x.Address, cb =>
        {
            cb.Property(p => p.Street)
                .HasMaxLength(400)
                .HasColumnName(nameof(Address.Street));

            cb.Property(p => p.ZipCode)
                .HasMaxLength(20)
                .HasColumnName(nameof(Address.ZipCode));

            cb.Property(p => p.City)
                .HasMaxLength(100)
                .HasColumnName(nameof(Address.City));

            cb.Property(p => p.Country)
                .HasMaxLength(100)
                .HasColumnName(nameof(Address.Country));
        });

        builder.Property(t => t.Email)
            .HasMaxLength(200);

        builder.Property(x => x.Created);

        builder.Property(x => x.LastModified)
            .IsRequired(false);

        builder.Property(x => x.DisabledAt)
            .IsRequired(false);

        builder.Property(x => x.Active);
    }
}
