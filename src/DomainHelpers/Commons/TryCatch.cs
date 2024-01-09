using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainHelpers.Commons;

public class TryCatchUtils {
    public static T? Try<T>(Func<T> func) {
        try {
            return func();
        }
        catch {
            return default!;
        }
    }

    public static T Try<T>(Func<T> func, T whenCatch) {
        try {
            return func();
        }
        catch {
            return whenCatch;
        }
    }

    public static T Try<T, TException>(Func<T> func, Func<TException, T> whenCatch) where TException : Exception {
        try {
            return func();
        }
        catch (TException ex) {
            return whenCatch(ex);
        }
    }

    public static async Task<T?> Try<T>(Func<Task<T>> func) {
        try {
            return await func()!;
        }
        catch {
            return default;
        }
    }

    public static async Task<T> Try<T>(Func<Task<T>> func, T whenCatch) {
        try {
            return await func();
        }
        catch {
            return whenCatch;
        }
    }

    public static async Task<T> Try<T, TException>(Func<Task<T>> func, Func<TException, T> whenCatch) where TException : Exception {
        try {
            return await func();
        }
        catch (TException ex) {
            return whenCatch(ex);
        }
    }
}
