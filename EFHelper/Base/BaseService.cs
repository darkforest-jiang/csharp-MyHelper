using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EFHelper.Context;

namespace EFHelper.Base
{
    public class BaseService<Entity> : IBaseService<Entity> where Entity : class
    {
        

        ///// <summary>
        ///// 实体数据上下文
        ///// </summary>
        //public ClassificationContext _DbSet { set; get; }

        /// <summary>
        /// 实体数据上下文
        /// </summary>
        public MyDBContextBase _DbSet { set; get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public BaseService()
        {
            // _DbSet = new ClassificationContext();
            _DbSet = MyDBContextBase.GetContext();

            //_DbSet.Configuration.ProxyCreationEnabled = false;
            //_DbSet.Configuration.LazyLoadingEnabled = false;
        }

        #region 公共函数
        /// <summary>
        /// 设置查询结果排序
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="orders"></param>
        /// <returns></returns>
        public IQueryable<Entity> GetOrderbyResult(IQueryable<Entity> entity, params IOrderByExpression<Entity>[] orders)
        {
            try
            {
                IOrderedQueryable<Entity> output = null;
                if (orders != null)
                {
                    foreach (var order in orders)
                    {
                        if (output == null)
                        {
                            output = order.ApplyOrderBy(entity);
                        }
                        else
                        {
                            output = order.ApplyThenBy(output);
                        }
                    }
                }
                return output ?? entity;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region 实体基础操作

        /// <summary>
        /// 添加单个实体数据
        /// </summary>
        /// <param name="T"></param>
        /// <returns></returns>
        public int EntityAdd(Entity T)
        {
            try
            {
                _DbSet.Entry(T).State = EntityState.Added;
                return _DbSet.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 添加多条实体数据
        /// </summary>
        /// <param name="T"></param>
        /// <returns></returns>
        public int EntityAdd(List<Entity> T)
        {
            try
            {
                _DbSet.Set<Entity>().AddRange(T);
                return _DbSet.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 修改单个实体数据
        /// </summary>
        /// <param name="T"></param>
        /// <returns></returns>
        public int EntityEdit(Entity T)
        {
            try
            {
                _DbSet.Set<Entity>().Attach(T);
                _DbSet.Entry(T).State = EntityState.Modified;
                return _DbSet.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 根据主键更新实体对象
        /// </summary>
        /// <param name="T"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int EntityEdit(Entity T, params object[] param)
        {
            try
            {
                Entity v = _DbSet.Set<Entity>().Find(param);
                _DbSet.Entry(v).CurrentValues.SetValues(T);
                return _DbSet.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 修改多个实体数据
        /// </summary>
        /// <param name="T"></param>
        /// <returns></returns>
        public int EntityEdit(List<Entity> T)
        {
            try
            {
                foreach (Entity p in T)
                {
                    _DbSet.Set<Entity>().Attach(p);
                    _DbSet.Entry(p).State = EntityState.Modified;
                }
                return _DbSet.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 删除单个实体
        /// </summary>
        /// <param name="T"></param>
        /// <returns></returns>
        public int EntityDelete(Entity T)
        {
            try
            {
                _DbSet.Set<Entity>().Attach(T);
                _DbSet.Entry(T).State = EntityState.Deleted;
                return _DbSet.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 按条件删除实体
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public int EntityDelete(Expression<Func<Entity, bool>> where)
        {
            try
            {
                IQueryable<Entity> list = _DbSet.Set<Entity>().Where<Entity>(where);
                if (list.Count() > 0)
                {
                    foreach (Entity item in list)
                    {
                        _DbSet.Entry(item).State = EntityState.Deleted;
                    }
                    return _DbSet.SaveChanges();
                }
                return 1;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 删除多个实体数据
        /// </summary>
        /// <param name="T"></param>
        /// <returns></returns>
        public int EntityDelete(List<Entity> T)
        {
            try
            {
                foreach (Entity p in T)
                {
                    _DbSet.Set<Entity>().Attach(p);
                    _DbSet.Entry(p).State = EntityState.Deleted;
                }
                return _DbSet.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 按条件查询实体数
        /// </summary>
        /// <typeparam name="S">类型参数,排序对象的类型</typeparam>
        /// <param name="where"></param>
        /// <param name="isASC"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public IQueryable<Entity> EntityQuery(Expression<Func<Entity, bool>> where, bool isASC, Expression<Func<Entity, object>> order)
        {
            try
            {
                IQueryable<Entity> list = _DbSet.Set<Entity>().Where<Entity>(where);
                if (isASC)
                    list = list.OrderBy(order);
                else
                    list = list.OrderByDescending(order);
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 按条件查询实体数(不分页)支持复合排序
        /// </summary>
        /// <param name="cndLambda">条件</param>
        /// <param name="orders">排序</param>
        /// <returns></returns>
        public IQueryable<Entity> EntityQuery(Expression<Func<Entity, bool>> cndLambda = null, params IOrderByExpression<Entity>[] orders)
        {
            try
            {
                IQueryable<Entity> list = _DbSet.Set<Entity>();
                if (cndLambda != null)
                {
                    list = list.Where(cndLambda);
                }
                IOrderedQueryable<Entity> output = null;
                if (orders != null)
                {
                    foreach (var order in orders)
                    {
                        if (output == null)
                        {
                            output = order.ApplyOrderBy(list);
                        }
                        else
                        {
                            output = order.ApplyThenBy(output);
                        }
                    }
                }
                return output ?? list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 按条件查询实体数,不用缓存
        /// </summary>
        /// <typeparam name="S">类型参数,排序对象的类型</typeparam>
        /// <param name="where"></param>
        /// <param name="isASC"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public IQueryable<Entity> NonCacheEntityQuery(Expression<Func<Entity, bool>> where, bool isASC, Expression<Func<Entity, object>> order)
        {
            try
            {
                IQueryable<Entity> list = _DbSet.Set<Entity>().Where(where).AsNoTracking();
                if (isASC)
                    list = list.OrderBy(order);
                else
                    list = list.OrderByDescending(order);
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 按条件查询实体数(不分页)支持复合排序
        /// </summary>
        /// <param name="cndLambda">条件</param>
        /// <param name="orders">排序</param>
        /// <returns></returns>
        public IQueryable<Entity> NonCacheEntityQuery(Expression<Func<Entity, bool>> cndLambda = null, params IOrderByExpression<Entity>[] orders)
        {
            try
            {
                IQueryable<Entity> list = _DbSet.Set<Entity>().AsNoTracking();
                if (cndLambda != null)
                {
                    list = list.Where(cndLambda).AsNoTracking();
                }
                list = GetOrderbyResult(list, orders);
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 按条件查询实体数,分页获取,并返回总记录数
        /// </summary>
        /// <typeparam name="S">类型参数,排序对象的类型</typeparam>
        /// <param name="where"></param>
        /// <param name="isASC"></param>
        /// <param name="order"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public IQueryable<Entity> EntityQuery(Expression<Func<Entity, bool>> where, bool isASC, Expression<Func<Entity, object>> order, int page, int rows, out int a)
        {
            try
            {
                IQueryable<Entity> list = _DbSet.Set<Entity>().Where(where);
                a = list.Count();
                if (isASC)
                    list = list.OrderBy(order);
                else
                    list = list.OrderByDescending(order);
                return list.Skip((page - 1) * rows).Take(rows);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 按条件查询实体数,分页获取,并返回总记录数 不使用缓存
        /// </summary>
        /// <typeparam name="S">类型参数,排序对象的类型</typeparam>
        /// <param name="where"></param>
        /// <param name="isASC"></param>
        /// <param name="order"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public IQueryable<Entity> NonCacheEntityQuery(Expression<Func<Entity, bool>> where, bool isASC, Expression<Func<Entity, object>> order, int page, int rows, out int a)
        {
            try
            {
                IQueryable<Entity> list = _DbSet.Set<Entity>().Where(where).AsNoTracking();
                a = list.Count();
                if (isASC)
                    list = list.OrderBy(order);
                else
                    list = list.OrderByDescending(order);
                if (rows > 0)
                    return list.Skip((page - 1) * rows).Take(rows);
                else
                    return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 按条件查询实体数(分页) 支持复合排序
        /// </summary>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="total">总条数</param>
        /// <param name="cndLambda">条件</param>
        /// <param name="orders">排序</param>
        /// <returns></returns>
        public IQueryable<Entity> NonCacheEntityQuery(int pageNumber, int pageSize, out int total, Expression<Func<Entity, bool>> cndLambda = null, params IOrderByExpression<Entity>[] orders)
        {
            try
            {
                IQueryable<Entity> list = _DbSet.Set<Entity>();
                if (cndLambda != null)
                {
                    list = list.Where(cndLambda);
                }
                list = GetOrderbyResult(list, orders);
                total = list.Count();
                return list.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entity> EntityQuery()
        {
            try
            {
                return _DbSet.Set<Entity>().AsQueryable();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 获取所有数据 禁用缓存
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entity> NonCacheEntityQuery()
        {
            try
            {
                return _DbSet.Set<Entity>().AsQueryable().AsNoTracking();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 获取实体记录条数
        /// </summary>
        /// <returns></returns>
        public int EntityCount()
        {
            try
            {
                return _DbSet.Set<Entity>().Count();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 根据条件查询单条数据
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public Entity Find(Expression<Func<Entity, bool>> whereLambda)
        {
            try
            {
                Entity _entity = _DbSet.Set<Entity>().FirstOrDefault(whereLambda);
                return _entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 根据条件查询单条数据
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public Entity NonCacheFind(Expression<Func<Entity, bool>> whereLambda)
        {
            try
            {
                Entity _entity = _DbSet.Set<Entity>().AsNoTracking().FirstOrDefault(whereLambda);
                return _entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        
    }
}
