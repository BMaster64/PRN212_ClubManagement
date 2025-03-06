using System.ComponentModel.DataAnnotations;

public class User
{
    [Key]
    public string StudentId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public int UserType { get; set; }
    public int ClubId { get; set; }
    public bool IsActive { get; set; } = true;
}
