using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uamazing.ConfValidatation.Core.Attributes
{
    /// <summary>
    /// Marked as an unnamed validator
    /// Anonymous validators cannot be accessed by string
    /// </summary>
    public class AnonymousValidatorAttribute:Attribute
    {
        public bool NotStringable { get; } = false;
    }
}
