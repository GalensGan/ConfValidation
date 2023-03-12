using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uamazing.ConfValidatation.Core.Results;

namespace Uamazing.ConfValidatation.Core.Validators
{
    /// <summary>
    /// 字符串验证器
    /// </summary>
    public class IsString : Validator
    {
        public IsString() { }

        public IsString(string message):base(message) { }

        /// <summary>
        /// 最大长度
        /// </summary>
        public int MaxLength { get; set; } = -1;

        /// <summary>
        /// 最小长度
        /// </summary>
        public int MinLength { get; set; } = -1;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">T 可能是对象，也可能是具体的字符串值</typeparam>
        /// <param name="value"></param>
        /// <param name="parentFullPath"></param>
        /// <returns></returns>
        protected override ValidateResult ValidateCore(object value, string parentFullPath, string fullPath)
        {
            // 获取值           
            if (!(value is string stringValue)) return new FailureResult(fullPath, FailureMessage, $"value require string, but acccept {value.GetType().Name}");
            
            // 验证最大长度
            if (MaxLength > -1)
            {
                bool isLowerThan = stringValue.Length <= MaxLength;
                if (!isLowerThan)
                {

                    var innerMessage1 = $"{fullPath} is limited to a maximum length of 10 ${MaxLength},but accept {stringValue.Length}";
                    return new FailureResult(fullPath, FailureMessage, innerMessage1);
                }
            }

            // 验证最小长度
            bool isGreaterThan = stringValue.Length >= MinLength;
            if (!isGreaterThan)
            {
                var innerMessage2 = $"{fullPath}  is limited to a minimum length {MinLength},but accept {stringValue.Length}";
                return new FailureResult(fullPath, FailureMessage, innerMessage2);
            }

            return new OkResult(fullPath);
        }
    }
}
