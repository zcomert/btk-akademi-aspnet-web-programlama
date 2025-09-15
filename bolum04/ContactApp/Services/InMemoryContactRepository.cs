using ContactApp.Models;

namespace ContactApp.Services;

public class InMemoryContactRepository : IContactRepository
{
    private readonly List<Contact> _contacts;
    private int _nextId = 1;
    public InMemoryContactRepository()
    {
        _contacts = new List<Contact>();

        // seed data
        var seed = new List<Contact>()
        {
            new Contact(){FirstName="Ahmet", LastName = "Yılmaz", Email="ahmet.yilmaz@example.com", Phone = "+905551112233", Company = "BTK Akademi", Title="Yazılım Geliştirme Uzmanı", Notes = ".NET" },
            new() { FirstName = "Ayşe", LastName = "Demir", Email = "ayse.demir@example.com", Phone = "+905554445566", Company = "XYZ", Title = "Analist", Notes = "CRM" },
            new() { FirstName = "Mehmet", LastName = "Kaya", Email = "mehmet.kaya@example.com", Phone = "+905559991122", Company = "TechCo", Title = "Takım Lideri" },
            new() { FirstName = "Elif", LastName = "Çetin", Email = "elif.cetin@example.com", Phone = "+905551234567", Company = "SoftWorks", Title = "QA" },
            new() { FirstName = "Can", LastName = "Aydın", Email = "can.aydin@example.com", Phone = "+905558765432", Company = "Innova", Title = "Stajyer" },
            new() { FirstName = "Zeynep", LastName = "Bulut", Email = "zeynep.bulut@example.com" },
            new() { FirstName = "Emre", LastName = "Arslan", Email = "emre.arslan@example.com" },
            new() { FirstName = "Selin", LastName = "Koç", Email = "selin.koc@example.com" }
        };

        foreach(var c in seed)
        {
            c.Id = _nextId++;
            _contacts.Add(c);
        }
    }
    public Contact Add(Contact contact)
    {
        contact.Id = _nextId++;
        _contacts.Add(contact);
        return contact;
    }

    public bool Delete(int id)
    {
        var existing = GetById(id);
        if (existing is null) 
            return false;
        _contacts.Remove(existing);
        return true;
    }

    public IEnumerable<Contact> GetAll() =>
        _contacts
            .OrderBy(c => c.LastName)
            .ThenBy(c => c.FirstName);
    

    public Contact? GetById(int id) => 
        _contacts.FirstOrDefault(c => c.Id.Equals(id));
   

    public bool Update(Contact contact)
    {
        var existing = GetById(contact.Id);
        
        if (existing is null) 
            return false;

        existing.FirstName = contact.FirstName;
        existing.LastName = contact.LastName;
        existing.Email = contact.Email;
        existing.Phone = contact.Phone;
        existing.Company = contact.Company;
        existing.Title = contact.Title;
        existing.Notes = contact.Notes;
        return true;
    }
}
