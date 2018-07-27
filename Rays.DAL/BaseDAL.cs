using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Rays.Model;

namespace Rays.DAL
{
    public class BaseDAL<T> : IDAL<T> where T : class
    {
        private DbContext db;
        private DbSet<T> dbSet;

        public BaseDAL(string key)
        {
            db = ContextFactory.GetCurrentContext(key);
            dbSet = db.Set<T>();
        }

        public BaseDAL()
        {
            db = ContextFactory.GetCurrentContext("zhiyin");
            dbSet = db.Set<T>();
        }

        public T Add(T entity)
        {
            db.Entry(entity).State = EntityState.Added;
            db.SaveChanges();
            return entity;
        }
        public int Add(List<T> entitys)
        {
            foreach(var entity in entitys)
            {
                db.Entry(entity).State = EntityState.Added;
            }
            return db.SaveChanges();
        }
        public int Count(Expression<Func<T, bool>> predicate)
        {
            return dbSet.Count(predicate);
        }
        public bool Update(List<T> entitys)
        {
            foreach (var entity in entitys)
            {
                dbSet.Attach(entity);
                db.Entry(entity).State = EntityState.Modified;
            }
            return db.SaveChanges() > 0;
        }
        public bool Update(T entity)
        {
            dbSet.Attach(entity);
            db.Entry(entity).State = EntityState.Modified;
            return db.SaveChanges() > 0;
        }
       
        public bool Delete(T entity)
        {
            dbSet.Attach(entity);
            db.Entry(entity).State = EntityState.Deleted;
            return db.SaveChanges() > 0;
        }
        public bool DeleteList(List<T> entitys)
        {
            foreach(var entity in entitys)
            {
                dbSet.Attach(entity);
                db.Entry(entity).State = EntityState.Deleted;
            }
            return db.SaveChanges() > 0;
        }
        public bool Exist(Expression<Func<T, bool>> anyLambda)
        {
            return dbSet.AsNoTracking().Any(anyLambda);
        }

        public T Find(Expression<Func<T, bool>> whereLambda)
        {
            T entity = dbSet.AsNoTracking().FirstOrDefault(whereLambda);
            return entity;
        }
        public T Find1(Expression<Func<T, bool>> whereLambda)
        {
            T entity = dbSet.FirstOrDefault(whereLambda);
            return entity;
        }
        public IQueryable<T> FindAllList()
        {
            var list = dbSet.AsNoTracking().Where(x => true);
            return list;
        }
        public IQueryable<T> FindList<TS>(Expression<Func<T, bool>> whereLamdba)
        {
            var list = dbSet.AsNoTracking().Where(whereLamdba);
            return list;
        }
        public IQueryable<T> FindList<TS>(Expression<Func<T, bool>> whereLamdba, bool isAsc, Expression<Func<T, TS>> orderLamdba)
        {
            var list = dbSet.AsNoTracking().Where(whereLamdba);
            if (isAsc) list = list.OrderBy(orderLamdba);
            else list = list.OrderByDescending(orderLamdba);
            return list;
        }

        public IQueryable<T> FindPageList<TS>(int pageIndex, int pageSize, out int totalRecord, Expression<Func<T, bool>> whereLamdba, bool isAsc, Expression<Func<T, TS>> orderLamdba)
        {
            var list = dbSet.AsNoTracking().Where(whereLamdba);
            totalRecord = list.Count();
            if (isAsc)
            {
                list = list.OrderBy(orderLamdba).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
            else
            {
                list = list.OrderByDescending(orderLamdba).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
            return list;
        }

    }
}
