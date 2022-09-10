using Contacts.Domain.Entities;
using Contacts.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Contacts.Infrastructure.Persistence.Configurations;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable("Messages");

        builder.Property(x => x.Type)
            .HasMaxLength(10)
            .HasConversion(
                v => v.ToString(),
                v => (MessageType)Enum.Parse(typeof(MessageType), v));

        builder.Property(t => t.Subject)
            .HasMaxLength(200)
            .IsRequired(false);

        builder.Property(t => t.Content);

        builder.Property(t => t.PhoneNumbers)
            .IsRequired(false);

        builder.Property(t => t.EmailsTo)
            .IsRequired(false);

        builder.Property(t => t.EmailsCc)
            .IsRequired(false);

        builder.Property(t => t.EmailsBcc)
            .IsRequired(false);

        builder.Property(x => x.ResultStatus)
            .HasMaxLength(10)
            .HasConversion(
                v => v.ToString(),
                v => (MessageResultStatus)Enum.Parse(typeof(MessageResultStatus), v));

        builder.Property(x => x.Result);

        builder.Property(x => x.Created);

        builder.Property(x => x.LastModified)
            .IsRequired(false);

        builder.Property(x => x.DisabledAt)
            .IsRequired(false);

        builder.Property(x => x.Active);
    }
}
