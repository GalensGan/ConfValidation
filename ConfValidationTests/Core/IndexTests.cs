using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uamazing.ConfValidatation.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uamazing.ConfValidatation.Core.Validators;
using Uamazing.ConfValidatation.Core.Entrance;
using ConfValidatationTests.Core.Utils.PathModel;
using Uamazing.ConfValidation.Core.Validators;

namespace Uamazing.ConfValidatation.Core.Tests
{
    [TestClass()]
    public class IndexTests
    {
        [TestMethod()]
        public void ValidateTest()
        {
            var user = new
            {
                Name = "galens",
                Age = 30,
                Address = new
                {
                    Nationality = "China1",
                    City = "Shanghai1"
                },
                Hobbies = new[] { "football", "basketball" },
                Children = new[] {
                    new {
                        Name = "Jack",
                        Age = 10,
                        Address = new
                            {
                                Nationality = "China2",
                                City = "Shanghai2"
                            }
                    },
                    new {
                        Name = "Timi",
                        Age=4,
                        Address = new
                            {
                                Nationality = "China3",
                                City = "Shanghai3"
                            }
                    }
                }
            };

            var vdResult1 = user.Validate(new VdObj
            {
                { "$Children[].Address.City",new EachElement(){ VdNames.IsString} }
            });
            Assert.IsTrue(vdResult1);


            // 测试表达式树
            var vdNationality = user.Validate(new VdObj
            {
                { () => user.Address.Nationality, new IsString()}
            });
            Assert.IsTrue(vdNationality.Ok);


            var vdResult2 = user.Validate(new VdObj()
            {
                {"$Name",new NotNullOrEmpty()},

                {"$Name",x=>x.Length>0},

                {user.Age,new And(){
                        new IsNumber(),
                        "LessThan"
                    }
                },

                {user.Age,new Validator[]{ VdNames.IsNumber}},

                { user.Name,new IsString()},

                { ()=>user.Name,new IsString()},

                // 集合
                { "$Children[].Address.City",new EachElement(){ new IsString()} },

                { ()=>user.Address.City,new IsString(){ MaxLength=10,MinLength=4 } },

                { ()=>user.Address,new VdObj(){
                    {"$City",VdNames.IsString}
                }},

                {()=>user.Children,new EachElement(){} }
            });
            Assert.IsTrue(vdResult2.Ok);

            user.Name.Validate(VdNames.IsString);
            user.Age.Validate(new Validator[] { VdNames.IsString, VdNames.IsNumber });
        }

        [TestMethod()]
        public void ValidateSampleTest()
        {
            var myClass = new Class()
            {
                Name = "class 1",
                Order = 1,
                // 嵌套对象
                Address = new Address()
                {
                    Email = "abc@china.com"
                },
                // 列表
                Students = new List<Student>()
                {
                   new Student(){ Name = "Jack",
                       // 数组
                       Subjects = new Subject[] {
                            new Subject(){Name = "English",Score = 80},
                            new Subject(){Name = "Math",Score=90}
                       },
                       // 字典
                       Skills = new Dictionary<string, double>()
                       {
                           { "cook",80 },
                           { "football",60}
                       },
                       // 字典
                       Scores = new Dictionary<string, Subject>()
                       {
                           {"English", new Subject(){Score = 80} },
                           {"Math", new Subject(){Score=90} }
                       }
                   },
                   new Student(){ Name = "Timi",
                       Subjects = new Subject[] {
                            new Subject(){Name = "English",Score = 80},
                            new Subject(){Name = "Math",Score=90}
                       },
                       Skills = new Dictionary<string, double>()
                       {
                           { "cook",80 },
                           { "football",60}
                       },
                        Scores = new Dictionary<string, Subject>()
                       {
                           {"English", new Subject(){Score = 80} },
                           {"Math", new Subject(){Score=90} }
                       }
                   }
                }
            };

            ;

            // 整体一次性验证
            var vdResult = myClass.Validate(new VdObj // 顶层是对象类型，使用 VdObj 验证器包裹其它验证器
            {
                //// 通过验证器名称 IsString 来验证数据 myClass.Name
                //{ "$Name","IsString"},
                //// 原理同上，VdNames 中定义了一些系统验证器常量
                //{ "$Name",VdNames.IsString},
                //// 通过传入验证器来验证
                //{ "$Name",new IsString()},
                //// 自定义错误消息
                //{ "$Name",new IsString(),"myClass.Name 不是字符串"},
                //// 自定义验证器初始化参数
                //{ "$Name",new IsString(){
                //    MaxLength = 10,
                //    Required = true,
                //    },"myClass.Name 的最大长度为 10"
                //},
                // 传入 Func<T> 进行自主验证
                { "$Name",x=>x=="class 1"},
                //// 通过表达方式传入验证值的路径，不用手动输入字符串，减少输入失误
                //{ ()=>myClass.Name,VdNames.IsString},
                //{ ()=>myClass.Name,x=>x=="class 1"},
                //// 同时输入多个验证器
                //{ ()=>myClass.Order,new Validator[]{
                //    VdNames.IsNumber,
                //    new LessThan(2),
                //    new InRange(0,10)
                //} },
                //// 逻辑且
                //{ "$Order",new And(){
                //    VdNames.IsNumber,
                //    new LessThan(2),
                //    new InRange(0,10)
                //} },
                //// 逻辑或
                //{ "$Order",new Or(){
                //    VdNames.IsNumber,
                //    new LessThan(2),
                //    new InRange(0,10)
                //} },
                //// 传入具体的值进行验证，传入具体值时，该验证器组就无法重用，因为特定值已经记录到了验证器中
                //{ myClass.Name,VdNames.IsString},

                //// 下面是嵌套验证
                //// 嵌套验证可以对上述基本使用方式进行任意组合

                //// 验证嵌套对象
                //{ ()=>myClass.Address.Email,new IsString()},
                //{ ()=>myClass.Address,new VdObj{
                //    { "$Emal",new IsString()}
                //} },
                //{ "$Address.Email",new IsString() },

                //// 验证嵌套数组
                //{ ()=>myClass.Students[0].Scores["Math"].Name,new IsString()},
                //// 验证第一个学生所有科目的分数
                //{ "$Students[0].Scores[].Score",new EachElement(){
                //    VdNames.IsNumber,
                //    new GreaterThan(60),
                //} },
                //// 不断向下嵌套验证
                //{ "$Students[0]",new VdObj{
                //    { "$Name",new Equals("Jack")},
                //    { "$Skills[\"cook\"]",new Or(){
                //        VdNames.IsNumber,
                //        new GreaterThan(60)
                //    } }
                //} },
            });

            Assert.IsTrue(vdResult);
            Assert.IsTrue(vdResult.Ok);

            // 多次验证
            var vdResult2 = myClass.Name.Validate(VdNames.IsNumber);
            Assert.IsTrue(vdResult2.Ok);
            var vdResult3 = myClass.Students[0].Scores["Math"].Score.Validate(new And()
            {
                new GreaterThan(60),
                VdNames.IsNumber
            });
            Assert.IsTrue(vdResult3);
        }
    }
}