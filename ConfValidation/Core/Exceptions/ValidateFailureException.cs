using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uamazing.ConfValidatation.Core.Results;

namespace Uamazing.ConfValidatation.Core.Exceptions
{
    /// <summary>
    /// 验证失败错误
    /// </summary>
    public class ValidateFailureException:Exception
    {
        public ValidateResult ValidateResult { get;}
        public ValidateFailureException(ValidateResult validateResult):base(validateResult.Message)
        {
            ValidateResult= validateResult;
        }
    }
}
