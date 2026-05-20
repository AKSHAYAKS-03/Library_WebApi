using LibraryManagement.DTOs;
using LibraryManagement.Models;
using LibraryManagement.Service;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers;

[ApiController]
[Route("api/members")]
public class MembersController : ControllerBase
{
    private readonly IMemberService _memberService;

    public MembersController(IMemberService memberService)
    {
        _memberService = memberService;
    }

    [HttpPost]
    public async Task<IActionResult> AddMember([FromBody] CreateMemberRequest request)
    {
        try
        {
            var member = new Member
            {
                FullName = request.FullName?.Trim() ?? string.Empty,
                Email = request.Email?.Trim() ?? string.Empty,
                PhoneNumber = request.PhoneNumber?.Trim() ?? string.Empty,
                MembershipDate = request.MembershipDate
            };

            var created = await _memberService.AddMember(member);
            return CreatedAtAction(nameof(GetMemberById), new { id = created.MemberId }, new { message = "Member added successfully" });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllMembers()
    {
        var members = await _memberService.GetAllMembers();
        return Ok(members);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetMemberById(int id)
    {
        var member = await _memberService.GetMemberById(id);
        if (member is null)
            return NotFound(new { message = "Member not found." });

        return Ok(member);
    }
}

