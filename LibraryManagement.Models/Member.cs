using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models;

public class Member
{
    public int MemberId { get; set; }

    [Required(ErrorMessage = "Member full name should not be empty.")]
    [StringLength(150)]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email should not be empty.")]
    [EmailAddress]
    [StringLength(255)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone number should not be empty.")]
    [StringLength(10, MinimumLength = 10)]
    [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Phone number must be 10 digits starting with 6-9.")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    public DateTime MembershipDate { get; set; }
}
