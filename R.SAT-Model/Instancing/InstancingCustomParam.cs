using System;

using JetBrains.Annotations;

namespace Rsdn.SmartApp
{
	/// <summary>
	/// Описатель дополнительных параметров конструктора.
	/// </summary>
	public class InstancingCustomParam
	{
		private readonly string _name;
		private readonly object _value;
		private readonly bool _optional;

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
				throw new ArgumentNullException("name");
			_name = name;
			_value = value;
			_optional = optional;
		}

		/// <summary>
		/// Имя параметра.
		/// </summary>
		public string Name
		{
			get { return _name; }
		}

		/// <summary>
		/// Значение параметра.
		/// </summary>
		public object Value
		{
			get { return _value; }
		}

		/// <summary>
		/// Опциональный.
		/// </summary>
		public bool Optional
		{
			get { return _optional; }
		}
	}
}