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
    /// 数组专用
    /// 判断某个节点的数组是否符合条件
    /// </summary>
    [AnonymousValidator]
    public class ElementAt : IsArray
    {
        public int Position { get; set; } = -1;
        public ElementAt(int position)
        {
            Position = position;
        }

        protected override ValidateResult ValidateCore(object value, string parentFullPath, string fullPath)
        {
            var baseResult = base.ValidateCore(value, parentFullPath,fullPath);
            if (!baseResult) return baseResult;

            // 有可能是字典类型，目前不对字典作处理            

            if (!(value is System.Collections.IEnumerable enumerableValue)) return new FailureResult(fullPath,FailureMessage,$"{fullPath} require IEnumerable, but accept {value.GetType().Name}");

            int index = -1;
            foreach (var ele in enumerableValue)
            {
                index++;
                if (Position == index)
                {
                    // 执行验证
                    var vdResult = RunLogicValidate(ele, parentFullPath, fullPath);
                    return vdResult;
                }
            }

            throw new ArgumentOutOfRangeException($"{Position} is out of ${index}");
        }
    }
}
