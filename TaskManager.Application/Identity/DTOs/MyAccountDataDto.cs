namespace TaskManager.Application.Identity.DTOs;

public sealed class MyAccountDataDto
{
    public string? Email { get; set; }
    public string? UserName { get; set; }
    public double FilesSize { get; set; }
}