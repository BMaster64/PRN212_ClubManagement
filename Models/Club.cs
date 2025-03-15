using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Club
{
    [Key]
    public int ClubId { get; set; }
    [Required]
    public string ClubName { get; set; }
    public string Description { get; set; }

    public static Club CreateNewClub(string clubName, string description)
    {
        return new Club
        {
            ClubName = clubName,
            Description = description
        };
    }
}