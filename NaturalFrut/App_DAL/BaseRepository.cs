using NaturalFrut.App_BLL.Interfaces;
using NaturalFrut.Models;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Data.Entity.Validation;

namespace NaturalFrut.App_DAL
{
    public class BaseRepository<T> : IRepository<T> where T : class, IEntity, new()
    {

        readonly ApplicationDbContext _context;

        public BaseRepository()
        {
            _context = new ApplicationDbContext();
        }


        public IQueryable<T> GetAll()
        {
            return _context.Set<T>();
        }

        public T GetByID(int id)
        {
            return GetAll().FirstOrDefault(x => x.ID == id);
        }

        public IQueryable<T> GetBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate);
        }

        public void Add(T entity)
        {
            DbEntityEntry dbEntityEntry = _context.Entry<T>(entity);
            _context.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            DbEntityEntry dbEntityEntry = _context.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Deleted;
        }

        public void Update(T entity)
        {
            DbEntityEntry dbEntityEntry = _context.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Modified;
        }

        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                //PARA EDITAR MAS ADELANTE
                throw;
            }
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                    _context.Dispose();
            }
        }

    }
}