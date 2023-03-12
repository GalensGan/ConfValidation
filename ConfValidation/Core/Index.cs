using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Uamazing.ConfValidatation.Core.Exceptions;
using Uamazing.ConfValidatation.Core.Results;
using Uamazing.ConfValidatation.Core.Validators;

namespace Uamazing.ConfValidatation.Core.Entrance
{
    public static class Index
    {
        public static ValidateResult Validate<T>(this T data, Validator validator, ValidateOption options = ValidateOption.None)
        {
            var vdResult = validator.Validate(data, "$");
            if (options.HasFlag(ValidateOption.ThrowError)) throw new ValidateFailureException(vdResult);
            return vdResult;
        }

        public static ValidateResult Validate<T>(this T data, Validator[] validators, ValidateOption options = ValidateOption.None)
        {
            var and = new And();
            foreach (var validator in validators) and.Add(validator);
            return Validate(data, and, options);
        }
    }

    [Flags]
    public enum ValidateOption
    {
        None = 1,
        ThrowError = 2
    }
}
