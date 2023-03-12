using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uamazing.ConfValidatation.Core.Validators
{
    /// <summary>
    /// 小于
    /// </summary>
    public class LessThan:InRange
    {
        public LessThan() { }
        public LessThan(int value)
        {
            Max = value;
            RightClosed = false;
        }
    }
}
