using LibraryManagement.Data;
using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Repository;

public class MemberRepository : IMemberRepository
{
    private readonly LibraryDbContext _context;

    public MemberRepository(LibraryDbContext context)
    {
        _context = context;
    }

    public async Task<Member> Add(Member member)
    {
        _context.Members.Add(member);
        await _context.SaveChangesAsync();
        return member;
    }

    public Task<List<Member>> GetAll()
    {
        return _context.Members
            .OrderBy(x => x.MemberId)
            .ToListAsync();
    }

    public Task<Member?> GetById(int memberId)
    {
        return _context.Members.FirstOrDefaultAsync(x => x.MemberId == memberId);
    }

    public Task<Member?> GetByEmail(string email)
    {
        return _context.Members.FirstOrDefaultAsync(x => x.Email == email);
    }

    public Task<Member?> GetByPhoneNumber(string phoneNumber)
    {
        return _context.Members.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber);
    }
}
