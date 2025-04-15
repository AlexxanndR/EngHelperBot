using ENGHelperBot.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ENGHelperBot.Services.Repositories.Users;

public class UserRepository(IDbContextFactory<AppDbContext> contextFactory)
    : RepositoryBase<User>(contextFactory), IUserRepository
{
}
