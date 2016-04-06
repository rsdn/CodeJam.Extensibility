using CodeJam.Extensibility.Registration;
using CodeJam.Services;

namespace CodeJam.Extensibility
{
	internal class ElementStrategy : RegElementsStrategy<TestElementInfo, ElementAttribute>
	{
		public ElementStrategy(IServicePublisher publisher)
			: base(publisher)
		{
		}

		/// <summary>
		/// Создать элемент.
		/// </summary>
		public override TestElementInfo CreateElement(ExtensionAttachmentContext context, ElementAttribute attr)
		{
			return new TestElementInfo(context.Type);
		}
	}
}