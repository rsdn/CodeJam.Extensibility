using System;

using JetBrains.Annotations;

namespace Rsdn.SmartApp
{
	/// <summary>
	/// Базовая реализация <see cref="IActivePart"/>.
	/// </summary>
	public abstract class ActivePartBase : ServiceConsumer, IActivePart
	{
		private IDisposable _eventHandlersSubscription;
		private bool _isActivated;

		/// <summary>
		/// Инициализирует экземпляр.
		/// </summary>
		protected ActivePartBase([NotNull] IServiceProvider serviceProvider)
			: base(serviceProvider) { }

		#region Implementation of IActivePart

		/// <summary>
		/// Activates part.
		/// </summary>
		public void Activate()
		{
			if (_isActivated)
				return;

			_eventHandlersSubscription = EventBrokerHelper.SubscribeEventHandlers(this, ServiceProvider);

			ActivateCore();

			_isActivated = true;
		}

		/// <summary>
		/// Passivates part.
		/// </summary>
		public void Passivate()
		{
			if (!_isActivated)
				return;

			PassivateCore();

			_eventHandlersSubscription.Dispose();

			_isActivated = false;
		}

		#endregion

		#region Protected Members
		/// <summary>
		/// Core activation implementation.
		/// </summary>
		protected abstract void ActivateCore();

		/// <summary>
		/// Core passivation implementation.
		/// </summary>
		protected abstract void PassivateCore();

		#endregion
	}
}