using ContactApp.Models;
using ContactApp.Services;
using Microsoft.EntityFrameworkCore;

namespace ContactApp.Repositories
{
    public class EfContactRepository : IContactRepository
    {
        private readonly ApplicationDbContext _db;

        public EfContactRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public Contact Add(Contact contact)
        {
            _db.Contacts.Add(contact);
            _db.SaveChanges();
            return contact;
        }

        public bool Delete(int id)
        {
            var existing = _db.Contacts.Find(id);
            if(existing is null)
                return false;
            _db.Contacts.Remove(existing);
            _db.SaveChanges();
            return true;
        }

        public IEnumerable<Contact> GetAll() => 
            _db.Contacts
                .AsNoTracking()
                .OrderBy(c => c.LastName)    
                .ThenBy(c => c.FirstName)
                .ToList();


        public Contact? GetById(int id) =>
            _db.Contacts
                .AsNoTracking()
                .FirstOrDefault(c => c.Id.Equals(id));
       

        public bool Update(Contact contact)
        {
            var existing = _db.Contacts.FirstOrDefault(c => c.Id.Equals(contact.Id));
            if(existing is null)
                return false;
            
            existing.FirstName = contact.FirstName;
            existing.LastName = contact.LastName;
            existing.Email = contact.Email;
            existing.Phone = contact.Phone;
            existing.Company = contact.Company;
            existing.Title = contact.Title;
            existing.Notes = contact.Notes;
            
            _db.SaveChanges();
            return true;
        }
    }
}
