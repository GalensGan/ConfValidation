using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uamazing.ConfValidatation.Core.Validators
{
    public class Or:Logic
    {
        public override LogicType LogicType { get; set; } = LogicType.Or;
    }
}
