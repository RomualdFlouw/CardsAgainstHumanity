using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CardsAgainstHumanity.Models.Repositories
{
    public interface IGenericRepo<TEntity> where TEntity : class
    {
        CardsAgainstHumanityAPIContext _context { get; set; }

        Task Create(TEntity entity);
        Task Delete(TEntity entity);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetByExpressionAsync(Expression<Func<TEntity, bool>> expression);
        Task SaveAsync();
        Task Update(TEntity entity);
    }
}