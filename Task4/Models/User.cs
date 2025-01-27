using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task4.Models
{
    [Table("Users")]
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; } 
        public string PasswordHash { get; set; } 
        public bool IsBlocked { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public DateTime RegistrationTime { get; set; } = DateTime.UtcNow;
    }
}