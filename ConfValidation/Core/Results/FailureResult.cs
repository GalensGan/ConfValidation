using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uamazing.ConfValidatation.Core.Results
{
    public class FailureResult : ValidateResult
    {
        public FailureResult(string fullPath, string message, string innerMessage) : base(false, fullPath, message,innerMessage)
        {
            if (string.IsNullOrEmpty(innerMessage)) InnerMessage = $"{fullPath} is Required,but accept null";
        }
    }
}
