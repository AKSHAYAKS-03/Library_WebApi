using Microsoft.AspNetCore.Http;

namespace LibraryManagement.DTOs;

public class UploadMembersRequest
{
    public IFormFile? ExcelFile { get; set; }
}
