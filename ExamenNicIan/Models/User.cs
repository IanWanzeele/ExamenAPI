using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ExamenNicIan.Models
{
    public class User
    {
        public int UserId { get; set; }
        [DisplayName("First name")]
        public string FirstName { get; set; }
        [DisplayName("Last name")]
        public string LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [PasswordPropertyText]
        public string Password { get; set; }
    }
}
