using System;

namespace CodeJam.Extensibility
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	internal class TestNamedElementAttribute : NamedElementAttribute
	{
		public TestNamedElementAttribute(string name) : base(name)
		{
		}
	}
}