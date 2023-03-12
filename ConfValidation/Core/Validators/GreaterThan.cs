using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uamazing.ConfValidatation.Core.Validators
{
    /// <summary>
    /// 大于
    /// </summary>
    public class GreaterThan : InRange
    {
        public GreaterThan() { }

        public GreaterThan(int value)
        {
            Min = value;
            LeftClosed = false;
        }
    }
}
