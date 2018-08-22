using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Rays.DAL;
using Rays.Model;

namespace Rays.BLL
{
    public class BaseBLL<T> where T : class
    {
        protected BaseDAL<T> dal;

        public BaseBLL()
        {
            dal = new BaseDAL<T>();
        } 
        public BaseBLL(string key)
        {
            dal = new BaseDAL<T>(key);
        }

        public T Add(T entity)
        {
            return dal.Add(entity);
        }
        public int Add(List<T> entitys)
        {
            return dal.Add(entitys);
        }
        public int Count(Expression<Func<T, bool>> predicate)
        {
            return dal.Count(predicate);
        }

        public bool Update(T entity)
        {
            return dal.Update(entity);
        }
        public bool UpdateMore(List<T> entitys)
        {
            return dal.Update(entitys);
        }
        public bool Delete(T entity)
        {
            return dal.Delete(entity);
        }
        public bool DeleteList(List<T> entitys)
        {
            return dal.DeleteList(entitys);
        }

        public bool Exist(Expression<Func<T, bool>> anyLambda)
        {
            return dal.Exist(anyLambda);
        }

        public T Find(Expression<Func<T, bool>> whereLambda)
        {
            return dal.Find(whereLambda);
        }
        public T Find1(Expression<Func<T, bool>> whereLambda)
        {
            return dal.Find1(whereLambda);
        }
        public IQueryable<T> FindAllList()
        {
            return dal.FindAllList();
        }
        public IQueryable<T> FindList<TS>(Expression<Func<T, bool>> whereLamdba)
        {
            return dal.FindList<TS>(whereLamdba);
        }
        public IQueryable<T> FindList<TS>(Expression<Func<T, bool>> whereLamdba, bool isAsc, Expression<Func<T, TS>> orderLamdba)
        {
            return dal.FindList(whereLamdba, isAsc, orderLamdba);
        }

        public IQueryable<T> FindPageList<TS>(int pageIndex, int pageSize, out int totalRecord, Expression<Func<T, bool>> whereLamdba, bool isAsc, Expression<Func<T, TS>> orderLamdba)
        {
            return dal.FindPageList(pageIndex, pageSize, out totalRecord, whereLamdba, isAsc, orderLamdba);
        }
    }

}
