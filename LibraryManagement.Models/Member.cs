using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models;

public class Member
{
    public int MemberId { get; set; }

    [StringLength(150)]
    public string? FullName { get; set; }

    [EmailAddress]
    [StringLength(255)]
    public string? Email { get; set; }

    [StringLength(10, MinimumLength = 10)]
    [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Phone number must be 10 digits starting with 6-9.")]
    public string? PhoneNumber { get; set; }

    public int? Age { get; set; }

    public DateTime? MembershipDate { get; set; }

    public string? ExcelFileName { get; set; }

    public string? ExcelContentType { get; set; }

    public byte[]? ExcelFileData { get; set; }
}
