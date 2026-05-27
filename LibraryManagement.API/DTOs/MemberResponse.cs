namespace LibraryManagement.DTOs;

public class MemberResponse
{
    public int MemberId { get; set; }

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public int? Age { get; set; }

    public DateTime? MembershipDate { get; set; }
}
