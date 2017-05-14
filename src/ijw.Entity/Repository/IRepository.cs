using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ijw.Entity {
    /// <summary>
    /// 仓储接口，定义公共的泛型GRUD
    /// </summary>
    /// <typeparam name="TEntity">泛型聚合根，因为在DDD里面仓储只能对聚合根做操作</typeparam>
    public interface IRepository<TEntity> where TEntity : AggregateRootBase {
        #region 属性
        IQueryable<TEntity> Entities { get; }
        #endregion

        #region 公共方法
        TEntity Find(Guid key);

        int Insert(TEntity entity, bool shouldCommit);

        int Insert(IEnumerable<TEntity> entities, bool shouldCommit);

        int Delete(Guid id, bool shouldCommit);

        int Delete(TEntity entity, bool shouldCommit);

        int Delete(IEnumerable<TEntity> entities, bool shouldCommit);

        int Update(TEntity entity, bool shouldCommit);
        #endregion
    }
}
