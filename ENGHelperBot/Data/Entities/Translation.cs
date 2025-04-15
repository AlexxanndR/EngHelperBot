using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ENGHelperBot.Data.Entities;

[Table("translation")]
public class Translation
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Required]
    [StringLength(400)]
    [Column("text")]
    public string Text { get; set; }

    public virtual ICollection<WordTranslation> WordTranslations { get; set; } = new HashSet<WordTranslation>();
}
