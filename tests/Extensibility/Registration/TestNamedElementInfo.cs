using System;

namespace Rsdn.SmartApp.Extensibility.Registration
{
	internal class TestNamedElementInfo : ElementInfo, IKeyedElementInfo<string>
	{
		public TestNamedElementInfo(string key, Type type) : base(type)
		{
			Key = key;
		}

		#region IKeyedElementInfo<string> Members
		public string Key { get; private set; }
		#endregion
	}
}