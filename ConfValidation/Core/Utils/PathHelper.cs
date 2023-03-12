using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Uamazing.ConfValidatation.Core.Utils
{
    public class PathHelper
    {
        /// <summary>
        /// 从调用中获取路径
        /// 例如：()=>user.Name => user.Name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lambdaExpression"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static string ResovePath<T>(Expression<Func<T>> lambdaExpression)
        {
            var expressionStr = lambdaExpression.ToString();

            string pattern = @"\bvalue\([^)]*\)\.(.+)";
            var match = Regex.Match(expressionStr, pattern);
            if (match.Success)
            {
                var result = match.Groups[1].Value.Replace(".get_Item(", "[").Replace(")", "]");
                return result;
            }

            throw new ArgumentException($"can not extract path from {expressionStr}");
        }

        /// <summary>
        /// 获取路径，始终以 "$" 开头
        /// 例如：()=>user.Name => $Name
        /// </summary>
        /// <param name="lambdaExpression"></param>
        /// <returns></returns>
        public static string ResovePathStartWithDollarSign<T>(Expression<Func<T>> lambdaExpression)
        {
            // 获取路径
            var path = PathHelper.ResovePath(lambdaExpression);
            // path 包含了调用的实例名，将其替换成 $
            var firstDotIndex = path.IndexOf(".");
            if (firstDotIndex < 0) path = "$";
            else path = "$" + path.Substring(firstDotIndex + 1);

            return path;
        }
    }
}
