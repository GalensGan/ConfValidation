using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uamazing.ConfValidatation.Core.Results;
using Uamazing.ConfValidatation.Core.Validators;

namespace Uamazing.ConfValidatation.Core.ValidatorWrappers
{
    /// <summary>
    /// 字符串可能为空，有 required 验证
    /// </summary>
    internal class StringValidatorWrapper : ValidatorWrapperBase
    {
        private readonly string _value;
        public StringValidatorWrapper(string value, Validator validator) : base(validator)
        {
            _value = value;
        }

        protected override ValidateResult ValidateCore(object value, string parentFullPath, string fullPath)
        {
           return Validator.Validate(_value, parentFullPath);
        }
    }
}
