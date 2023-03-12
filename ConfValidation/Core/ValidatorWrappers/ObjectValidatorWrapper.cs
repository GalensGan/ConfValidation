using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uamazing.ConfValidatation.Core.Results;
using Uamazing.ConfValidatation.Core.Validators;

namespace Uamazing.ConfValidatation.Core.ValidatorWrappers
{
    /// <summary>
    /// 包装对象类型
    /// 对象类型可能为空，此时会有 required 验证，不能重写 Validate 方法
    /// </summary>
    internal class ObjectValidatorWrapper : ValidatorWrapperBase
    {
        private readonly object _value;
        
        public ObjectValidatorWrapper(object value,Validator validator):base(validator)
        {
            _value=value;
        }

        /// <summary>
        /// 使用固定值进行调用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="parentFullPath"></param>
        /// <param name="failureMessage"></param>
        /// <returns></returns>
        protected override ValidateResult ValidateCore(object value, string parentFullPath, string fullPath) 
        {
            return Validator.Validate(_value, parentFullPath);
        }
    }
}
