using System;

using JetBrains.Annotations;

namespace Rsdn.SmartApp.Demos
{
	[AttributeUsage(AttributeTargets.Class)]
	[MeansImplicitUse]
	public class SimpleExtensionAttribute : Attribute
	{
		private readonly string _name;

		public SimpleExtensionAttribute(string name)
		{
			_name = name;
		}

		public string Name
		{
			get { return _name; }
		}
	}
}
