using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bib.Models
{
    public class UserAdd
    { 
        public required string Name { get; set; }
        public required string Prenome { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string ConfirmePassword { get; set; }
        public string Role { get; set; } = "user";
    }
}
