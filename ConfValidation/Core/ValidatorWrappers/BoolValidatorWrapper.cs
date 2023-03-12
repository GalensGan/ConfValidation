using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uamazing.ConfValidatation.Core.Results;
using Uamazing.ConfValidatation.Core.Validators;

namespace Uamazing.ConfValidatation.Core.ValidatorWrappers
{
    internal class BoolValidatorWrapper : ValidatorWrapperBase
    {
        private readonly bool _value;
        public BoolValidatorWrapper(bool value, Validator validator) : base(validator)
        {
            _value = value;
        }

        public override ValidateResult Validate<T>(T value, string parentFullPath)
        {
           return Validator.Validate(_value, parentFullPath);
        }
    }
}
