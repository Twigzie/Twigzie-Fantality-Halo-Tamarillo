using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Tamarillo.Classes.Helpers.Extensions {

    public static class ExpressionExtensions {

        public static T GetFlag<T>(this Expression<Func<T>> input) {
            return input.Compile().Invoke();
        }
        public static void SetFlag<T>(this Expression<Func<T>> input, T value) {
            var a = input.Body as MemberExpression;
            var b = (PropertyInfo)a.Member;
            var c = Expression.Lambda(a.Expression).Compile().DynamicInvoke();
            b.SetValue(c, value);
        }

    }

}