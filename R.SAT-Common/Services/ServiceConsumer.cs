using System;

using JetBrains.Annotations;

namespace Rsdn.SmartApp
{
	/// <summary>
	/// Базовая реализация класса, использующего автоматическое получение сервисов.
	/// </summary>
	public abstract class ServiceConsumer
	{
		private readonly IServiceProvider _provider;

		/// <summary>
		/// Инициализирует экземпляр.
		/// </summary>
		protected ServiceConsumer([NotNull] IServiceProvider provider)
		{
			if (provider == null)
				throw new ArgumentNullException("provider");

			_provider = provider;

			this.AssignServices(_provider);
		}

		/// <summary>
		/// Instance of service provider.
		/// </summary>
		public IServiceProvider ServiceProvider
		{
			get { return _provider; }
		}
	}
}