using System;

using JetBrains.Annotations;

namespace CodeJam.Extensibility.Demos
{
	[AttributeUsage(AttributeTargets.Class)]
	[MeansImplicitUse]
	public class SimpleExtensionAttribute : Attribute
	{
		public SimpleExtensionAttribute(string name)
		{
			Name = name;
		}

		public string Name { get; }
	}
}
