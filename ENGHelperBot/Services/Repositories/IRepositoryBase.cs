using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace ENGHelperBot.Services.Repositories;

public interface IRepositoryBase<T>
{
    ValueTask<bool> CreateAsync(T entity, Expression<Func<T, bool>> existenceExpression, CancellationToken cancellationToken = default);
    ValueTask UpdateAsync(T entity, CancellationToken cancellationToken = default);
    ValueTask DeleteAsync(T entity, CancellationToken cancellationToken = default);
    ValueTask<T?> FindAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default);
    ValueTask<bool> AnyAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default);
    ValueTask<(IEnumerable<T> Data, int TotalPages)> GetByPageAsync(int pageNumber, int pageSize = 5, CancellationToken cancellationToken = default);
}
