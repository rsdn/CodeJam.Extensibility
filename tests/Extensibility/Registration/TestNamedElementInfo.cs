using System;

using CodeJam.Extensibility.Registration;

namespace CodeJam.Extensibility
{
	internal class TestNamedElementInfo : TestElementInfo, IKeyedElementInfo<string>
	{
		public TestNamedElementInfo(string key, Type type) : base(type)
		{
			Key = key;
		}

		#region IKeyedElementInfo<string> Members
		public string Key { get; }
		#endregion
	}
}