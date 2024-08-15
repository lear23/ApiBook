

using System.Globalization;

namespace Infrastructure.Entities
{
    public class BookEntity
    {

        public Guid Id { get; set; } = Guid.NewGuid();

        public string Title { get; set; } = string.Empty;
        public string? Author { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime? PublicationDate { get; set; }
        public string? ImageName { get; set; }
    }
}
