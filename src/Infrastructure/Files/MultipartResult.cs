namespace Contacts.Infrastructure.Files;

public class MultipartResult : IDisposable
{
    public IList<MultipartFormDataMemoryStreamFileProvider> Files { get; set; } = new List<MultipartFormDataMemoryStreamFileProvider>();
    public string Model { get; set; }

    public void Dispose()
    {
        try
        {
            if (Files.Count > 0)
            {
                foreach (var f in Files)
                {
                    f.Stream.Dispose();
                    f.Dispose();
                }
            }
            Files = null;
            Model = null;
        }
        catch { }
    }
}
