using ijw.Contract;
using System;
using System.Threading;

namespace ijw {
    /// <summary>
    /// 多次运行一个方法或者委托的帮助类
    /// </summary>
    public static class DelegateHelper {
        /// <summary>
        /// 运行该委托指定次数
        /// </summary>
        /// <param name="loopbody"></param>
        /// <param name="times">运行次数</param>
        public static void Run(this Action loopbody, int times) {
            for (int i = 0; i < times; i++) {
                loopbody();
            }
        }

        /// <summary>
        /// 运行某方法指定次数
        /// </summary>
        /// <param name="times">运行次数</param>
        /// <param name="loopBody">欲运行的委托</param>
        public static void Run(int times, Action loopBody) {
            for(int i = 0; i < times; i++) {
                loopBody();
            }
        }

        /// <summary>
        /// 尝试运行某方法指定次数, 直至该方法成功运行不抛出异常. 达到最大次数后仍未成功, 则抛出异常.
        /// </summary>
        /// <param name="toRun">指定的方法, 返回true则为成功运行, false为运行失败.</param>
        /// <param name="maxRetryTimes">尝试的最多次数</param>
        /// <returns>是否成功运行该指定方法</returns>
        public static bool TryRunUntilTrue(Func<bool> toRun, int maxRetryTimes = 5) {
            while (maxRetryTimes > 0) {
                if (toRun() == true) {
                    return true;
                }
                maxRetryTimes--;
            }
            return false;
        }

        /// <summary>
        /// 尝试运行某方法指定次数, 直至该方法成功运行不抛出异常. 达到最大次数后仍未成功, 则抛出异常.
        /// 方法每次运行时将得知这是第几次运行.
        /// </summary>
        /// <param name="toRun">指定的方法, 返回true则为成功运行, false为运行失败.</param>
        /// <param name="maxRetryTimes">尝试的最多次数</param>
        /// <returns>是否成功运行该指定方法</returns>
        public static bool TryRunUntilTrue(Func<int, bool> toRun, int maxRetryTimes = 5) {
            int tryTime = 1;
            while (tryTime <= maxRetryTimes) {
                if (toRun(tryTime) == true) {
                    return true;
                }
                tryTime++;
            }
            return false;
        }

        /// <summary>
        /// 尝试运行某方法指定次数, 直至该方法成功运行不抛出异常. 达到最大次数后仍未成功, 则抛出异常.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="toRun">欲运行的方法</param>
        /// <param name="maxRetryTimes">尝试的最多次数</param>
        /// <returns>返回该制定方法成功运行的结果</returns>
        public static T TryRun<T>(Func<T> toRun, int maxRetryTimes = 5) {
            while (maxRetryTimes > 0) {
                try {
                    return toRun();
                }
                catch {
                    maxRetryTimes--;
                }
            }
            throw new ReachMaxRetryTimeException();
        }

        /// <summary>
        /// 尝试运行某方法指定次数, 直至该方法成功运行不抛出异常. 达到最大次数后仍未成功, 则抛出异常.
        /// 方法每次运行时将得知这是第几次运行.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="toRun">欲运行的方法</param>
        /// <param name="maxRetryTimes">尝试的最多次数</param>
        /// <returns>返回该制定方法成功运行的结果</returns>
        public static T TryRun<T>(Func<int, T> toRun, int maxRetryTimes = 5) {
            int tryTime = 1;
            while (tryTime <= maxRetryTimes) {
                try {
                    return toRun(tryTime);
                }
                catch {
                    tryTime++;
                }
            }
            throw new ReachMaxRetryTimeException();
        }

        /// <summary>
        /// 尝试运行某方法指定次数, 直至该方法不返回空. 达到最大次数后仍未得到有效值, 则抛出异常.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="toRun">欲运行的方法</param>
        /// <param name="maxRetryTimes">尝试的最多次数</param>
        /// <returns>返回该制定方法成功运行的结果</returns>
        public static T TryRunUntilNotNull<T>(Func<T> toRun, int maxRetryTimes = 5) where T : class {
            while (maxRetryTimes > 0) {
                var result = toRun();
                if (result != null) {
                    return result;
                }
                else {
                    maxRetryTimes--;
                }
            }
            throw new ReachMaxRetryTimeException();
        }

        /// <summary>
        /// 尝试运行某方法指定次数, 直至该方法不返回空. 达到最大次数后仍未得到有效值, 则抛出异常.
        /// 方法每次运行时将得知这是第几次运行.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="toRun">欲运行的方法</param>
        /// <param name="maxRetryTimes">尝试的最多次数</param>
        /// <returns>返回该制定方法成功运行的结果</returns>
        public static T TryRunUntilNotNull<T>(Func<int, T> toRun, int maxRetryTimes = 5) where T : class {
            int tryTime = 1;
            while (tryTime <= maxRetryTimes) {
                var result = toRun(tryTime);
                if (result != null) {
                    return result;
                }
                else {
                    tryTime++;
                }
            }
            throw new ReachMaxRetryTimeException();
        }

        /// <summary>
        /// 运行该委托指定次数
        /// </summary>
        /// <param name="loopbody"></param>
        /// <param name="times">运行次数</param>
        public static void RunWithInterval(this Action loopbody, int times, int interval = 0)
        {
            interval.ShouldSatisfy((i) => i > 0);
            for (int i = 0; i < times; i++)
            {
                loopbody();
                if (interval > 0)
                {
                    Thread.Sleep(interval);
                }
            }
        }

