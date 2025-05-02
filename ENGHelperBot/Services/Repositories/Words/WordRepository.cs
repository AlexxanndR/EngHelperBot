using ENGHelperBot.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ENGHelperBot.Services.Repositories.Words;

public class WordRepository(IDbContextFactory<AppDbContext> contextFactory)
    : RepositoryBase<Word>(contextFactory), IWordRepository
{
}
