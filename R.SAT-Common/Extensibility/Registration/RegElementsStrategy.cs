using System;

namespace Rsdn.SmartApp
{
	/// <summary>
	/// Вспомогательный класс для реализации атрибутов регистрации.
	/// </summary>
	public abstract class RegElementsStrategy<TInfo, TAttribute> : AttachmentStrategyBase<TAttribute>
		where TAttribute : Attribute where TInfo : class
	{
		private readonly IServicePublisher _publisher;

		/// <summary>
		/// Инициализирует экземпляр.
		/// </summary>
		protected RegElementsStrategy(IServicePublisher publisher)
		{
			if (publisher == null)
				throw new ArgumentNullException("publisher");
			_publisher = publisher;
		}

		/// <summary>
		/// Публикатор сервисов.
		/// </summary>
		protected IServicePublisher Publisher
		{
			get { return _publisher; }
		}

		/// <summary>
		/// Подключает расширение.
		/// </summary>
		protected override void Attach(ExtensionAttachmentContext context, TAttribute attribute)
		{
			var regSvc = GetService(context, attribute);
			regSvc.Register(CreateElement(context, attribute));
		}

		/// <summary>
		/// Создать элемент.
		/// </summary>
		public abstract TInfo CreateElement(ExtensionAttachmentContext context, TAttribute attr);

		/// <summary>
		/// Создать реализацию сервиса.
		/// </summary>
		protected virtual IRegElementsService<TInfo> CreateService(ExtensionAttachmentContext context, TAttribute attr)
		{
			return new RegElementsService<TInfo>();
		}

		private IRegElementsService<TInfo> GetService(ExtensionAttachmentContext context, TAttribute attr)
		{
			var regSvc = Publisher.GetService<IRegElementsService<TInfo>>();
			if (regSvc == null)
			{
				regSvc = CreateService(context, attr);
				Publisher.Publish(regSvc);
			}
			return regSvc;
		}
	}
}