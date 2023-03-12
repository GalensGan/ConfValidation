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
    public class IsNumberTests
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
                Children = new[] { new {
                    Name = "Jack",
                    Age = 10
                },new {  Name = "Timi",
                    Age = 5} }
            };

            var vdResult1 = user.Name.Validate(new IsString("value is not string"));
            Assert.IsTrue(vdResult1.Ok);

            var vdResult2 = user.Age.Validate(new IsString("value is not string"));
            Assert.IsFalse(vdResult2.Ok);
            Assert.AreEqual(vdResult2.Message, "value is not string");
        }
    }
}