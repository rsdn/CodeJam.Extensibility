using CodeJam.Extensibility.Registration;

using Rsdn.SmartApp;
using Rsdn.SmartApp.Extensibility.Registration;

using ElementInfo = Rsdn.SmartApp.Extensibility.Registration.ElementInfo;

namespace CodeJam.Extensibility
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