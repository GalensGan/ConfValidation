using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uamazing.ConfValidatation.Core.Results;
using Uamazing.ConfValidatation.Core.Utils;
using Uamazing.ConfValidatation.Core.Validators;
using Uamazing.ConfValidatation.Core.ValidatorWrappers;

namespace Uamazing.ConfValidatation.Core.Validators
{
    /// <summary>
    /// 容器型验证器
    /// </summary>
    public abstract class Container : Validator, IEnumerable<Validator>
    {
        private readonly List<Validator> _validators = new List<Validator>();

        #region IEnumberable 实现
        public IEnumerator<Validator> GetEnumerator()
        {
            return _validators.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region 增加验证器
        /// <summary>
        /// 添加验证器
        /// </summary>
        /// <param name="validator"></param>
        /// <param name="failureMessage"></param>
        public void Add(Validator validator, string failureMessage = "")
        {
            // 将错误消息保存到验证器中
            if (!string.IsNullOrEmpty(failureMessage)) validator.FailureMessage = failureMessage;

            _validators.Add(validator);
        }

        /// <summary>
        /// 如果包含 $ 前缀，表示为字段路径
        /// 否则被认为是字符串值
        /// </summary>
        /// <param name="valueOrPath"></param>
        /// <param name="validator"></param>
        /// <param name="failureMessage"></param>
        public void Add(string valueOrPath, Validator validator, string failureMessage = "")
        {
            // 单独处理字符串输入
            // 说明是路径
            if (!valueOrPath.StartsWith("$"))
            {
                Add(new StringValidatorWrapper(valueOrPath, validator));
                return;
            }

            // 将前面的 $ 去掉
            validator.Path = valueOrPath[1..];
            Add(validator, failureMessage);
        }

        /// <summary>
        /// 通过 validatorName 来自动获取验证器
        /// </summary>
        /// <param name="fieldPathOrValue"></param>
        /// <param name="validatorName"></param>
        /// <param name="failureMessage"></param>
        public void Add(string fieldPathOrValue, string validatorName, string failureMessage = "")
        {
            // 将 name 转换成实际的 validator
            var validator = ValidatorManager.Instance.GetValidatorByName(validatorName);

            Add(fieldPathOrValue, validator, failureMessage);
        }

        /// <summary>
        /// 通过名称批量添加验证
        /// </summary>
        /// <param name="fieldPathOrValue"></param>
        /// <param name="validatorNames"></param>
        /// <param name="failureMessage"></param>
        public void Add(string fieldPathOrValue, Validator[] validators, string failureMessage = "")
        {
            // 从全局容器中获取
            var andValidator = new And();
            foreach (var validator in validators)
            {
                andValidator.Add(validator);
            }

            Add(fieldPathOrValue, andValidator, failureMessage);
        }

        /// <summary>
        /// 通过路径添加自定义表达式的验证器
        /// 当以 $ 开头时，说明是路径，此时传入的自定义函数体类型是
        /// </summary>
        /// <param name="valueOrPath">实际值或者路径</param>
        /// <param name="validator"></param>
        /// <param name="failureMessage"></param>
        public void Add<T>(string valueOrPath, Func<T, bool> customValidator, string failureMessage = "")
        {
            var function = new Function<T>(customValidator);
            Add(valueOrPath, function, failureMessage);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lambdaExpression"></param>
        /// <param name="validator"></param>
        /// <param name="failureMessage"></param>
        public void Add<T>(Expression<Func<T>> lambdaExpression, Validator validator, string failureMessage = "")
        {
            var path = PathHelper.ResovePathStartWithDollarSign(lambdaExpression);
            Add(path, validator, failureMessage);
        }

        /// <summary>
        /// 通过表达式批量添加验证
        /// </summary>
        /// <param name="fieldPathOrValue"></param>
        /// <param name="validatorNames"></param>
        /// <param name="failureMessage"></param>
        public void Add<T>(Expression<Func<T>> lambdaExpression, Validator[] validators, string failureMessage = "")
        {
            // 从全局容器中获取
            var path = PathHelper.ResovePathStartWithDollarSign(lambdaExpression);
            Add(path, validators, failureMessage);
        }

        /// <summary>
        /// 添加自定义表达式的验证器
        /// </summary>
        /// <param name="value">验证实际的值</param>
        /// <param name="validator"></param>
        /// <param name="failureMessage"></param>
        public void Add<T>(T value, Func<T, bool> customValidator, string failureMessage = "")
        {
            // 当 value 是字符串时，调用字符串型的重载

            var function = new Function<T>(customValidator);
            Add(value, function, failureMessage);
        }      

        /// <summary>
        /// 通过表达式树添加自定义表达式的验证器
        /// 从表达式树中获取路径
        /// </summary>
        /// <param name="lambdaExpression">表达树</param>
        /// <param name="validator"></param>
        /// <param name="failureMessage"></param>
        public void Add<T>(Expression<Func<T>> lambdaExpression, Func<T, bool> customValidator, string failureMessage = "")
        {
            var path = PathHelper.ResovePathStartWithDollarSign(lambdaExpression);
            var function = new Function<T>(customValidator);
            Add(path, function, failureMessage);
        }

        /// <summary>
        /// 添加特定值的验证器
        /// 该验证器只能在当前使用，因为其值已经固定无法修改了
        /// </summary>
        /// <param name="value">验证实际的值</param>
        /// <param name="validator"></param>
        /// <param name="failureMessage"></param>
        public void Add<T>(T value, Validator validator, string failureMessage = "")
        {
            // 提前将类型转为指定类型，避免后续重复拆箱
            // 对固定值进行包装，以便调用时传入固定值
            if (value is int intValue)
            {
                Add(new IntValidatorWrapper(intValue, validator), failureMessage);
                return;
            }

            if (value is long longValue)
            {
                Add(new LongValidatorWrapper(longValue, validator), failureMessage);
                return;
            }

            if (value is decimal decimalValue)
            {
                Add(new DecimalValidatorWrapper(decimalValue, validator), failureMessage);
                return;
            }

            if (value is double doubleValue)
            {
                Add(new DoubleValidatorWrapper(doubleValue, validator), failureMessage);
                return;
            }

            if (value is bool boolValue)
            {
                Add(new BoolValidatorWrapper(boolValue, validator), failureMessage);
                return;
            }

            // 字符串在字符串重载的那个方法中处理
#pragma warning disable CS8604 // 引用类型参数可能为 null。
            Add(new ObjectValidatorWrapper(value, validator), failureMessage);
#pragma warning restore CS8604 // 引用类型参数可能为 null。
        }

        /// <summary>
        /// 添加特定值的验证器
        /// 该验证器只能在当前使用，因为其值已经固定无法修改了
        /// </summary>
        /// <param name="value">验证实际的值</param>
        /// <param name="validator"></param>
        /// <param name="failureMessage"></param>
        public void Add<T>(T value, Validator[] validators, string failureMessage = "")
        {
            var and = new And();
            foreach (var validator in validators) and.Add(validator);
            Add(value, and, failureMessage);
        }
        #endregion
    }
}
