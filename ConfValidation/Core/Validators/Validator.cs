using Pather.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uamazing.ConfValidatation.Core.Results;

namespace Uamazing.ConfValidatation.Core.Validators
{
    /// <summary>
    /// 验证器基类
    /// 警告：不要在子类中保存任何实例化的数据或者过程数据，这会导致在重用验证器对象时，产生非预期结果
    /// 可以使用 ValidatorWrapper 来保存特定值
    /// </summary>
    public  abstract partial class Validator
    {
        #region 属性
        /// <summary>
        /// 验证器名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 失败的消息
        /// </summary>
        public string FailureMessage { get; set; }

        /// <summary>
        /// 路径
        /// 为 $ 时，代表使用传入的元素的值
        /// 为 [] 时，表示所有的集合
        /// 为 [1] 时，表示索引为 1 的单个值
        /// 为 ["1"] 时，表示键为 "1" 的字典
        /// </summary>
        public string Path { get; set; } = "$";

        /// <summary>
        /// 是否必须
        /// 默认为是
        /// </summary>
        public bool Required { get; set; } = true;
        #endregion


        #region 构造函数
        public Validator() { }
        public Validator(string failureMessage) { FailureMessage = failureMessage; }
        #endregion


        #region 与 string 的隐式转换    
        /// <summary>
        /// 将字符串隐式转换成 validator
        /// </summary>
        /// <param name="validatorName"></param>

        public static implicit operator Validator(string validatorName)
        {
            return ValidatorManager.Instance.GetValidatorByName(validatorName);
        }
       

        // 显示转换成字符串
        public static explicit operator string(Validator validator)
        {
            var name = validator.Name;
            if (string.IsNullOrEmpty(name)) name = validator.GetType().Name;
            return name;
        }

        public static explicit operator Validator(string[] validatorNames)
        {
            // 从全局容器中获取
            var andValidator = new And();
            foreach (var validatorName in validatorNames)
            {
                var validator = ValidatorManager.Instance.GetValidatorByName(validatorName);
                if (validator == null) continue;

                andValidator.Add(validator);
            }

            return andValidator;
        }
        #endregion
    }
}
