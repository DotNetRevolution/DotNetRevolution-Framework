using DotNetRevolution.Core.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotNetRevolution.Core.Tests.Base
{
    [TestClass]
    public class SequentialGuidTests
    {
        [TestInitialize]
        public void Init()
        {
        }

        [TestMethod]
        public void AssertNoClash()
        {
            var set1 = new HashSet<Guid>();
            var set2 = new HashSet<Guid>();
            var set3 = new HashSet<Guid>();
            var set4 = new HashSet<Guid>();

            var tasks = new List<Task>();

            tasks.Add(new Task(() => CreateGuids(set1, 100000)));
            tasks.Add(new Task(() => CreateGuids(set2, 100000)));
            tasks.Add(new Task(() => CreateGuids(set3, 100000)));
            tasks.Add(new Task(() => CreateGuids(set4, 100000)));

            tasks.ForEach(task => task.Start());

            Task.WaitAll(tasks.ToArray());

            foreach (var guid in set2)
            {
                Assert.IsTrue(set1.Add(guid));
            }

            foreach (var guid in set3)
            {
                Assert.IsTrue(set1.Add(guid));
            }

            foreach (var guid in set4)
            {
                Assert.IsTrue(set1.Add(guid));
            }
        }

        private void CreateGuids(HashSet<Guid> set, int count)
        {
            for (var i = 0; i < count; i++)
            {
                Assert.IsTrue(set.Add(SequentialGuid.Create()));
            }
        }
    }
}
