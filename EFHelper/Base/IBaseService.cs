using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EFHelper.Base
{
    public interface IBaseService<Entity> where Entity:class
    {
        #region 实体基础操作
        /// <summary>
        /// 添加单个实体数据
        /// </summary>
        /// <param name="T"></param>
        /// <returns></returns>
        int EntityAdd(Entity T);

        /// <summary>
        /// 添加多条实体数据
        /// </summary>
        /// <param name="T"></param>
        /// <returns></returns>
        int EntityAdd(List<Entity> T);

        /// <summary>
        /// 修改单个实体数据
        /// </summary>
        /// <param name="T"></param>
        /// <returns></returns>
        int EntityEdit(Entity T);

        /// <summary>
        /// 修改多个实体数据
        /// </summary>
        /// <param name="T"></param>
        /// <returns></returns>
        int EntityEdit(List<Entity> T);

        /// <summary>
        /// 根据主键更新实体对象
        /// </summary>
        /// <param name="T"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        int EntityEdit(Entity T, params object[] param);

        /// <summary>
        /// 删除单个实体
        /// </summary>
        /// <param name="T"></param>
        /// <returns></returns>
        int EntityDelete(Entity T);

        /// <summary>
        /// 删除多个实体数据
        /// </summary>
        /// <param name="T"></param>
        /// <returns></returns>
        int EntityDelete(List<Entity> T);

        /// <summary>
        /// 按条件删除实体
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        int EntityDelete(Expression<Func<Entity, bool>> where);

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        IQueryable<Entity> EntityQuery();

        /// <summary>
        /// 获取实体记录条数
        /// </summary>
        /// <returns></returns>
        int EntityCount();

        /// <summary>
        /// 按条件查询实体数
        /// </summary>
        /// <typeparam name="S">类型参数,排序对象的类型</typeparam>
        /// <param name="where"></param>
        /// <param name="isASC"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        IQueryable<Entity> EntityQuery(Expression<Func<Entity, bool>> where, bool isASC, Expression<Func<Entity, object>> order);

        /// <summary>
        /// 按条件查询实体数,不用缓存
        /// </summary>
        /// <typeparam name="S">类型参数,排序对象的类型</typeparam>
        /// <param name="where"></param>
        /// <param name="isASC"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        IQueryable<Entity> NonCacheEntityQuery(Expression<Func<Entity, bool>> where, bool isASC, Expression<Func<Entity, object>> order);

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
        IQueryable<Entity> NonCacheEntityQuery(Expression<Func<Entity, bool>> where, bool isASC, Expression<Func<Entity, object>> order, int page, int rows, out int a);

        /// <summary>
        /// 按条件查询实体数(分页) 支持复合排序
        /// </summary>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="total">总条数</param>
        /// <param name="cndLambda">条件</param>
        /// <param name="orders">排序</param>
        /// <returns></returns>
        IQueryable<Entity> NonCacheEntityQuery(int pageNumber, int pageSize, out int total, Expression<Func<Entity, bool>> cndLambda = null, params IOrderByExpression<Entity>[] orders);

        /// <summary>
        /// 按条件查询实体数(不分页)支持复合排序
        /// </summary>
        /// <param name="cndLambda">条件</param>
        /// <param name="orders">排序</param>
        /// <returns></returns>
        IQueryable<Entity> NonCacheEntityQuery(Expression<Func<Entity, bool>> cndLambda = null, params IOrderByExpression<Entity>[] orders);

        /// <summary>
        /// 按条件查询实体数(不分页)支持复合排序
        /// </summary>
        /// <param name="cndLambda">条件</param>
        /// <param name="orders">排序</param>
        /// <returns></returns>
        IQueryable<Entity> EntityQuery(Expression<Func<Entity, bool>> cndLambda = null, params IOrderByExpression<Entity>[] orders);

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
        IQueryable<Entity> EntityQuery(Expression<Func<Entity, bool>> where, bool isASC, Expression<Func<Entity, object>> order, int page, int rows, out int a);

        /// <summary>
        /// 获取所有数据 禁用缓存
        /// </summary>
        /// <returns></returns>
        IQueryable<Entity> NonCacheEntityQuery();

        /// <summary>
        /// 根据条件查询单条数据
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        Entity Find(Expression<Func<Entity, bool>> whereLambda);

        /// <summary>
        /// 根据条件查询单条数据
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        Entity NonCacheFind(Expression<Func<Entity, bool>> whereLambda);
        #endregion
    }
}
