﻿

namespace Infrastructure.Dtos;

public class BookDto
{

    public string Title { get; set; } = string.Empty;
    public string? Author { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? PublicationDate { get; set; }
    public string? ImageName { get; set; }
}
