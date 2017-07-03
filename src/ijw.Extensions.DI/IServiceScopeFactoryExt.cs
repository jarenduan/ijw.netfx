using System;
using Microsoft.Extensions.DependencyInjection;

namespace ijw.Extensions.DI {
    public static class IServiceScopeFactoryExt {
        /// <summary>
        /// 创建一个新的scope，找到指定类型的资源，进行用户的操作, 执行完后，立即释放scope及创建出来的资源。
        /// </summary>
        /// <typeparam name="TResource">想使用的资源类型</typeparam>
        /// <param name="serviceFactory"></param>
        /// <param name="action">想要对资源进行的操作</param>
        /// <remarks>
        /// 资源是实现了IDisposable的服务。只有资源才被serviceProvider引用，无法及时回收。为了及时回收，需要创建单独的scope进行使用。
        /// scope的释放，将会同时释放使用的资源。
        /// 未实现IDisposable的服务，可以直接使用ServiceCollection.GetService<T>()。并不会被引用，进而阻止回收。
        /// </remarks>
        public static void UseResourceInNewScope<TResource>(this IServiceScopeFactory serviceFactory, Action<TResource> action) where TResource : IDisposable {
            if (action == null) {
                return;
            }
            var scope = serviceFactory.CreateScope();
            var resource = scope.ServiceProvider.GetService<TResource>();
            if (resource != null) {
                if (resource is IDisposable disposable) {
                    using (scope) {
                        action(resource);
                    }
                }
            }
        }

        /// <summary>
        /// 创建一个新的scope，找到指定类型的资源，进行用户的操作。使用此重载用户应该自行负责scope的释放。
        /// </summary>
        /// <typeparam name="TResource">想使用的资源类型</typeparam>
        /// <param name="serviceFactory"></param>
        /// <param name="action">想要对资源进行的操作。操作必须在使用完子夜之后负责scope的释放</param>
        /// <remarks>
        /// 资源是IDisposable的服务。只有资源才被serviceProvider引用，无法及时回收。为了及时回收，需要创建单独的scope进行使用。
        /// scope的释放，将会同时释放使用的资源。
        /// 未实现IDisposable的服务，可以直接使用GetService()。并不会被引用，进而阻止回收。
        /// </remarks>
        public static void UseResourceInNewScope<TResource>(this IServiceScopeFactory serviceFactory, Action<IServiceScope, TResource> action) where TResource : IDisposable {
            if (action == null) {
                return;
            }
            var scope = serviceFactory.CreateScope();
            var resource = scope.ServiceProvider.GetService<TResource>();
            if (resource != null) {
                if (resource is IDisposable disposable) {
                    action(scope, resource);
                }
            }
        }
    }
}