        /// <summary>
        /// 运行某方法指定次数
        /// </summary>
        /// <param name="times">运行次数</param>
        /// <param name="loopBody">欲运行的委托</param>
        public static void RunWithInterval(int times, Action loopBody, int interval = 0)
        {
            interval.ShouldSatisfy((i) => i > 0);
            for (int i = 0; i < times; i++)
            {
                loopBody();
                if (interval > 0)
                {
                    Thread.Sleep(interval);
                }
            }
        }

        /// <summary>
        /// 尝试运行某方法指定次数, 直至该方法成功运行不抛出异常. 达到最大次数后仍未成功, 则抛出异常.
        /// </summary>
        /// <param name="toRun">指定的方法, 返回true则为成功运行, false为运行失败.</param>
        /// <param name="maxRetryTimes">尝试的最多次数</param>
        /// <returns>是否成功运行该指定方法</returns>
        public static bool TryRunUntilTrueWithInterval(Func<bool> toRun, int maxRetryTimes = 5, int interval = 0)
        {
            interval.ShouldSatisfy((i) => i > 0);
            while (maxRetryTimes > 0)
            {
                if (toRun() == true)
                {
                    return true;
                }
                if (interval > 0)
                {
                    Thread.Sleep(interval);
                }
                maxRetryTimes--;
            }
            return false;
        }

        /// <summary>
        /// 尝试运行某方法指定次数, 直至该方法成功运行不抛出异常. 达到最大次数后仍未成功, 则抛出异常.
        /// 方法每次运行时将得知这是第几次运行.
        /// </summary>
        /// <param name="toRun">指定的方法, 返回true则为成功运行, false为运行失败.</param>
        /// <param name="maxRetryTimes">尝试的最多次数</param>
        /// <returns>是否成功运行该指定方法</returns>
        public static bool TryRunUntilTrueWithInterval(Func<int, bool> toRun, int maxRetryTimes = 5, int interval = 0)
        {
            interval.ShouldSatisfy((i) => i > 0);
            int tryTime = 1;
            while (tryTime <= maxRetryTimes)
            {
                if (toRun(tryTime) == true)
                {
                    return true;
                }
                tryTime++;
                if (interval > 0)
                {
                    Thread.Sleep(interval);
                }
            }
            return false;
        }

        /// <summary>
        /// 尝试运行某方法指定次数, 直至该方法成功运行不抛出异常. 达到最大次数后仍未成功, 则抛出异常.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="toRun">欲运行的方法</param>
        /// <param name="maxRetryTimes">尝试的最多次数</param>
        /// <returns>返回该制定方法成功运行的结果</returns>
        public static T TryRunWithInterval<T>(Func<T> toRun, int maxRetryTimes = 5, int interval = 0)
        {
            interval.ShouldSatisfy((i) => i > 0);
            while (maxRetryTimes > 0)
            {
                try
                {
                    return toRun();
                }
                catch
                {
                    maxRetryTimes--;
                }
                if (interval > 0)
                {
                    Thread.Sleep(interval);
                }
            }
            throw new ReachMaxRetryTimeException();
        }

        /// <summary>
        /// 尝试运行某方法指定次数, 直至该方法成功运行不抛出异常. 达到最大次数后仍未成功, 则抛出异常.
        /// 方法每次运行时将得知这是第几次运行.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="toRun">欲运行的方法</param>
        /// <param name="maxRetryTimes">尝试的最多次数</param>
        /// <returns>返回该制定方法成功运行的结果</returns>
        public static T TryRunWithInterval<T>(Func<int, T> toRun, int maxRetryTimes = 5, int interval = 0)
        {
            interval.ShouldSatisfy((i) => i > 0);
            int tryTime = 1;
            while (tryTime <= maxRetryTimes)
            {
                try
                {
                    return toRun(tryTime);
                }
                catch
                {
                    tryTime++;
                }
                if (interval > 0)
                {
                    Thread.Sleep(interval);
                }
            }
            throw new ReachMaxRetryTimeException();
        }

        /// <summary>
        /// 尝试运行某方法指定次数, 直至该方法不返回空. 达到最大次数后仍未得到有效值, 则抛出异常.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="toRun">欲运行的方法</param>
        /// <param name="maxRetryTimes">尝试的最多次数</param>
        /// <returns>返回该制定方法成功运行的结果</returns>
        public static T TryRunUntilNotNullWithInterval<T>(Func<T> toRun, int maxRetryTimes = 5, int interval = 0) where T : class
        {
            interval.ShouldSatisfy((i) => i > 0);
            while (maxRetryTimes > 0)
            {
                var result = toRun();
                if (result != null)
                {
                    return result;
                }
                else
                {
                    maxRetryTimes--;
                }
                if (interval > 0)
                {
                    Thread.Sleep(interval);
                }
            }
            throw new ReachMaxRetryTimeException();
        }

        /// <summary>
        /// 尝试运行某方法指定次数, 直至该方法不返回空. 达到最大次数后仍未得到有效值, 则抛出异常.
        /// 方法每次运行时将得知这是第几次运行.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="toRun">欲运行的方法</param>
        /// <param name="maxRetryTimes">尝试的最多次数</param>
        /// <returns>返回该制定方法成功运行的结果</returns>
        public static T TryRunUntilNotNullWithInterval<T>(Func<int, T> toRun, int maxRetryTimes = 5, int interval = 0) where T : class
        {
            interval.ShouldSatisfy((i) => i > 0);
            int tryTime = 1;
            while (tryTime <= maxRetryTimes)
            {
                var result = toRun(tryTime);
                if (result != null)
                {
                    return result;
                }
                else
                {
                    tryTime++;
                }
                if (interval > 0)
                {
                    Thread.Sleep(interval);
                }
            }
            throw new ReachMaxRetryTimeException();
        }
    }
}