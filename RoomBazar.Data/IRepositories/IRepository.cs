namespace RoomBazar.Data.IRepositories;

public interface IRepository<TEntity>
{
    Task<TEntity> CreateAsync(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity);

    IQueryable<TEntity> SelecAll();
}
