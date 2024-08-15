

namespace Infrastructure.Entities
{
    public class QuoteEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid(); 

        public string Text { get; set; } = string.Empty; 

        public string Author { get; set; } = string.Empty;
  
        public Guid ClientId { get; set; }

        public ClientEntity Client { get; set; } = null!;
    }
}