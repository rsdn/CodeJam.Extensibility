using System;

namespace Rsdn.SmartApp.Demos
{
	public class FruitInfo : IKeyedElementInfo<string>
	{
		private readonly string _name;
		private readonly Type _type;

		public FruitInfo(string name, Type type)
		{
			_name = name;
			_type = type;
		}

		public string Name
		{
			get { return _name; }
		}

		public Type Type
		{
			get { return _type; }
		}

		string IKeyedElementInfo<string>.Key
		{
			get { return Name; }
		}
	}
}
