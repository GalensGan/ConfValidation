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
    /// 对每个数组元素进行验证
    /// 如果不是数组，则只针对单位元素进行验证
    /// </summary>
    public class EachElement : And
    {
        protected override ValidateResult ValidateCore(object value, string parentFullPath, string fullPath)
        {
            // 验证是否是可迭代的
            // 字符串也是可迭代的，需要去掉
            if (value is IEnumerable enumerableValue && value.GetType()!=typeof(string))
            {
                // 对每一个值执行验证
                foreach (var obj in enumerableValue)
                {
                    var childResult = RunLogicValidate(obj, parentFullPath, fullPath);
                    if (childResult.NotOk) return childResult;
                }

                return new OkResult(fullPath);
            }

            // 对当前值执行验证
            return RunLogicValidate(value, parentFullPath, fullPath);
        }
    }
}
