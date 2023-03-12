using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uamazing.ConfValidatation.Core.Attributes;
using Uamazing.ConfValidatation.Core.Results;
using Uamazing.ConfValidatation.Core.Validators;

namespace Uamazing.ConfValidatation.Core.ValidatorWrappers
{
    /// <summary>
    /// Wrapper 型验证器主要用于包裹已知数据类型，实现对特定值的验证
    /// </summary>
    [AnonymousValidator]
    internal abstract class ValidatorWrapperBase:Validator
    {
        protected Validator Validator { get; set; }
        public ValidatorWrapperBase(Validator validator)
        {
            Validator = validator;
        }

        protected override ValidateResult ValidateCore(object value, string parentFullPath, string fullPath) {
#pragma warning disable CS8603 // 可能返回 null 引用。
            return null;
#pragma warning restore CS8603 // 可能返回 null 引用。
        }
    }
}
