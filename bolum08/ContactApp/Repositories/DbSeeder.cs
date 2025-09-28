using ContactApp.Models;

namespace ContactApp.Repositories
{
    public static class DbSeeder
    {
        public static void Seed(ContactDbContext db)
        {
            if (db.Contacts.Any())
                return;

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

            db.Contacts.AddRange(seed);
            db.SaveChanges();
        }
    }
}
