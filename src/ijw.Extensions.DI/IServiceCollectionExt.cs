using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ijw.Extensions.DI {
    public static class IServiceCollectionExt {
        public static T GetService<T>(this IServiceCollection srv) {
            return srv.BuildServiceProvider().GetService<T>();
        }
    }
}
