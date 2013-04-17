using System;
using System.Collections.Generic;

namespace Rsdn.SmartApp
{
	/// <summary>
	/// Базовая реализация <see cref="IRegElementsService{EI}"/>
	/// </summary>
	public class RegElementsService<TInfo> : IRegElementsService<TInfo> where TInfo : class
	{
		private readonly List<TInfo> _elements = new List<TInfo>();

		#region IRegElementsService<EI> Members
		/// <summary>
		/// Зарегистрировать элемент.
		/// </summary>
		public virtual void Register(TInfo elementInfo)
		{
			if (elementInfo == null)
				throw new ArgumentNullException("elementInfo");
			_elements.Add(elementInfo);
		}

		/// <summary>
		/// Получить список зарегистрированных элементов.
		/// </summary>
		public virtual TInfo[] GetRegisteredElements()
		{
			return _elements.ToArray();
		}
		#endregion
	}
}