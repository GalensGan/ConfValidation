using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uamazing.ConfValidatation.Core.Utils
{
    public class NumberHelper
    {
        private static readonly HashSet<Type> _numericTypes = new HashSet<Type>
        {
            typeof(int),
            typeof(uint),
            typeof(long),
            typeof(ulong),
            typeof(float),
            typeof(double),
            typeof(decimal),
        };

        /// <summary>
        /// 是否是数字类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNumericType(Type type)
        {
            return _numericTypes.Contains(type) ||
                   _numericTypes.Contains(Nullable.GetUnderlyingType(type));
        }
    }
}
