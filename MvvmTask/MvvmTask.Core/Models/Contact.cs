
namespace MvvmTask.Core.Models
{
    public class Contact
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public System.DateTime Updated { get; set; }

        public Contact(int id, string name, string phone, string email)
        {
            ID = id;
            Name = name;
            Phone = phone;
            Email = email;
            Updated = System.DateTime.Now;
        }
    }
}
