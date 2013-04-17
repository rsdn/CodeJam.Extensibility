using System;
using System.Reactive.Subjects;
using NUnit.Framework;

namespace Rsdn.SmartApp
{
	[TestFixture]
	public class ObserverTest
	{
		[Test]
		public void FireTest()
		{
			var i = 0;
			var observable = new Subject<int>();

			observable.OnNext(1);
			Assert.AreEqual(0, i);

			using (observable.Subscribe(item => i += item))
			{
				observable.OnNext(1);
				Assert.AreEqual(1, i);

				using (observable.Subscribe(item => i += item))
				{
					observable.OnNext(1);
					Assert.AreEqual(3, i);
				}
			}

			i = 0;
			observable.OnNext(1);
			Assert.AreEqual(0, i);

			observable.OnCompleted();
		}
	}
}