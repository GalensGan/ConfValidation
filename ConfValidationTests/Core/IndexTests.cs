using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uamazing.ConfValidatation.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uamazing.ConfValidatation.Core.Validators;
using Uamazing.ConfValidatation.Core.Entrance;

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

                {user.Age,new[]{ VdNames.IsNumber}},

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
            user.Age.Validate(new[] { VdNames.IsString, VdNames.IsNumber });
        }
    }
}