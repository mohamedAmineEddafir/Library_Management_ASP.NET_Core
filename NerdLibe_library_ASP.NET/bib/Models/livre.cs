using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bib.Models
{
    public class Livre
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string Titre { get; set; }
        public required string Auteur { get; set; }
        public required string Discription { get; set; }
        public string imageName { get; set; }
        public required DateTime DatePube { get; set; }

    }
}
