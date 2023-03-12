using System;
using System.Collections.Generic;
using System.Text;
using Uamazing.ConfValidatation.Core.Results;
using Uamazing.ConfValidatation.Core.Validators;

namespace Uamazing.ConfValidation.Core.Validators
{
    /// <summary>
    /// 判断相等
    /// </summary>
    public class Equals : Validator
    {
        public Equals() { }
        private readonly object _target;
        public Equals(object target) { _target = target; }

        protected override ValidateResult ValidateCore(object value, string parentFullPath, string fullPath)
        {
            bool isEqual = value.Equals(_target);
            if(isEqual)return new OkResult(fullPath);

            return new FailureResult(fullPath, FailureMessage, $"the value of {fullPath} is not equal to {_target}");
        }
    }
}
