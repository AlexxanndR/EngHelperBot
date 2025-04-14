using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ENGHelperBot.Data.Entities;

[Table("words")]
public class Word
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Required]
    [StringLength(400)]
    [Column("text")]
    public string Text { get; set; } 

    [StringLength(200)]
    [Column("transcription")]
    public string? Transcription { get; set; }

    public virtual ICollection<WordTranslation> WordTranslations { get; set; } = new HashSet<WordTranslation>();
}
