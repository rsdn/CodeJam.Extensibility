using CodeJam.Extensibility.Demos.FruitModel;
using CodeJam.Extensibility.Registration;

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
