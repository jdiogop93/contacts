namespace Contacts.Domain.Entities;

public class Message : BaseAuditableEntity
{
    public MessageType Type { get; set; }
    public string? Subject { get; set; }
    public string Content { get; set; }
    public string? PhoneNumbers { get; set; }
    public string? EmailsTo { get; set; }
    public string? EmailsCc { get; set; }
    public string? EmailsBcc { get; set; }
}
