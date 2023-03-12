using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uamazing.ConfValidatation.Core.Results;

namespace Uamazing.ConfValidatation.Core.Validators
{
    public class InRange : IsNumber
    {
        /// <summary>
        /// 范围左侧是闭区间
        /// </summary>
        public bool LeftClosed { get; set; } = true;

        /// <summary>
        /// 范围右侧是开闭间
        /// </summary>
        public bool RightClosed { get; set; } = true;

        public double Max { get; set; } = double.PositiveInfinity;
        public double Min { get; set; } = double.NegativeInfinity;

        protected override ValidateResult ValidateCore(object value, string parentFullPath, string fullPath)
        {
            var baseResult = base.ValidateCore(value, parentFullPath,fullPath);
            if (!baseResult) return baseResult;


            // 比较大小
            // 该转换未进行精度测试
            var valueResult = double.Parse(value.ToString());

            var innerError = $"value is limited to range of {(LeftClosed ? '[' : '(')}{Max},{Min}{(RightClosed ? ']' : ')')}, but accept {valueResult}";
            if (valueResult > Max || valueResult < Min) return new FailureResult(fullPath, FailureMessage, innerError);

            // 判断是否相等
            if(!LeftClosed && Math.Abs(Max-valueResult)<1e-10)return new FailureResult(fullPath, FailureMessage, innerError);

            if (!RightClosed && Math.Abs(valueResult - Min) < 1e-10) return new FailureResult(fullPath, FailureMessage, innerError);

            return new OkResult(fullPath);
        }
    }
}
