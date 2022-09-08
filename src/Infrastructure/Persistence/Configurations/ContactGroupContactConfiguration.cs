//using Contacts.Domain.Entities;
//using Contacts.Domain.Enums;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace Contacts.Infrastructure.Persistence.Configurations;

//public class ContactGroupContactConfiguration : IEntityTypeConfiguration<ContactGroupContact>
//{
//    public void Configure(EntityTypeBuilder<ContactGroupContact> builder)
//    {
//        builder.ToTable("ContactGroupContacts");

//        builder.HasOne(x => x.Contact)
//            .WithMany(x => x.Groups)
//            .HasForeignKey(x => x.ContactId);

//        builder.HasOne(x => x.ContactGroup)
//            .WithMany(x => x.Contacts)
//            .HasForeignKey(x => x.ContactGroupId);

//        builder.Property(x => x.Created);

//        builder.Property(x => x.LastModified)
//            .IsRequired(false);

//        builder.Property(x => x.DisabledAt)
//            .IsRequired(false);

//        builder.Property(x => x.Active);
//    }
//}
