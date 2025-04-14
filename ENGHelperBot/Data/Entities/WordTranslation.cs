using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ENGHelperBot.Data.Entities;

[Table("words_translations")]
public class WordTranslation
{
    [Key]
    [Column("word_id", Order = 1)]
    public long WordId { get; set; }
    public virtual Word Word { get; set; }

    [Key]
    [Column("translation_id", Order = 2)]
    public long TranslationId { get; set; }
    public virtual Translation Translation { get; set; }
}
