using CodeJam.Extensibility.Registration;

using Rsdn.SmartApp;
using Rsdn.SmartApp.Demos;

namespace CodeJam.Extensibility.Demos
{
	internal class RegKeyedFruitStrategy : RegKeyedElementsStrategy<string, FruitInfo, FruitAttribute>
	{
		public RegKeyedFruitStrategy(IServicePublisher publisher) : base(publisher)
		{}

		/// <summary>
		/// Создать элемент.
		/// </summary>
		public override FruitInfo CreateElement(ExtensionAttachmentContext context, FruitAttribute attr)
		{
			return new FruitInfo(attr.Name, context.Type);
		}
	}
}
