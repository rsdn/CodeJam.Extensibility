using System;

using JetBrains.Annotations;

namespace Rsdn.SmartApp.Demos
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	[MeansImplicitUse]
	public class FruitAttribute : Attribute
	{
		private readonly string _name;

		public FruitAttribute(string name)
		{
			_name = name;
		}

		public string Name
		{
			get { return _name; }
		}
	}
}
