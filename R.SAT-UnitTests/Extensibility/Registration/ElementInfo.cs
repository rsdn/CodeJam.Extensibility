using System;

namespace Rsdn.SmartApp.Extensibility.Registration
{
	public class ElementInfo
	{
		private readonly Type _elementType;

		public ElementInfo(Type elementType)
		{
			_elementType = elementType;
		}

		public Type ElementType
		{
			get { return _elementType; }
		}
	}
}