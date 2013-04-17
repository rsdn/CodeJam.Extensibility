namespace Rsdn.SmartApp.Extensibility.Registration
{
	internal class NamedElementStrategy :
		RegKeyedElementsStrategy<string, TestNamedElementInfo, TestNamedElementAttribute>
	{
		public NamedElementStrategy(IServicePublisher publisher)
			: base(publisher)
		{
		}

		/// <summary>
		/// Создать элемент.
		/// </summary>
		public override TestNamedElementInfo CreateElement(
			ExtensionAttachmentContext context,
			TestNamedElementAttribute attr)
		{
			return new TestNamedElementInfo(attr.Name, context.Type);
		}
	}
}