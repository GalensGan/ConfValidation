using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uamazing.ConfValidatation.Core.Attributes;
using Uamazing.ConfValidatation.Core.Utils;
using Uamazing.ConfValidatation.Core.ValidatorWrappers;

namespace Uamazing.ConfValidatation.Core.Validators
{
    /// <summary>
    /// 全局的 validator 容器
    /// </summary>
    public class ValidatorManager
    {
        private readonly Dictionary<string, Type> _validatorTypes = new Dictionary<string, Type>();
        private readonly Dictionary<string, Validator> _validators = new Dictionary<string, Validator>();

        private ValidatorManager()
        {
            // 初始化时，增加系统 validator
            var vlidatorTypes = GetValidValidatorTypes(Assembly.GetExecutingAssembly());

            foreach (var vdType in vlidatorTypes)
            {
                AddValidator(vdType);
            }
        }

        private IEnumerable<Type> GetValidValidatorTypes(Assembly assembly)
        {
            var baseType = typeof(Validator);
            return assembly.GetTypes()
                .Where(x => !x.IsAbstract && baseType.IsAssignableFrom(x))
                .Where(x => AttributeHelper.GetCustomAttribute<AnonymousValidatorAttribute>(x) == null);
        }

        private static ValidatorManager _instance;
        public static ValidatorManager Instance
        {
            get
            {
                if (_instance == null) _instance = new ValidatorManager();
                return _instance;
            }
        }


        #region  添加验证器方法
        /// <summary>
        /// 检验验证器名称是否已经存在
        /// </summary>
        /// <returns></returns>
        public bool ExistValidator(string validatorName)
        {
            return _validators.ContainsKey(validatorName) || _validatorTypes.ContainsKey(validatorName);
        }

        /// <summary>
        /// 向容器中添加验证器
        /// </summary>
        /// <param name="type">验证器类型</param>
        /// <param name="validatorName">验证器名称</param>
        public void AddValidator(Type type)
        {
            var constructor = type.GetConstructor(Type.EmptyTypes);
            if (constructor == null) throw new ArgumentException($"{type} must have a parameterless constructor");

            // 实例化 type,获取里面的 Name
            var instance = (Validator)Activator.CreateInstance(type);
            var name = instance.Name;
            if (string.IsNullOrEmpty(instance.Name)) name = type.Name;
            AddValidator(name, type);
        }

        /// <summary>
        /// 添加自定义的验证器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="validatorName"></param>
        public void AddValidator<T>() where T : Validator
        {
            AddValidator(typeof(T));
        }

        /// <summary>
        /// 添加当前程序集中所有自定义的验证器
        /// </summary>
        public void AddAllValidatorInCurrentAssembly()
        {
            var vlidatorTypes = GetValidValidatorTypes(Assembly.GetCallingAssembly());

            foreach (var vdType in vlidatorTypes)
            {
                AddValidator(vdType);
            }
        }

        /// <summary>
        /// 指定名称添加验证器
        /// </summary>
        /// <param name="validatorName"></param>
        /// <param name="validatorType"></param>
        public void AddValidator(string validatorName,Type validatorType)
        {
            if (ExistValidator(validatorName)) throw new ArgumentException($"the validator named {validatorName} already exists");
            _validatorTypes.Add(validatorName, validatorType);
        }

        /// <summary>
        /// 通过名称添加验证器实例组<br/>
        /// 警告：该方法添加的验证器是单例模式，仅限添加验证器组
        /// </summary>
        /// <param name="validatorName"></param>
        /// <param name="validator"></param>
        public void AddValidator(string validatorName, Container validatorContainer)
        {
            if (ExistValidator(validatorName)) throw new ArgumentException($"the validator named {validatorName} already exists");
            _validators.Add(validatorName, validatorContainer);
        }

        #endregion

        /// <summary>
        /// 通过名称获取 validator
        /// </summary>
        /// <param name="validatorName"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public Validator GetValidatorByName(string validatorName)
        {
            // 从全局容器中获取
            if (_validatorTypes.TryGetValue(validatorName, out Type validatorType))
            {
                // 反射 validator
                var instance = Activator.CreateInstance(validatorType);
                if (instance == null) throw new NullReferenceException($"can not create instance of {validatorType}");

                return (Validator)instance;
            }

            if(_validators.TryGetValue(validatorName,out Validator validator))
            {
                return validator;
            }

            throw new ArgumentNullException($"can not find any validator named {validatorName}");
        }
    }
}
