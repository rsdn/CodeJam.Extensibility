using System;

using JetBrains.Annotations;

namespace Rsdn.SmartApp
{
	/// <summary>
	/// Simple element info implementation.
	/// </summary>
	public abstract class ElementInfo
	{
		private readonly Type _type;
		private readonly string _description;

		/// <summary>
		/// Initialize instance.
		/// </summary>
		protected ElementInfo([NotNull] Type type, string description)
		{
			if (type == null)
				throw new ArgumentNullException("type");
			_type = type;
			_description = description;
		}

		/// <summary>
		/// Initialize instance.
		/// </summary>
		protected ElementInfo(Type type) : this(type, "")
		{}

		/// <summary>
		/// Element description.
		/// </summary>
		public string Description
		{
			get { return _description; }
		}

		/// <summary>
		/// Element type.
		/// </summary>
		public Type Type
		{
			get { return _type; }
		}
	}
}