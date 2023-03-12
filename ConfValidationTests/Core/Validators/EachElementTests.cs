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
    public class EachElementTests
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
                { "$Name",new EachElement(){ VdNames.IsString} },

                { "$Children",new EachElement(){
                    new VdObj()
                    {
                        {"$Name",VdNames.IsString }
                    },
                    { "$Age",new IsNumber()}
                } },

                { "$Children[]",new EachElement(){
                    new VdObj()
                    {
                        {"$Name",VdNames.IsString }
                    },
                    { "$Age",new IsNumber()}
                } },
            });
            Assert.IsTrue(vdResult1.Ok);
        }
    }
}