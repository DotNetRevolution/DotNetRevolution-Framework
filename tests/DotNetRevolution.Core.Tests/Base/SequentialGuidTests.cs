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
            var set0 = new HashSet<Guid>();
            var set1 = new HashSet<Guid>();
            var set2 = new HashSet<Guid>();
            var set3 = new HashSet<Guid>();
            var set4 = new HashSet<Guid>();
            var set5 = new HashSet<Guid>();
            var set6 = new HashSet<Guid>();
            var set7 = new HashSet<Guid>();
            var set8 = new HashSet<Guid>();
            var set9 = new HashSet<Guid>();            

            var tasks = new List<Task>();

            tasks.Add(new Task(() => CreateGuids(set0, 10000)));
            tasks.Add(new Task(() => CreateGuids(set1, 10000)));
            tasks.Add(new Task(() => CreateGuids(set2, 10000)));
            tasks.Add(new Task(() => CreateGuids(set3, 10000)));
            tasks.Add(new Task(() => CreateGuids(set4, 10000)));
            tasks.Add(new Task(() => CreateGuids(set5, 10000)));
            tasks.Add(new Task(() => CreateGuids(set6, 10000)));
            tasks.Add(new Task(() => CreateGuids(set7, 10000)));
            tasks.Add(new Task(() => CreateGuids(set8, 10000)));
            tasks.Add(new Task(() => CreateGuids(set9, 10000)));            

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

            foreach (var guid in set5)
            {
                Assert.IsTrue(set1.Add(guid));
            }

            foreach (var guid in set6)
            {
                Assert.IsTrue(set1.Add(guid));
            }

            foreach (var guid in set7)
            {
                Assert.IsTrue(set1.Add(guid));
            }

            foreach (var guid in set8)
            {
                Assert.IsTrue(set1.Add(guid));
            }

            foreach (var guid in set9)
            {
                Assert.IsTrue(set1.Add(guid));
            }

            foreach (var guid in set0)
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
