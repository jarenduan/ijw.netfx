using ijw.Log;
using ijw.Log.File;
using System;

namespace ijw.Data.Entity {
    ////TContext本应该继承于DbContext, 即泛型参数的基类是EF程序集中的类;
    ////这将导致任何EntityServicesBase<TContext>的调用者(哪怕是其继承类的调用者), 必须也要手动显式引用EF的程序集;
    ////这会可能造成不便, 因此这里退为实现IDisposable.
    ////具体情况参见ijw.Demo.ProjectReference.User项目.

    /// <summary>
    /// 实体服务抽象基类, 内部提供了一个日志记录器.
    /// </summary>
    /// <typeparam name="TContext">实体上下文</typeparam>
    /// <remarks>
    /// 使用此类继承将会节省一些代码
    /// 但是会要求基类的使用者也安装entityframework 
    /// </remarks>
    public abstract class EntityServicesBase<TContext> : IDisposable where TContext : IDisposable, new() {
        /// <summary>
        /// 日志记录器
        /// </summary>
        protected LogHelper _logger = new LogHelper();
        /// <summary>
        /// 实体上下文实例
        /// </summary>
        protected TContext _context = new TContext();

        /// <summary>
        /// 释放实体上下文
        /// </summary>
        public void Dispose() {
            this._context.Dispose();
        }

        ////没有下面这行语句的显式类型使用, 调用者build后, 项目输出文件夹中EF.SqlServer Dll的引用会被自动优化掉, 导致运行时缺少相应引用
        //static private Type _tempType1_ = typeof(System.Data.Entity.SqlServer.SqlProviderServices);

        //PS: 使用如下的方法是不行的:(
        //static EntityServicesBase() {
        //    var _tempType2_ = typeof(System.Data.Entity.SqlServerCompact.SqlCeProviderServices);
        //}
    }
}