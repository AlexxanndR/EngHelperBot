using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ENGHelperBot.Data.Entities;

[Table("phrases")]
public class Phrase
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [ForeignKey("word_id")]
    public long WordId { get; set; }

    [Required]
    [StringLength(500)]
    [Column("original")]
    public string Original { get; set; }

    [Required]
    [StringLength(500)]
    [Column("translation")]
    public string Translation { get; set; }
}
