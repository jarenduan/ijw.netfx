using System;
using System.Collections.Generic;

namespace ijw.Entity {
    /// <summary>
    /// 仓储上下文工作单元接口，使用这个的一般情况是多个仓储之间存在事务性的操作，用于标记聚合根的增删改状态
    /// </summary>
    public interface IUnitOfWorkRepositoryContext : IUnitOfWork, IDisposable {
        /// <summary>
        /// 将聚合根的状态标记为新建，但上下文此时并未提交
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        void RegisterNew<TEntity>(TEntity entity)
            where TEntity : AggregateRootBase;

        /// <summary>
        /// 将聚合根的状态标记为新建，但上下文此时并未提交
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        void RegisterNew<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : AggregateRootBase;

        /// <summary>
        /// 将聚合根的状态标记为修改，但上下文此时并未提交
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entitiy"></param>
        void RegisterModified<TEntity>(TEntity entity)
            where TEntity : AggregateRootBase;

        /// <summary>
        /// 将聚合根的状态标记为删除，但上下文此时并未提交
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        void RegisterDeleted<TEntity>(TEntity entity)
            where TEntity : AggregateRootBase;

        /// <summary>
        /// 将聚合根的状态标记为删除，但EF上下文此时并未提交
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        void RegisterDeleted<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : AggregateRootBase;

    }
}