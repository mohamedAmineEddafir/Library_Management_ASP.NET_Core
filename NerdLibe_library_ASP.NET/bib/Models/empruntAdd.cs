namespace bib.Models
{
    public class empruntAdd
    {
        public int Id { get; set; }
        public required string Cin { get; set; }
        public required string Liver { get; set; }
        public required string NameEmprunt { get; set; }
        public required string Telephone { get; set; }
        public required DateTime DateEmprunt { get; set; }
        public required DateTime DateRetoure { get; set; }
    }
}
