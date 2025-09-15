using ContactApp.Models;

namespace ContactApp.Services;
public interface IContactRepository
{
    IEnumerable<Contact> GetAll();
    Contact? GetById(int id);
    Contact Add(Contact contact);
    bool Update(Contact contact);
    bool Delete(int id);
}
