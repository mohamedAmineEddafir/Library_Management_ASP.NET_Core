namespace bib.Models
{
    public class livreAdd
    {
        public required string Titre { get; set; }
        public required string Auteur { get; set; }
        public required string Discription { get; set; }
        public IFormFile imageFile { get; set; }
        public required DateTime DatePube { get; set; }

    }
}
