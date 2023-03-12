using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uamazing.ConfValidatation.Core.Results
{
    /// <summary>
    /// 验证结果
    /// </summary>
    public class ValidateResult
    {
        public ValidateResult(bool ok, string fullPath)
        {
            Ok = ok;
            FullPath = fullPath;
        }
        public ValidateResult(bool ok, string fullPath, string message,string innerMessage) : this(ok, fullPath)
        {
            Message = message;
            InnerMessage = innerMessage;
        }

        public bool Ok { get; private set; }

        public bool NotOk => !Ok;

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 系统内部的错误方法
        /// </summary>
        public string InnerMessage { get; set; }

        /// <summary>
        /// 出错的字段全路径
        /// </summary>
        public string FullPath { get; set; }

        #region 与 bool 的隐式转换
        public static implicit operator bool(ValidateResult validator)
        {
            return validator.Ok;
        }

        public static explicit operator ValidateResult(bool ok)
        {
            // 从全局容器中获取
            return new ValidateResult(ok,"");
        }
        #endregion
    }
}
