namespace bib.Models
{
    public class eventeAdd
    {
        public required string Titre { get; set; }
        public required string localisation { get; set; }
        public required string Discription { get; set; }
        public IFormFile imageFile { get; set; }
        public required DateTime date { get; set; }
    }
}
