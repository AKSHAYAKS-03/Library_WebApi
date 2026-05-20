using LibraryManagement.Models;

namespace LibraryManagement.Service;

public interface IMemberService
{
    Task<Member> AddMember(Member member);
    Task<List<Member>> GetAllMembers();
    Task<Member?> GetMemberById(int memberId);
}
