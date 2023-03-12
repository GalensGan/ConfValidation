using Pather.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uamazing.ConfValidatation.Core.Results;

namespace Uamazing.ConfValidatation.Core.Validators
{
    /// <summary>
    /// validator 的逻辑部分代码
    /// </summary>
    public abstract partial class Validator
    {
        #region 入口
        /// <summary>
        /// 验证入口
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="parentFullPath"></param>
        /// <returns></returns>
        public virtual ValidateResult Validate<T>(T value, string parentFullPath)
        {
            var fullPath = CombineFullPath(parentFullPath);
            var objValue = value == null ? null : GetValue(value);

            // 在 GetValue 中已经处理过 required 为 true 且没有结果问题
            if (!Required && objValue == null)
            {
                return new OkResult(fullPath);
            }

            if(objValue == null)throw new ArgumentNullException($"{fullPath} is required, but accept null");
            return ValidateCore(objValue, parentFullPath, fullPath);
        }
        #endregion

        #region 抽象方法
        protected abstract ValidateResult ValidateCore(object value, string parentFullPath, string fullPath);
        #endregion

        #region Path 帮助方法
        /// <summary>
        /// 组装对象访问路径
        /// </summary>
        /// <param name="parentPath"></param>
        /// <returns></returns>
        protected string CombineFullPath(string parentFullPath)
        {
            if (Path == "$") return parentFullPath;

            // 如果包含中括号，则直接拼接
            if (Path.StartsWith("[")) return parentFullPath + Path;

            return $"{parentFullPath}.{Path}";
        }

        /// <summary>
        /// 获取当前级的实际值
        /// 子类如果要完全重写 <see cref="Validate"/> 方法，则应通过该接口获取值
        /// </summary>
        /// <typeparam name="Tin"></typeparam>
        /// <typeparam name="Tout"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        protected object GetValue(object value)
        {
            if (Path == "$") return value;

            // 通过路径获取值
            Resolver resolver = new Resolver();
            if (Required) return resolver.Resolve(value, Path);

            return resolver.ResolveSafe(value, Path);
        }
        #endregion
    }
}
