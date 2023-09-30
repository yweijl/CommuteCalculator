using AutoMapper;
using Infrastructure.CosmosDb;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public abstract class GenericRepository
{
    private readonly CommuteCalculatorContext _context;
    protected readonly IMapper _mapper;

    public GenericRepository(CommuteCalculatorContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    protected async Task<TEntity> AddAsync<TEntity>(TEntity tEntity) where TEntity : EntityBase
    {
        var entityEntry = await _context.Set<TEntity>().AddAsync(tEntity);
        await _context.SaveChangesAsync();
        return entityEntry.Entity;
    }

    protected async Task AddRangeAsync<TEntity>(List<TEntity> tEntities) where TEntity : EntityBase
    {
        await _context.Set<TEntity>().AddRangeAsync(tEntities);
    }

    protected async Task<TEntity> SingleAsync<TEntity>(Guid id, bool asNoTracking = true) where TEntity : EntityBase
    {
        try
        {
            var query = _context.Set<TEntity>().WithPartitionKey(id.ToString());
            if (!asNoTracking)
            {
                query.AsNoTracking();
            }
                
            var entity = await query.SingleAsync();
            return entity;
        }
        catch (InvalidOperationException)
        {
            throw new KeyNotFoundException($"Could not fint entity with id: {id}");
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected async Task<TEntity?> SingleOrDefaultAsync<TEntity>(Guid id, bool asNoTracking = true) where TEntity : EntityBase
    {
        try
        {
            var query = _context.Set<TEntity>().WithPartitionKey(id.ToString());
            if (!asNoTracking)
            {
                query.AsNoTracking();
            }

            var entity = await query.SingleOrDefaultAsync();
            return entity;
        }
        catch (InvalidOperationException)
        {
            return null;
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected async Task<TEntity?> SingleOrDefaultAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : EntityBase
    {
        try
        {
            var contact = await _context.Set<TEntity>().AsNoTracking().SingleOrDefaultAsync(predicate);
            return contact;
        }
        catch (InvalidOperationException)
        {
            return null;
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected async Task<bool> DeleteAsync<TEntity>(Guid id) where TEntity : EntityBase
    {
        try
        {
            var entity = await SingleAsync<TEntity>(id, false);
            _context.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException)
        {
            throw;
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected Task<List<TEntity>> ListAsync<TEntity>() where TEntity : EntityBase
    {
        return _context.Set<TEntity>().AsNoTracking().ToListAsync();
    }

    protected Task<TEntity> SingleAsync<TEntity>(Expression<Func<TEntity, bool>> predicate, bool isTracking = false) where TEntity : EntityBase
    {
        return isTracking
            ? _context.Set<TEntity>().SingleAsync(predicate)
            : _context.Set<TEntity>().AsNoTracking().SingleAsync(predicate);
    }

    protected Task<List<TEntity>> ListAsync<TEntity>(Expression<Func<TEntity, bool>> predicate, bool isTracking = false) where TEntity : EntityBase
    {
        return isTracking 
            ? _context.Set<TEntity>().Where(predicate).ToListAsync()
            : _context.Set<TEntity>().AsNoTracking().Where(predicate).ToListAsync();
    }

    protected void UpdateRange<TEntity>(List<TEntity> entities) where TEntity : EntityBase
    {
        _context.UpdateRange(entities);
    }

    protected Task SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }

    protected async Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : EntityBase
    {
        var count = await _context.Set<TEntity>().AsNoTracking().CountAsync(predicate);
        return count != 0;
    }
}