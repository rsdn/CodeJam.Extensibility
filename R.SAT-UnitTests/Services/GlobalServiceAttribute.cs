using System;

namespace Rsdn.SmartApp
{
	[AttributeUsage(AttributeTargets.Class)]
	public class GlobalServiceAttribute : Attribute, IExtensionAttribute<ServicePublishingParam>
	{
		private readonly Type _serviceType;

		public GlobalServiceAttribute(Type serviceType)
		{
			_serviceType = serviceType;
		}

		public Type ServiceType
		{
			get { return _serviceType; }
		}

		/// <summary>
		/// Выполняет подключение расширения.
		/// </summary>
		public void Attach(IExtensionAttachmentContext<ServicePublishingParam> context)
		{
			ServicePublishingHelper.Attach<ServicePublishingParam>(context, _serviceType);
		}
	}
}
