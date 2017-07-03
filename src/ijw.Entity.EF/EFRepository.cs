using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ijw.Entity.EF {
    public class EFRepository<TEntity> : IRepository<TEntity> where TEntity : AggregateRootBase {
        private EFUnitOfWork _unitOfWork;

        public EFRepository(EFUnitOfWork unitOfWork) {
            this._unitOfWork = unitOfWork;
        }

        public IQueryable<TEntity> Entities {
            get { return _unitOfWork.Context.Set<TEntity>(); }
        }

        public TEntity Find(Guid key) {
            return _unitOfWork.Context.Set<TEntity>().Find(key);
        }

        public int Insert(TEntity entity, bool shouldCommit = false) {
            _unitOfWork.RegisterNew(entity);
            return shouldCommit ? _unitOfWork.Commit() : 0;
        }

        public int Insert(IEnumerable<TEntity> entities, bool shouldCommit = false) {
            _unitOfWork.RegisterNew(entities);
            return shouldCommit ? _unitOfWork.Commit() : 0;
        }

        public int Delete(Guid id, bool shouldCommit = false) {
            var obj = _unitOfWork.Context.Set<TEntity>().Find(id);
            if (obj == null) {
                return 0;
            }
            _unitOfWork.RegisterDeleted(obj);
            return shouldCommit ? _unitOfWork.Commit() : 0;
        }

        public int Delete(TEntity entity, bool shouldCommit = false) {
            _unitOfWork.RegisterDeleted(entity);
            return shouldCommit ? _unitOfWork.Commit() : 0;
        }

        public int Delete(IEnumerable<TEntity> entities, bool shouldCommit = false) {
            _unitOfWork.RegisterDeleted(entities);
            return shouldCommit ? _unitOfWork.Commit() : 0;
        }

        public int Update(TEntity entity, bool shouldCommit = false) {
            _unitOfWork.RegisterModified(entity);
            return shouldCommit ? _unitOfWork.Commit() : 0;
        }
    }
}
