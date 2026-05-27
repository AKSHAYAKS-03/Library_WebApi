namespace LibraryManagement.Service;

public sealed class DuplicateMemberException : Exception
{
    public DuplicateMemberException(string message) : base(message)
    {
    }
}
