using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MedinetWeb.Models
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected MedinetWebEntities DbContext { get; set; }
        protected DbSet<T> DbSet { get; set; }

        public Repository(MedinetWebEntities dbContext)
        {
            if (dbContext == null) throw new NullReferenceException("dbContext");
            DbContext = dbContext;
            DbSet = dbContext.Set<T>();
        }

        #region Implementation of IRepository<T>

        public IQueryable<T> GetAll()
        {
            return DbSet;
        }

        public T GetById(int id)
        {
            return DbSet.Find(id);
        }

        public void Add(T entity)
        {
            DbSet.Add(entity);
        }

        public void Update(T entity)
        {
            var entry = DbContext.Entry(entity);
            DbSet.Attach(entity);
            entry.State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            var entry = DbContext.Entry(entity);
            DbSet.Attach(entity);
            entry.State = EntityState.Deleted;
        }

        public void Delete(int id)
        {
            var entity = GetById(id);
            if (entity == null) return;
            Delete(entity);
        }

        #endregion
    }
}