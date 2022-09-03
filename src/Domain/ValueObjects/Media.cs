namespace Contacts.Domain.ValueObjects;

public class Media : ValueObject
{
    public byte[] Bytes { get; private set; }
    public string FileName { get; private set; }
    public string FileExtension { get; private set; }
    public int Size { get; private set; }

    private Media() { }

    public Media(byte[] bytes, string fileName, string fileExtension, int size)
    {
        Bytes = bytes;
        FileName = fileName;
        FileExtension = fileExtension;
        Size = size;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        // Using a yield return statement to return each element one at a time
        yield return Bytes;
        yield return FileName;
        yield return FileExtension;
        yield return Size;
    }
}
