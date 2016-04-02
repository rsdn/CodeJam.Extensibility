using System;

using JetBrains.Annotations;

namespace CodeJam.Extensibility
{
	/// <summary>
	/// Simple element info implementation.
	/// </summary>
	public abstract class ElementInfo
	{
		/// <summary>
		/// Initialize instance.
		/// </summary>
		protected ElementInfo([NotNull] Type type, string description)
		{
			if (type == null)
				throw new ArgumentNullException(nameof(type));
			Type = type;
			Description = description;
		}

		/// <summary>
		/// Initialize instance.
		/// </summary>
		protected ElementInfo(Type type) : this(type, "")
		{}

		/// <summary>
		/// Element description.
		/// </summary>
		public string Description { get; }

		/// <summary>
		/// Element type.
		/// </summary>
		public Type Type { get; }
	}
}