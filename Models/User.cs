using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class User
{
    [Key]
    public string StudentId { get; set; }
    public string FullName { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public int RoleId { get; set; }
    [ForeignKey("Club")]
    public int ClubId { get; set; }
    public Club Club { get; set; }
    public bool Status { get; set; } = true;
}
