using ENGHelperBot.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ENGHelperBot.Services.Repositories.Dictionaries;

public class DictionaryRepository(IDbContextFactory<AppDbContext> contextFactory)
    : RepositoryBase<Dictionary>(contextFactory), IDictionaryRepository
{
}
