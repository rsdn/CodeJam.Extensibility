using System;

namespace CodeJam.Extensibility
{
	public class TestElementInfo : ElementInfo
	{
		public TestElementInfo(Type elementType) : base(elementType)
		{
			ElementType = elementType;
		}

		public Type ElementType { get; }
	}
}