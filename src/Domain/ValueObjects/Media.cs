namespace Contacts.Domain.ValueObjects;

public class Media : ValueObject
{
    public byte[] Bytes { get; private set; }
    public string FileName { get; private set; }
    public string MimeType { get; private set; }
    public long Size { get; private set; }

    private Media() { }

    public Media(byte[] bytes, string fileName, string mimeType, long size)
    {
        Bytes = bytes;
        FileName = fileName;
        MimeType = mimeType;
        Size = size;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        // Using a yield return statement to return each element one at a time
        yield return Bytes;
        yield return FileName;
        yield return MimeType;
        yield return Size;
    }
}
