using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ENGHelperBot.Data.Entities;

[Table("users")]
public class User
{
    [Key]
    public long Id { get; set; }

    [StringLength(50)] 
    [Column("username")]
    public string? Username { get; set; }

    public virtual ICollection<Dictionary> Dictionaries { get; set; }
}
