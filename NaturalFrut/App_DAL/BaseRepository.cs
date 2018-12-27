using NaturalFrut.App_BLL.Interfaces;
using NaturalFrut.Models;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Data.Entity.Validation;
using System.Diagnostics;
using log4net;

namespace NaturalFrut.App_DAL
{
    public class BaseRepository<T> : IRepository<T> where T : class, IEntity, new()
    {

        readonly ApplicationDbContext _context;
        internal DbSet dbSet;

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public BaseRepository()
        {
            _context = new ApplicationDbContext();
        }

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
            dbSet = _context.Set<T>();
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

                log.Info("Datos salvados en la base de datos satisfactoriamente.");

            }
            catch (DbEntityValidationException e)
            {
                foreach (var validationErrors in e.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                log.Error("Se ha producido una excepcion al intentar operar con la base de datos. Error: " + e.Message);
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