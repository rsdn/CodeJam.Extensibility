using System;

using JetBrains.Annotations;

namespace CodeJam.Extensibility.Demos.FruitModel
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	[MeansImplicitUse]
	public class FruitAttribute : Attribute
	{
		public FruitAttribute(string name)
		{
			Name = name;
		}

		public string Name { get; }
	}
}
