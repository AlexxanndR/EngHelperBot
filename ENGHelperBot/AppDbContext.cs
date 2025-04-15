using ENGHelperBot.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ENGHelperBot;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Word> Words { get; set; }
    public DbSet<Translation> Translations { get; set; }
    public DbSet<WordTranslation> WordsTranslations { get; set; }
    public DbSet<Dictionary> Dictionaries { get; set; }
    public DbSet<DictionaryWord> DictionariesWords { get; set; }
    public DbSet<Phrase> Phrases { get; set; }
    public DbSet<Example> Examples { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DictionaryWord>(entity =>
        {
            entity.HasKey(dw => new { dw.DictionaryId, dw.WordId });
            entity.Property(dw => dw.DictionaryId).HasColumnName("dictionary_id");
            entity.Property(dw => dw.WordId).HasColumnName("word_id");
        });

        modelBuilder.Entity<WordTranslation>(entity =>
        {
            entity.HasKey(wt => new { wt.WordId, wt.TranslationId });
            entity.Property(wt => wt.WordId).HasColumnName("word_id");
            entity.Property(wt => wt.TranslationId).HasColumnName("translation_id");
        });
    }
}
