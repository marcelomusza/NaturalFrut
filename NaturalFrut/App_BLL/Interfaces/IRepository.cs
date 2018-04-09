using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace NaturalFrut.App_BLL.Interfaces
{
    public interface IRepository<T> : IDisposable where T : class, IEntity, new()
    {

        IQueryable<T> GetAll();
        T GetByID(int id);
        IQueryable<T> GetBy(Expression<Func<T, bool>> predicate);

        void Add(T entity);
        void Delete(T entity);
        void Update(T entity);
        void Save();

    }
}