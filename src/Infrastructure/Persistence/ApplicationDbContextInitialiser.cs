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

    private void SeedContactsAndNumbers()
    {
        if (!_context.Contacts.Any(x => x.Active))
        {
            var contactsToAdd = new List<Contact>
            {
                //1
                new Contact
                {
                    FirstName = "José",
                    LastName = "Pereira",
                    Initials = "JP",
                    Address = new Domain.ValueObjects.Address("Nogueira", "4900", "Viana do Castelo", "Portugal"),
                    Email = "jdiogopereira93@gmail.com",
                    Numbers = new List<ContactNumber>
                    {
                        new ContactNumber
                        {
                            CountryCode = "+351",
                            PhoneNumber = "258000000",
                            Type = Domain.Enums.ContactNumberType.HOME
                        },
                        new ContactNumber
                        {
                            CountryCode = "+48",
                            PhoneNumber = "0011223344",
                            Type = Domain.Enums.ContactNumberType.MOBILE,
                            Default = true
                        }
                    }
                },
                //2
                new Contact
                {
                    FirstName = "Catarina",
                    LastName = "Pereira",
                    Initials = "CP",
                    Address = new Domain.ValueObjects.Address("Massarelos", "4901", "Porto", "Portugal"),
                    Email = "jde93pereira@gmail.com"
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
                        PhoneNumber = $"960000{idx}",
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
            var firstGroup = _context.ContactGroups
                .FirstOrDefault(x => x.Name == "Group A");

            var contacts = _context.Contacts
                .Where(x => x.Initials.Contains("JP") || x.Initials.Contains("CP"))
                .ToList();

            var groupContacts = contacts
                .Select(x => new ContactGroupContact { ContactId = x.Id, ContactGroupId = firstGroup.Id })
                .ToList();

            _context.ContactGroupContacts.AddRange(groupContacts);
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

        // Default data
        // Seed, if necessary
        if (!_context.TodoLists.Any())
        {
            _context.TodoLists.Add(new TodoList
            {
                Title = "Todo List",
                Items =
                {
                    new TodoItem { Title = "Make a todo list 📃" },
                    new TodoItem { Title = "Check off the first item ✅" },
                    new TodoItem { Title = "Realise you've already done two things on the list! 🤯"},
                    new TodoItem { Title = "Reward yourself with a nice, long nap 🏆" },
                }
            });

            await _context.SaveChangesAsync();
        }
    }

}