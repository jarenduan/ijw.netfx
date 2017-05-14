using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ijw.Entity.EF {
    /// <summary>
    /// EntityFramework工作单实现类
    /// </summary>
    public class EFUnitOfWork : IUnitOfWorkRepositoryContext {
        #region 属性
        //通过工作单元向外暴露的EF上下文对象
        public DbContext Context { get; protected set; }
        #endregion

        #region 构造函数
        public EFUnitOfWork(DbContext context) {
            this.Context = context;
        }
        #endregion

        #region IUnitOfWorkRepositoryContext接口
        public void RegisterNew<TEntity>(TEntity obj) where TEntity : AggregateRootBase {
            var state = Context.Entry(obj).State;
            if (state == EntityState.Detached) {
                Context.Entry(obj).State = EntityState.Added;
            }
            IsCommitted = false;
        }

        public void RegisterModified<TEntity>(TEntity obj) where TEntity : AggregateRootBase {
            if (Context.Entry(obj).State == EntityState.Detached) {
                Context.Set<TEntity>().Attach(obj);
            }
            Context.Entry(obj).State = EntityState.Modified;
            IsCommitted = false;
        }

        public void RegisterDeleted<TEntity>(TEntity obj) where TEntity : AggregateRootBase {
            Context.Entry(obj).State = EntityState.Deleted;
            IsCommitted = false;
        }

        public void RegisterNew<TEntity>(IEnumerable<TEntity> entities) where TEntity : AggregateRootBase {
            try {
                this.Context.Set<TEntity>().AddRange(entities);
                //Context.Configuration.AutoDetectChangesEnabled = false;
                //foreach (TEntity entity in entities) {
                //    RegisterNew(entity);
                //}
            }
            finally {
                //Context.Configuration.AutoDetectChangesEnabled = true;
            }
        }

        public void RegisterDeleted<TEntity>(IEnumerable<TEntity> entities) where TEntity : AggregateRootBase {
            this.Context.Set<TEntity>().RemoveRange(entities);
            //try {
            //    Context.Configuration.AutoDetectChangesEnabled = false;
            //    foreach (TEntity entity in entities) {
            //        RegisterNew(entity);
            //    }
            //}
            //finally {
            //    Context.Configuration.AutoDetectChangesEnabled = true;
            //}
        }
        #endregion

        #region IUnitOfWork接口

        public bool IsCommitted { get; set; }

        public int Commit() {
            if (IsCommitted) {
                return 0;
            }
            try {
                int result = Context.SaveChanges();
                IsCommitted = true;
                return result;
            }
            catch (DbUpdateException e) {
                if (e.InnerException != null && e.InnerException.InnerException is SqlException) {
                    SqlException sqlEx = e.InnerException.InnerException as SqlException;
                    string msg = GetSqlExceptionMessage(sqlEx.Number);
                    throw new Exception("提交数据更新时发生异常：" + msg, sqlEx);
                }
                throw e;
            }
        }

        public void Rollback() {
            IsCommitted = false;
        }
        #endregion

        #region IDisposable接口
        public void Dispose() {
            if (!IsCommitted) {
                Commit();
            }
            Context.Dispose();
        }
        #endregion

        private string GetSqlExceptionMessage(int number) {
            throw new NotImplementedException();
        }
    }
}
