using System;

using JetBrains.Annotations;

namespace CodeJam.Extensibility.Instancing
{
	/// <summary>
	/// Описатель дополнительных параметров конструктора.
	/// </summary>
	public class InstancingCustomParam
	{
		/// <summary>
		/// Инициализирует экземпляр опциональным параметром.
		/// </summary>
		public InstancingCustomParam([NotNull] string name, object value) : this(name, value, true)
		{}

		/// <summary>
		/// Инициализирует экземпляр.
		/// </summary>
		public InstancingCustomParam([NotNull] string name, object value, bool optional)
		{
			if (name == null)
				throw new ArgumentNullException(nameof(name));
			Name = name;
			Value = value;
			Optional = optional;
		}

		/// <summary>
		/// Имя параметра.
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// Значение параметра.
		/// </summary>
		public object Value { get; }

		/// <summary>
		/// Опциональный.
		/// </summary>
		public bool Optional { get; }
	}
}