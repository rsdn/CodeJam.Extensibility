using System;

using JetBrains.Annotations;

using ServiceContainer = CodeJam.Services.ServiceContainer;

namespace CodeJam.Extensibility.Demos
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
		public string Name => "Apple";
	}

	[Fruit]
	public class Pear : IFruit
	{
		public string Name => "Pear";
	}

	[Fruit]
	public class Peach : IFruit
	{
		public string Name => "Peach";
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
			var root = new ServiceContainer();
			var strategy = root.CreateStrategy<FruitInfo, FruitAttribute>(type => new FruitInfo(type));
			root.ScanExtensions(strategy);
			var cache = new ExtensionsCache<FruitInfo, IFruit>(root);
			foreach (var fruit in cache.GetAllExtensions())
				Console.WriteLine(fruit.Name);
		}
	}
}


