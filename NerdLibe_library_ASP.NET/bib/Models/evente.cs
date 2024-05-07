using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace bib.Models
{
    public class Evente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string Titre { get; set; }
        public required string localisation { get; set; }
        public required string Discription { get; set; }
        public string imageName { get; set; }
        public required DateTime date { get; set; }

    }
}
