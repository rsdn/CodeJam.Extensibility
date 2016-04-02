using System;

namespace Rsdn.SmartApp.Extensibility.Registration
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	internal class TestNamedElementAttribute : NamedElementAttribute
	{
		public TestNamedElementAttribute(string name) : base(name)
		{
		}
	}
}