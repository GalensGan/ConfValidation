using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uamazing.ConfValidatation.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfValidatationTests.Core.Utils.PathModel;

namespace Uamazing.ConfValidatation.Core.Utils.Tests
{
    [TestClass()]
    public class PathHelperTests
    {
        [TestMethod()]
        public void ResovePathTest()
        {
            var myclass = new Class()
            {
                Name = "class 1",
                Address = new Address()
                {
                    Email = "abc@china.com"
                },
                Students = new List<Student>()
                {
                   new Student(){ Name = "Jack",
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

            var case1 = PathHelper.ResovePath(() => myclass.Name);
            Assert.AreEqual(case1, "myclass.Name");

            var case2 = PathHelper.ResovePath(() => myclass.Address.Email);
            Assert.AreEqual(case2, "myclass.Address.Email");

            var case3 = PathHelper.ResovePath(() => myclass.Students);
            Assert.AreEqual(case3, "myclass.Students");          

            var case4 = PathHelper.ResovePath(() => myclass.Students[0].Name);
            Assert.AreEqual(case4, "myclass.Students[0].Name");

            var case5 = PathHelper.ResovePath(() => myclass.Students[0].Subjects[0].Name);
            Assert.AreEqual(case5, "myclass.Students[0].Subjects[0].Name");

            var case6 = PathHelper.ResovePath(() => myclass.Students[0].Scores["Math"].Score);
            Assert.AreEqual(case6, "myclass.Students[0].Scores[\"Math\"].Score");
        }
    }
}