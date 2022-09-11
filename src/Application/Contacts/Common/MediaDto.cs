namespace Contacts.Application.Contacts.Commands.Common;

public class MediaDto
{
    public byte[] Bytes { get; set; }
    public string FileName { get; set; }
    public string MimeType { get; set; }
    public long Size { get; set; }
}