using Microsoft.EntityFrameworkCore;
using RoomBazar.Data.DbContexts;
using RoomBazar.Data.IRepositories;
using RoomBazar.Domain.Entites.Commons;

namespace RoomBazar.Data.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : Auditable
{
    private readonly AppDbContext appDbContext;
    private readonly DbSet<TEntity> dbSet;
    public Repository(AppDbContext appDbContext)
    {
        this.appDbContext = appDbContext;   
        this.dbSet = appDbContext.Set<TEntity>();
    }
    public async Task<TEntity> CreateAsync(TEntity entity)
    {
        await this.dbSet.AddAsync(entity);
        await this.appDbContext.SaveChangesAsync();

        return entity;
    }

    public IQueryable<TEntity> SelecAll() =>
        this.dbSet;
    

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        var result = (this.appDbContext.Update(entity)).Entity;

        await this.appDbContext.SaveChangesAsync();

        return result;
    }
}
