using LibraryManagement.Models;
using LibraryManagement.Repository;

namespace LibraryManagement.Service;

public class MemberService : IMemberService
{
    private readonly IMemberRepository _memberRepository;

    public MemberService(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<Member> AddMember(Member member)
    {
        ValidateMember(member);
        return await _memberRepository.Add(member);
    }


    public Task<List<Member>> GetAllMembers()
    {
        return _memberRepository.GetAll();
    }

    public Task<Member?> GetMemberById(int memberId)
    {
        return _memberRepository.GetById(memberId);
    }

    private static void ValidateMember(Member member)
    {
        if (member is null)
            throw new ArgumentNullException(nameof(member));

        if (string.IsNullOrWhiteSpace(member.FullName))
            throw new ArgumentException("Member full name should not be empty.");

        if (string.IsNullOrWhiteSpace(member.Email))
            throw new ArgumentException("Email should not be empty.");

        if (string.IsNullOrWhiteSpace(member.PhoneNumber))
            throw new ArgumentException("Phone number should not be empty.");
    }
}
