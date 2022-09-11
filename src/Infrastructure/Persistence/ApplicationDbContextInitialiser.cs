using Contacts.Domain.Entities;
using Contacts.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using static AutoMapper.Internal.ExpressionFactory;
using static Duende.IdentityServer.Models.IdentityResources;

namespace Contacts.Infrastructure.Persistence;

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task Seed()
    {
        try
        {
            await TrySeed();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeed()
    {
        SeedContactsAndNumbers();
        SeedTestContactsWithNumbers();
        SeedContactGroups();
        SeedContactGroupContacts();

        _context.SaveChanges();
    }

    private void SeedContactsAndNumbers()
    {
        if (!_context.Contacts.Any(x => x.Active))
        {
            var contactsToAdd = new List<Contact>
            {
                //1
                new Contact
                {
                    FirstName = "Américo",
                    LastName = "Pereira",
                    Initials = "AP",
                    Address = new Domain.ValueObjects.Address("Nogueira", "4900", "Viana do Castelo", "Portugal"),
                    Email = "americo.pereira@emailteste.pt",
                    Numbers = new List<ContactNumber>
                    {
                        new ContactNumber
                        {
                            CountryCode = "+111",
                            PhoneNumber = "111111111",
                            Type = Domain.Enums.ContactNumberType.HOME
                        },
                        new ContactNumber
                        {
                            CountryCode = "+351",
                            PhoneNumber = "222222222",
                            Type = Domain.Enums.ContactNumberType.MOBILE,
                            Default = true
                        }
                    }
                },
                //2
                new Contact
                {
                    FirstName = "Bárbara",
                    LastName = "Fernandes",
                    Initials = "BF",
                    Address = new Domain.ValueObjects.Address("Massarelos", "4901", "Porto", "Portugal"),
                    Email = "barbara.fernandes@emailteste.pt",
                    Numbers = new List<ContactNumber>
                    {
                        new ContactNumber
                        {
                            CountryCode = "+333",
                            PhoneNumber = "333333333",
                            Type = Domain.Enums.ContactNumberType.WORK
                        },
                        new ContactNumber
                        {
                            CountryCode = "+351",
                            PhoneNumber = "444444444",
                            Type = Domain.Enums.ContactNumberType.MOBILE,
                            Default = true
                        }
                    }
                },
                //3
                new Contact
                {
                    FirstName = "Tiago",
                    LastName = "Carvalho",
                    Initials = "TC",
                    Address = new Domain.ValueObjects.Address("Figueira da Foz", "5678", "Coimbra", "Portugal"),
                    Email = "tiago.carvalho@emailteste.pt",
                    Numbers = new List<ContactNumber>
                    {
                        new ContactNumber
                        {
                            CountryCode = "+555",
                            PhoneNumber = "555555555",
                            Type = Domain.Enums.ContactNumberType.HOME
                        },
                        new ContactNumber
                        {
                            CountryCode = "+351",
                            PhoneNumber = "666666666",
                            Type = Domain.Enums.ContactNumberType.WORK,
                            Default = true
                        }
                    }
                },
                //4
                new Contact
                {
                    FirstName = "Rodrigo",
                    LastName = "Poiares",
                    Initials = "RP",
                    Address = new Domain.ValueObjects.Address("Nazaré", "4445", "Leiria", "Portugal"),
                    Email = "rodrigo.poiares@emailteste.pt",
                    Numbers = new List<ContactNumber>
                    {
                        new ContactNumber
                        {
                            CountryCode = "+777",
                            PhoneNumber = "777777777",
                            Type = Domain.Enums.ContactNumberType.MOBILE,
                            Default = true
                        }
                    }
                }
            };
            _context.Contacts.AddRange(contactsToAdd);
        }
    }

    private void SeedTestContactsWithNumbers()
    {
        if (_context.Contacts.Where(x => x.Active).Count() == 2)
        {
            var contactsToAdd = new List<Contact>();

            int numberOfContactsToCreate = 123;
            for (int i = 0; i < numberOfContactsToCreate; i++)
            {
                int idx = (i + 1);
                var phone = string.Format("{0:000}", idx);

                var c = new Contact();
                c.FirstName = $"Nome{idx}";
                c.LastName = $"Apelido{idx}";
                c.Initials = $"NA{idx}";
                c.Address = new Domain.ValueObjects.Address("Foz", "4900", "Porto", "Portugal");
                c.Email = $"na{idx}@teste.com";
                c.Numbers = new List<ContactNumber>
                {
                    new ContactNumber
                    {
                        CountryCode = "+351",
                        PhoneNumber = $"960000{phone}",
                        Type = Domain.Enums.ContactNumberType.MOBILE,
                        Default = true
                    }
                };
                contactsToAdd.Add(c);
            }

            _context.Contacts.AddRange(contactsToAdd);
        }
    }

    private void SeedContactGroups()
    {
        if (!_context.ContactGroups.Any(x => x.Active))
        {
            var groups = new List<ContactGroup>
            {
                new ContactGroup { Name = "Group A" },
                new ContactGroup { Name = "Group B" },
                new ContactGroup { Name = "Group C" }
            };
            _context.ContactGroups.AddRange(groups);
        }
    }

    private void SeedContactGroupContacts()
    {
        if (!_context.ContactGroupContacts.Any(x => x.Active))
        {
            #region Group A
            var groupA = _context.ContactGroups
                .FirstOrDefault(x => x.Name == "Group A");

            var contactsA = _context.Contacts
                .Where(x => x.Initials.Contains("AP") || x.Initials.Contains("BF"))
                .ToList();

            var groupContactsA = contactsA
                .Select(x => new ContactGroupContact { ContactId = x.Id, ContactGroupId = groupA.Id })
                .ToList();

            _context.ContactGroupContacts.AddRange(groupContactsA);
            #endregion

            #region Group B
            var groupB = _context.ContactGroups
                .FirstOrDefault(x => x.Name == "Group B");

            var contactsB = _context.Contacts
                .Where(x => x.Initials.Contains("TC"))
                .ToList();

            var groupContactsB = contactsB
                .Select(x => new ContactGroupContact { ContactId = x.Id, ContactGroupId = groupB.Id })
                .ToList();

            _context.ContactGroupContacts.AddRange(groupContactsB);
            #endregion

            #region Group C
            var groupC = _context.ContactGroups
                .FirstOrDefault(x => x.Name == "Group C");

            var contactsC = _context.Contacts
                .Where(x => x.Initials.Contains("RP"))
                .ToList();

            var groupContactsC = contactsC
                .Select(x => new ContactGroupContact { ContactId = x.Id, ContactGroupId = groupC.Id })
                .ToList();

            _context.ContactGroupContacts.AddRange(groupContactsC);
            #endregion
        }
    }


    public async Task TrySeedAsync()
    {
        // Default roles
        var administratorRole = new IdentityRole("Administrator");

        if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
        {
            await _roleManager.CreateAsync(administratorRole);
        }

        // Default users
        var administrator = new ApplicationUser { UserName = "administrator@localhost", Email = "administrator@localhost" };

        if (_userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            await _userManager.CreateAsync(administrator, "Administrator1!");
            await _userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
        }
    }


}