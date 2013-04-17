using System;

using JetBrains.Annotations;

namespace Rsdn.SmartApp
{
	/// <summary>
	/// Помечает поля, ожидающие сервиса.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	[MeansImplicitUse]
	public class ExpectServiceAttribute : Attribute
	{
		/// <summary>
		/// Инициализирует экземпляр.
		/// </summary>
		public ExpectServiceAttribute()
		{
			Required = true;
		}

		/// <summary>
		/// Признак обязательности наличия сервиса.
		/// </summary>
		public bool Required { get; set; }
	}
}