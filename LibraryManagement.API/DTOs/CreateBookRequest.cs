using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.DTOs;

public class CreateBookRequest
{
    [Required(ErrorMessage = "Book title should not be empty.")]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Author name should not be empty.")]
    [StringLength(150)]
    public string Author { get; set; } = string.Empty;

    [Required(ErrorMessage = "ISBN should not be empty.")]
    [StringLength(20)]
    public string ISBN { get; set; } = string.Empty;

    [Range(1, 9999)]
    public int PublishedYear { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Available copies should be greater than or equal to 0.")]
    public int AvailableCopies { get; set; }
}
