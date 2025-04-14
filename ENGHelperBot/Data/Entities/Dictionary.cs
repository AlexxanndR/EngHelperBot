using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ENGHelperBot.Data.Entities;

[Table("dictionaries")]
public class Dictionary
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    
    [Required]
    [StringLength(50)] 
    [Column("name")] 
    public string Name { get; set; }

    [ForeignKey("user_id")]
    public long UserId { get; set; }
    public virtual User User { get; set; }

}
