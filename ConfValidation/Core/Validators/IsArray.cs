using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uamazing.ConfValidatation.Core.Results;

namespace Uamazing.ConfValidatation.Core.Validators
{
    /// <summary>
    /// 可迭代类型（比如数组、列表、集合等）验证器
    /// </summary>
    public class IsArray : And
    {
        protected override ValidateResult ValidateCore(object value, string parentFullPath, string fullPath)
        {
            if(value is IEnumerable && value.GetType() != typeof(string)) return new OkResult(fullPath);
            return new FailureResult(fullPath, "", "");
        }
    }
}
