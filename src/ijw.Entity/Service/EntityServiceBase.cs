using System;
using ijw.Log;

namespace ijw.Entity {
    public abstract class EntityServiceBase : IDisposable {
        protected IUnitOfWorkRepositoryContext _unitOfWork;
        protected ILogHelper _logger;

        public EntityServiceBase(IUnitOfWorkRepositoryContext unitOfWork, ILogHelper logger) {
            this._unitOfWork = unitOfWork;
            this._logger = logger;
        }

        public void Dispose() {
            _unitOfWork.Dispose();
        }
    }
}