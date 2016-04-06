using CodeJam.Extensibility.Demos.FruitModel;
using CodeJam.Extensibility.Registration;
using CodeJam.Services;

namespace CodeJam.Extensibility.Demos
{
	internal class RegFruitStrategy : RegElementsStrategy<FruitInfo, FruitAttribute>
	{
		public RegFruitStrategy(IServicePublisher publisher) : base(publisher)
		{}

		/// <summary>
		/// Создать элемент.
		/// </summary>
		public override FruitInfo CreateElement(
			ExtensionAttachmentContext context,
			FruitAttribute attr)
		{
			return new FruitInfo(attr.Name, context.Type);
		}
	}
}
