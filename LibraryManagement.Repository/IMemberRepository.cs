using LibraryManagement.Models;

namespace LibraryManagement.Repository;

public interface IMemberRepository
{
    Task<Member> Add(Member member);
    Task<List<Member>> GetAll();
    Task<Member?> GetById(int memberId);
}
