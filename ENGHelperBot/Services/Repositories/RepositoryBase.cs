using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ENGHelperBot.Services.Repositories;

public abstract class RepositoryBase<T>(IDbContextFactory<AppDbContext> contextFactory) 
    : IRepositoryBase<T> where T : class
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory = contextFactory;

    public async ValueTask CreateAsync(T entity, Expression<Func<T, bool>> existenceExpression, CancellationToken cancellationToken)
    {
        await using var databaseContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
        await using var transaction = await databaseContext.Database.BeginTransactionAsync(cancellationToken);

        var exists = await databaseContext.Set<T>().AnyAsync(existenceExpression, cancellationToken);
        if (exists) return;

        await databaseContext.Set<T>().AddAsync(entity, cancellationToken);
        await databaseContext.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
    }

    public async ValueTask UpdateAsync(T entity, CancellationToken cancellationToken)
    {
        await using var databaseContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
        databaseContext.Set<T>().Update(entity);
    }

    public async ValueTask DeleteAsync(T entity, CancellationToken cancellationToken)
    {
        await using var databaseContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
        databaseContext.Set<T>().Remove(entity);
    }

    public async ValueTask<T?> FindAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken)
    {
        await using var databaseContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
        return await databaseContext.Set<T>().FirstOrDefaultAsync(expression, cancellationToken);
    }
}
