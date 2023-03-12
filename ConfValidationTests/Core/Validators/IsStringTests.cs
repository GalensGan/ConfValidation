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
    public class IsStringTests
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
                Hobbies = new[] { "football", "basketball" }
            };

            var vdResult1 = user.Name.Validate(new IsString("value is not string"));
            Assert.IsTrue(vdResult1.Ok);

            var vdResult2 = user.Age.Validate(new IsString("value is not string"));
            Assert.IsFalse(vdResult2.Ok);
            Assert.AreEqual(vdResult2.Message, "value is not string");

            var vdResult3 = user.Validate(new VdObj()
            {
                { ()=>user.Name,x=>x.Length==6},
                { ()=>user.Name,new IsString()},
                { user.Name,VdNames.IsString},
            });
            Assert.IsTrue(vdResult3);
        }
    }
}