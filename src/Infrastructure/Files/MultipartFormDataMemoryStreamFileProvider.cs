using Microsoft.Net.Http.Headers;

namespace Contacts.Infrastructure.Files;

public class MultipartFormDataMemoryStreamFileProvider : IDisposable
{
    public MultipartFormDataMemoryStreamFileProvider(string fileName, Stream stream, MediaTypeHeaderValue mediaType)
    {
        FileName = fileName;
        Stream = stream;
        MediaType = mediaType;
    }

    public string FileName { get; private set; }
    public Stream Stream { get; private set; }
    public MediaTypeHeaderValue MediaType { get; private set; }

    public void Dispose()
    {
        Stream = null;
        MediaType = null;
        FileName = null;
    }
}