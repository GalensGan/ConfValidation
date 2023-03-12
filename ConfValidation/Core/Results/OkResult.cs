using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uamazing.ConfValidatation.Core.Results
{
    public class OkResult : ValidateResult
    {
        public OkResult(string fullPath) : base(true, fullPath, "","")
        {
        }
    }
}
