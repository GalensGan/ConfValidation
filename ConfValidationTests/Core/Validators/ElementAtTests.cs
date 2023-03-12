using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uamazing.ConfValidatation.Core.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uamazing.ConfValidatation.Core.Entrance;

namespace Uamazing.ConfValidatation.Core.Validators.Tests
{
    [TestClass()]
    public class ElementAtTests
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
                    Nationality = "China",
                    City = "Shanghai"
                },
                Hobbies = new[] { "football", "basketball" },
                Children = new[] {
                    new {
                        Name = "Jack",
                        Age = 10,
                        Address = new
                            {
                                Nationality = "China",
                                City = "Shanghai"
                            }
                    },
                    new {
                        Name = "Timi",
                        Age=4,
                        Address = new
                            {
                                Nationality = "China",
                                City = "Shanghai"
                            }
                    }
                }
            };


            var vdResult1 = user.Validate(new VdObj()
            {
                { "$Name",new ElementAt(0){ VdNames.IsString} }
            });
            Assert.IsTrue(vdResult1.NotOk);

            var vdResult2 = user.Validate(new VdObj()
            {                
                { "$Children",new ElementAt(0){
                    new VdObj()
                    {
                        {"$Name",VdNames.IsString }
                    },
                    { "$Age",VdNames.IsString}
                } },
            });
            Assert.IsTrue(vdResult2.NotOk);

            var vdResult3 = user.Validate(new VdObj()
            {               
                { "$Children",new ElementAt(1){
                    new VdObj()
                    {
                        {"$Name",VdNames.IsString }
                    },
                    { "$Age",new IsNumber()}
                } },
            });
            Assert.IsTrue(vdResult3.Ok);
        }
    }
}