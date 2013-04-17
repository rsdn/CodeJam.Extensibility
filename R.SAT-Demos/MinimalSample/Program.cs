using System;

using JetBrains.Annotations;

namespace Rsdn.SmartApp.Demos
{
	public interface IFruit
	{
		string Name { get; }
	}

	[MeansImplicitUse]
	[BaseTypeRequired(typeof(IFruit))]
	public class FruitAttribute : Attribute { }

	[Fruit]
	public class Apple : IFruit
	{
		public string Name
		{
			get { return "Apple"; }
		}
	}

	[Fruit]
	public class Pear : IFruit
	{
		public string Name
		{
			get { return "Pear"; }
		}
	}

	[Fruit]
	public class Peach : IFruit
	{
		public string Name
		{
			get { return "Peach"; }
		}
	}

	public class FruitInfo : ElementInfo
	{
		public FruitInfo([NotNull] Type type)
			: base(type)
		{ }
	}

	class Program
	{
		static void Main()
		{
			var root = new ServiceManager(true);
			var strategy = root.CreateStrategy<FruitInfo, FruitAttribute>(type => new FruitInfo(type));
			root.ScanExtensions(strategy);
			var cache = new ExtensionsCache<FruitInfo, IFruit>(root);
			foreach (var fruit in cache.GetAllExtensions())
				Console.WriteLine(fruit.Name);
		}
	}
}


