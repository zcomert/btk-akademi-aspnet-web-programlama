namespace ContactApp.Models;
public class Contact
{
    public int Id { get; set; }
    public String FirstName { get; set; }
    public String LastName { get; set; }
    public String? Email { get; set; }
    public String? Phone { get; set; }
    public String? Company { get; set; }
    public String? Title { get; set; }
    public String? Notes { get; set; }
}
