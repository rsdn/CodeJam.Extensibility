using System;

using JetBrains.Annotations;

namespace Rsdn.SmartApp
{
	/// <summary>
	/// Атрибут, помечающий источник события.
	/// </summary>
	[AttributeUsage(AttributeTargets.Event | AttributeTargets.Property, AllowMultiple = true)]
	[MeansImplicitUse]
	public class EventSourceAttribute : Attribute
	{
		private readonly string _eventName;

		/// <summary>
		/// Initialize instance.
		/// </summary>
		public EventSourceAttribute(string eventName)
		{
			_eventName = eventName;
		}

		/// <summary>
		/// Event name.
		/// </summary>
		public string EventName
		{
			get { return _eventName; }
		}
	}
}