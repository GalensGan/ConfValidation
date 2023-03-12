using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uamazing.ConfValidatation.Core.Results;

namespace Uamazing.ConfValidatation.Core.Validators
{
    /// <summary>
    /// 逻辑判断
    /// 对一种当前结果执行多种验证
    /// </summary>
    public class Logic : Container
    {
        public virtual LogicType LogicType { get; set; }
        protected override ValidateResult ValidateCore(object value, string parentFullPath,string fullPath)
        {
            if (LogicType == LogicType.None) throw new ArgumentException($"{nameof(LogicType)} should not be a default value of None");

            return RunLogicValidate(value, parentFullPath, fullPath);
        }

        protected ValidateResult RunLogicValidate(object valueResult, string parentFullPath, string fullPath)
        {
            if (LogicType == LogicType.And)
            {
                foreach (var validator in this)
                {
                    var subResult = validator.Validate(valueResult,fullPath);
                    if (subResult.NotOk) return subResult;
                }

                return new OkResult(parentFullPath);
            }

            if (LogicType == LogicType.Or)
            {
                foreach (var validator in this)
                {
                    var subResult = validator.Validate(valueResult, fullPath);
                    if (subResult.Ok) return subResult;
                }

                return new FailureResult(parentFullPath, FailureMessage, "");
            }

            throw new ArgumentOutOfRangeException($"not support logical type of {LogicType}");
        }
    }

    public enum LogicType
    {
        None,
        And,
        Or,
    }
}
