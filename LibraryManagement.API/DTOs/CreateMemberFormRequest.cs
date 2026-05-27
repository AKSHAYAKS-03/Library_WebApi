using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace LibraryManagement.DTOs;

public class CreateMemberFormRequest
{
    [StringLength(150)]
    public string? FullName { get; set; }

    [EmailAddress]
    [StringLength(255)]
    public string? Email { get; set; }

    [StringLength(10, MinimumLength = 10)]
    [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Phone number must be 10 digits starting with 6-9.")]
    public string? PhoneNumber { get; set; }

    public DateTime? MembershipDate { get; set; }

    public int? Age { get; set; }

    public IFormFile? ExcelFile { get; set; }
}
