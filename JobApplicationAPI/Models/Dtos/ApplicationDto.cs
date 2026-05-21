namespace JobApplicationAPI.Models.Dtos;

public class ApplicationDto
{
    public int Id { get; set; }
    public int JobId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? CoverLetter { get; set; }
    public DateTime CreatedAt { get; set; }
}
