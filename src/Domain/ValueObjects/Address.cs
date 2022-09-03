namespace Contacts.Domain.ValueObjects;

public class Address : ValueObject
{
    public string Street { get; private set; }
    public string ZipCode { get; private set; }
    public string City { get; private set; }
    public string Country { get; private set; }

    private Address() { }

    public Address(string street, string zipCode, string city, string country)
    {
        Street = street;
        ZipCode = zipCode;
        City = city;
        Country = country;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        // Using a yield return statement to return each element one at a time
        yield return Street;
        yield return ZipCode;
        yield return City;
        yield return Country;
    }
}
