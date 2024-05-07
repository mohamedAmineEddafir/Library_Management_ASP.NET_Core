using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bib.Models
{
    public class Utilisateur
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Prenome { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string ConfirmePassword { get; set; }
        public string Role { get; set; } = "user";

    }
}
