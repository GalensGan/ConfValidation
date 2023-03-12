using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uamazing.ConfValidatation.Core.Attributes;
using Uamazing.ConfValidatation.Core.Results;

namespace Uamazing.ConfValidatation.Core.Validators
{
    /// <summary>
    /// use custom functions for validation
    /// </summary>
    [AnonymousValidator]
    public class Function<T> : Validator
    {
        private readonly Func<T, bool> _func;
        public Function(Func<T,bool> func){ _func = func; }
        protected override ValidateResult ValidateCore(object value, string parentFullPath, string fullPath)
        {
            // 将数据强制转换成 T 类型
            if(!(value is T targetValue))throw new ArgumentNullException($"can not be converted from {value.GetType().Name}to {typeof(T).Name}");

            // 调用函数获取结果
            var vdResult = _func.Invoke(targetValue);
            if(vdResult)return new OkResult(fullPath);

            return new FailureResult(fullPath, FailureMessage, "custom function returns false");
        }
    }
}
