using System;

namespace CodeJam.Extensibility
{
	[AttributeUsage(AttributeTargets.Class)]
	public class SimpleExtensionAttribute : Attribute
	{
		public bool Prop { get; set; }
	}
}