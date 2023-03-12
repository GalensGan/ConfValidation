using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uamazing.ConfValidatation.Core.Utils
{
    public class AttributeHelper
    {
        /// <summary>
        /// 获取自定义特性
        /// 如果类上确实定义了 [T] 特性，则返回该特性的实例；否则，返回 null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static T GetCustomAttribute<T>(Type type) where T : Attribute
        {
            var customAtt = type.GetCustomAttributes(typeof(T), true).FirstOrDefault();
#pragma warning disable CS8603 // 可能返回 null 引用。
            if (customAtt == null)return default;
            return customAtt as T;
#pragma warning restore CS8603 // 可能返回 null 引用。
        }
    }
}
