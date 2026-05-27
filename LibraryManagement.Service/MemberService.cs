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
        await EnsureMemberIsUnique(member);
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

        var hasData =
            !string.IsNullOrWhiteSpace(member.FullName) ||
            !string.IsNullOrWhiteSpace(member.Email) ||
            !string.IsNullOrWhiteSpace(member.PhoneNumber) ||
            member.MembershipDate.HasValue ||
            member.Age.HasValue;

        var hasFile = member.ExcelFileData is { Length: > 0 };

        if (!hasData && !hasFile)
            throw new ArgumentException("Provide either member data or an Excel file.");

        if (hasData)
        {
            if (string.IsNullOrWhiteSpace(member.FullName))
                throw new ArgumentException("Member full name should not be empty.");

            if (string.IsNullOrWhiteSpace(member.Email))
                throw new ArgumentException("Email should not be empty.");

            if (string.IsNullOrWhiteSpace(member.PhoneNumber))
                throw new ArgumentException("Phone number should not be empty.");

            if (!member.MembershipDate.HasValue)
                throw new ArgumentException("Membership date should not be empty.");
        }

        if (hasFile)
        {
            if (string.IsNullOrWhiteSpace(member.ExcelFileName))
                throw new ArgumentException("Excel file name should not be empty.");
        }
    }

    private async Task EnsureMemberIsUnique(Member member)
    {
        if (!string.IsNullOrWhiteSpace(member.Email))
        {
            var existingByEmail = await _memberRepository.GetByEmail(member.Email.Trim());
            if (existingByEmail is not null)
                throw new DuplicateMemberException($"A member with email '{member.Email}' already exists.");
        }

        if (!string.IsNullOrWhiteSpace(member.PhoneNumber))
        {
            var existingByPhone = await _memberRepository.GetByPhoneNumber(member.PhoneNumber.Trim());
            if (existingByPhone is not null)
                throw new DuplicateMemberException($"A member with phone number '{member.PhoneNumber}' already exists.");
        }
    }
}
