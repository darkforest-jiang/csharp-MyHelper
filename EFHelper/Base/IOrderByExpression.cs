using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFHelper.Base
{
    public interface IOrderByExpression<Entity> where Entity: class
    {
        #region 接口
        IOrderedQueryable<Entity> ApplyOrderBy(IQueryable<Entity> query);

        IOrderedQueryable<Entity> ApplyThenBy(IOrderedQueryable<Entity> query);
        #endregion
    }
}
