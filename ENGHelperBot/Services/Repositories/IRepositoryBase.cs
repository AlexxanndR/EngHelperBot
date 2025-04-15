using Microsoft.EntityFrameworkCore.Internal;
using System.Linq.Expressions;

namespace ENGHelperBot.Services.Repositories;

public interface IRepositoryBase<T>
{
    ValueTask CreateAsync(T entity, Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default);
    ValueTask UpdateAsync(T entity, CancellationToken cancellationToken = default);
    ValueTask DeleteAsync(T entity, CancellationToken cancellationToken = default);
    ValueTask<T?> FindAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default);
}
