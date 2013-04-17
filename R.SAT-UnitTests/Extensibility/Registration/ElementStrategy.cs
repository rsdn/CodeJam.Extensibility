namespace Rsdn.SmartApp.Extensibility.Registration
{
	internal class ElementStrategy : RegElementsStrategy<ElementInfo, ElementAttribute>
	{
		public ElementStrategy(IServicePublisher publisher)
			: base(publisher)
		{
		}

		/// <summary>
		/// Создать элемент.
		/// </summary>
		public override ElementInfo CreateElement(ExtensionAttachmentContext context, ElementAttribute attr)
		{
			return new ElementInfo(context.Type);
		}
	}
}