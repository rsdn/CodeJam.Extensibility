using System;

using CodeJam.Extensibility.Registration;

namespace CodeJam.Extensibility.Demos.FruitModel
{
	public class FruitInfo : IKeyedElementInfo<string>
	{
		public FruitInfo(string name, Type type)
		{
			Name = name;
			Type = type;
		}

		public string Name { get; }

		public Type Type { get; }

		string IKeyedElementInfo<string>.Key => Name;
	}
}
