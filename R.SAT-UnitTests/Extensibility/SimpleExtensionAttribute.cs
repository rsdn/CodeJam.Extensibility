using System;

namespace Rsdn.SmartApp.Extensibility
{
	[AttributeUsage(AttributeTargets.Class)]
	public class SimpleExtensionAttribute : Attribute
	{
		public bool Prop { get; set; }
	}
}