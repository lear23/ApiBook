
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Dtos;

public class QuoteDto
{
    [Required]
    public string Text { get; set; } = string.Empty;

    [Required]
    public string Author { get; set; }= string.Empty;

    [Required]
    public Guid ClientId { get; set; }
}
