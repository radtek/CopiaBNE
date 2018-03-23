using AllInTriggers.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace BNE.Services.Test
{
    [TestClass]
    public class ReflectionTest
    {
        [TestMethod]
        public void CovariantTest()
        {
            var d = EqualityComparer<Exception>.Default;

            IEqualityComparer<NullReferenceException> s = d;
        }

        [TestMethod]
        public void EventMediatorTest()
        {
            var med = new ReactSharedEventMediator<int>();

            var routed = med.CreateIfNotExists<Tuple<int, string>, string>(1, a => a.Item2.ToString(), StringComparer.OrdinalIgnoreCase);
            Assert.IsTrue(routed != null);

            var second = med.CreateIfNotExists<Tuple<int, string>, string>(1, a => a.Item2.ToString());
            Assert.IsTrue(second != null);

            Assert.IsTrue(routed == second);

            var multi = new ReplaySubject<ShootResultArgs<Tuple<int, string>>>();
            var asd = routed.Broadcast().Where(a => a.OriginalEvent.Item2.ToString() == "asd").Multicast(multi).RefCount().Subscribe();

            bool firedAsd = false;
            var specific = routed.Observe("asd").Subscribe(a =>
            {
                Trace.WriteLine(a);
                firedAsd = true;
            });

            med.PublishIfExists(1, new Tuple<int, string>(234897, "asd"));

            var reflect = med.GetConsumer<Tuple<int,string>>(1);

            specific.Dispose();

            Assert.IsTrue(firedAsd);

            bool firedAsd2 = false;
            routed.Observe("asd").Subscribe(a =>
            {
                Trace.WriteLine(a);
                firedAsd2 = true;
            });

            med.PublishIfExists(1, new Tuple<int, string>(234897, "ASD"));
            Assert.IsTrue(firedAsd2);

            bool firedKkk = false;
            routed.Observe("kkk").Subscribe(a =>
                {
                    Trace.WriteLine(a);
                    firedKkk = true;
                });
            Assert.IsTrue(!firedKkk);
            multi.OnCompleted();
            Assert.IsTrue(multi.Count().Last() == 1);


        }
    }
}
