namespace Lalena.Database.Entities
{
    public class Fout
    {
        public int Id { get; set; }
        
        public string Opgave { get; set; } = null!;
        public int CorrectAntwoord { get; set; }
        public int IngevuldAntwoord { get; set; }
    }
}