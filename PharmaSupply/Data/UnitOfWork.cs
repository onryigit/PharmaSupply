using Microsoft.EntityFrameworkCore.Storage;

namespace PharmaSupply.Data;

public interface IUnitOfWork
{
    AppDbContext Context { get; }
    Task ExecuteInTransactionAsync(Func<Task> operation);
}

public sealed class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public AppDbContext Context => context;

    public async Task ExecuteInTransactionAsync(Func<Task> operation)
    {
        await using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync();
        try
        {
            await operation();
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
