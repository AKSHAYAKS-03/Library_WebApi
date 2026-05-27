using ClosedXML.Excel;
using LibraryManagement.DTOs;
using LibraryManagement.Models;
using LibraryManagement.Service;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers;

[ApiController]
[Route("api/members")]
public class MembersController : ControllerBase
{
    //This is a Microsoft Excel .xlsx file
    private static readonly string ExcelMimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    private readonly IMemberService _memberService;

    public MembersController(IMemberService memberService)
    {
        _memberService = memberService;
    }

    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadMembers([FromForm] UploadMembersRequest request)
    {
        var excelFile = request.ExcelFile;

        if (excelFile is null || excelFile.Length == 0)
            return BadRequest(new { message = "Please upload a valid Excel file." });

        var extension = Path.GetExtension(excelFile.FileName).ToLowerInvariant();  //and returns everything after it including the dot.
        if (extension != ".xlsx")
            return BadRequest(new { message = "Invalid file type. Only .xlsx files are allowed." });

        var importedCount = 0;
        var errors = new List<string>();

        using var memoryStream = new MemoryStream(); //Excel file temporary RAM/memory stored
        await excelFile.CopyToAsync(memoryStream);
        memoryStream.Position = 0;

        using var workbook = new XLWorkbook(memoryStream); //actual opening of the file
        var worksheet = workbook.Worksheets.FirstOrDefault(); // getting the first worksheet
        if (worksheet is null)
            return BadRequest(new { message = "Excel file does not contain any worksheet." });

        var headerRow = worksheet.FirstRowUsed();
        if (headerRow is null)
            return BadRequest(new { message = "Excel file does not contain a header row." });

        var headerMap = BuildHeaderMap(headerRow);
        if (!headerMap.TryGetValue("fullname", out var fullNameCol) ||
            !headerMap.TryGetValue("email", out var emailCol) ||
            !headerMap.TryGetValue("phonenumber", out var phoneNumberCol) ||
            !headerMap.TryGetValue("membershipdate", out var membershipDateCol))
        {
            return BadRequest(new
            {
                message = "Excel headers must include FullName, Email, PhoneNumber, and MembershipDate."
            });
        }

        var ageCol = headerMap.TryGetValue("age", out var ageColumn) ? ageColumn : (int?)null;
        var firstDataRow = headerRow.RowNumber() + 1;
        var lastRow = worksheet.LastRowUsed()?.RowNumber() ?? headerRow.RowNumber(); 
        //Find the last used row number in the Excel sheet.  
        // If no used row exists, use the header row number instead.


        for (var rowNumber = firstDataRow; rowNumber <= lastRow; rowNumber++)
        {
            var row = worksheet.Row(rowNumber);
            if (IsRowEmpty(row))
                continue;

            try
            {
                var member = new Member
                {
                    //Cell Value Read
                    FullName = row.Cell(fullNameCol).GetString().Trim(),
                    Email = row.Cell(emailCol).GetString().Trim(),
                    PhoneNumber = row.Cell(phoneNumberCol).GetString().Trim(),
                    MembershipDate = ParseDate(row.Cell(membershipDateCol).GetString()),
                    Age = ageCol.HasValue ? ParseInt(row.Cell(ageCol.Value).GetString()) : null
                };

                await _memberService.AddMember(member);
                importedCount++;
            }
            catch (Exception ex) when (ex is ArgumentException or DuplicateMemberException)
            {
                errors.Add($"Row {rowNumber}: {ex.Message}");
            }
        }

        return Ok(new
        {
            message = "Excel file processed.",
            importedCount,
            failedCount = errors.Count,
            errors
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetAllMembers()
    {
        var members = await _memberService.GetAllMembers();
        return Ok(members.Select(MapToResponse));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetMemberById(int id)
    {
        var member = await _memberService.GetMemberById(id);
        if (member is null)
            return NotFound(new { message = "Member not found." });

        return Ok(MapToResponse(member));
    }

    [HttpGet("file")]
    public async Task<IActionResult> GetMembersFile()
    {
        var members = await _memberService.GetAllMembers();

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Members"); //Worksheet name

        //Headers
        worksheet.Cell(1, 1).Value = "MemberId";
        worksheet.Cell(1, 2).Value = "FullName";
        worksheet.Cell(1, 3).Value = "Email";
        worksheet.Cell(1, 4).Value = "PhoneNumber";
        worksheet.Cell(1, 5).Value = "Age";
        worksheet.Cell(1, 6).Value = "MembershipDate";

        var row = 2;
        foreach (var member in members)
        {
            worksheet.Cell(row, 1).Value = member.MemberId;
            worksheet.Cell(row, 2).Value = member.FullName;
            worksheet.Cell(row, 3).Value = member.Email;
            worksheet.Cell(row, 4).Value = member.PhoneNumber;
            worksheet.Cell(row, 5).Value = member.Age;
            worksheet.Cell(row, 6).Value = member.MembershipDate;
            row++;
        }

        //Auto fit
        worksheet.Columns().AdjustToContents();

        //Download
        using var output = new MemoryStream();  //like temporary memory box containing excel file data
        workbook.SaveAs(output);
        return File(output.ToArray(), ExcelMimeType, "members.xlsx");
    }

    private static MemberResponse MapToResponse(Member member)
    {
        return new MemberResponse
        {
            MemberId = member.MemberId,
            FullName = member.FullName,
            Email = member.Email,
            PhoneNumber = member.PhoneNumber,
            Age = member.Age,
            MembershipDate = member.MembershipDate
        };
    }

    private static Dictionary<string, int> BuildHeaderMap(IXLRow headerRow)
    {
        return headerRow.CellsUsed()
            .Where(cell => !string.IsNullOrWhiteSpace(cell.GetString()))
            .ToDictionary(
                cell => NormalizeHeader(cell.GetString()),
                cell => cell.Address.ColumnNumber);
    }

    private static string NormalizeHeader(string value)
    {
        return new string(value
            .Trim()
            .ToLowerInvariant()
            .Where(char.IsLetterOrDigit)
            .ToArray());
    }

    private static bool IsRowEmpty(IXLRow row)
    {
        return !row.CellsUsed().Any(cell => !string.IsNullOrWhiteSpace(cell.GetString()));
    }

    private static DateTime? ParseDate(string text)
    {
        if (DateTime.TryParse(text, out var parsed))
            return parsed;

        return null;
    }

    private static int? ParseInt(string text)
    {
        if (int.TryParse(text, out var parsed))
            return parsed;

        return null;
    }
}
