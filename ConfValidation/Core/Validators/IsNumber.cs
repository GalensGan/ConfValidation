using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uamazing.ConfValidatation.Core.Results;
using Uamazing.ConfValidatation.Core.Utils;

namespace Uamazing.ConfValidatation.Core.Validators
{
    /// <summary>
    /// 数字验证
    /// </summary>
    public class IsNumber : Validator
    {
        protected override ValidateResult ValidateCore(object value, string parentFullPath, string fullPath)
        {           
            var valueType = value.GetType();
            if (!NumberHelper.IsNumericType(valueType)) return new FailureResult(fullPath, FailureMessage, $"value required numberic type,but accept {valueType.Name}");

            return new OkResult(fullPath);
        }
    }
}
