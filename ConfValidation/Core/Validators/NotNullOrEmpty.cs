using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uamazing.ConfValidatation.Core.Results;

namespace Uamazing.ConfValidatation.Core.Validators
{
    /// <summary>
    /// 字符串非空
    /// </summary>
    public class NotNullOrEmpty:Validator
    {
        protected override ValidateResult ValidateCore(object value, string parentFullPath, string fullPath)
        {
            // 如果是字符串，判断是否为 null 和 empty

            // 如果是集合，判断是否为 null 或者 count 为 0

            // 其它对象，判断是否为 null

            if (string.IsNullOrEmpty(value.ToString())) return new FailureResult(parentFullPath,FailureMessage, $"{parentFullPath} is null or empty");

            return new OkResult(parentFullPath);
        }
    }
}
