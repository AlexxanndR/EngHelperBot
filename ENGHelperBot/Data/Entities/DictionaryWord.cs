using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ENGHelperBot.Data.Entities;

[Table("dictionaries_words")]
public class DictionaryWord
{
    [Key]
    [Column("dictionary_id", Order = 1)]
    public long DictionaryId { get; set; }
    public virtual Dictionary Dictionary { get; set; }

    [Key]
    [Column("word_id", Order = 2)]
    public long WordId { get; set; }
    public virtual Word Word { get; set; }
}
