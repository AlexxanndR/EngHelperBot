using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ENGHelperBot.Services.Repositories;

public abstract class RepositoryBase<T>(IDbContextFactory<AppDbContext> contextFactory)
    : IRepositoryBase<T> where T : class
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory = contextFactory;

    public async ValueTask<bool> CreateAsync(T entity, Expression<Func<T, bool>> existenceExpression, CancellationToken cancellationToken = default)
    {
        await using var databaseContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
        await using var transaction = await databaseContext.Database.BeginTransactionAsync(cancellationToken);

        if (await databaseContext.Set<T>().AnyAsync(existenceExpression, cancellationToken))
            return false;

        await databaseContext.Set<T>().AddAsync(entity, cancellationToken);
        await databaseContext.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return true;
    }

    public async ValueTask UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        await using var databaseContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
        databaseContext.Set<T>().Update(entity);
    }

    public async ValueTask DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        await using var databaseContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
        databaseContext.Set<T>().Remove(entity);
    }

    public async ValueTask<T?> FindAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default)
    {
        await using var databaseContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
        return await databaseContext.Set<T>().FirstOrDefaultAsync(expression, cancellationToken);
    }

    public async ValueTask<(IEnumerable<T> Data, int TotalPages)> GetByPageAsync(int pageNumber, int pageSize = 5, CancellationToken cancellationToken = default)
    {
        await using var databaseContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
         
        int totalRecords = await databaseContext.Set<T>().CountAsync(cancellationToken);
        int totalPages = (totalRecords + pageSize - 1) / pageSize;
        
        int skip = (pageNumber - 1) * pageSize;
        var data = await databaseContext.Set<T>()
                                        .Skip(skip)
                                        .Take(pageSize)
                                        .AsNoTracking()
                                        .ToListAsync(cancellationToken);

        return (data, totalPages);
    }
}
