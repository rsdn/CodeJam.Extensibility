using System;
using System.Collections.Generic;
using System.Linq;

namespace Rsdn.SmartApp
{
	using IRegSvc = IRegElementsService<ActivePartInfo>;

	/// <summary>
	/// Стандартная реализация <see cref="IActivePartManager"/>
	/// </summary>
	public class ActivePartManager : ServiceConsumer, IActivePartManager, IDisposable
	{
		private readonly Dictionary<string, IActivePart> _parts = new Dictionary<string, IActivePart>();
		private ActivePartManagerState _state = ActivePartManagerState.Passivated;

#pragma warning disable 0649
		[ExpectService(Required = false)]
		private IRegSvc _regSvc;
#pragma warning restore 0649

		/// <include file='../CommonXmlDocs.xml' path='common-docs/def-ctor/*'/>
		public ActivePartManager(IServiceProvider provider) : base(provider)
		{}

		#region IActivePartManager Members
		/// <summary>
		/// Current state.
		/// </summary>
		public ActivePartManagerState State
		{
			get { return _state; }
		}

		/// <summary>
		/// Получить экземпляр active part.
		/// </summary>
		public object GetPartInstance(Type type)
		{
			if (type == null)
				throw new ArgumentNullException("type");
			if (State != ActivePartManagerState.Activated)
				throw new InvalidOperationException("This operation allowed only in 'Activated' state");

			IActivePart part;
			if (!_parts.TryGetValue(type.AssemblyQualifiedName, out part))
				throw new ArgumentException("Part for type '{0}' is not registered".FormatStr(type));
			return part;
		}

		/// <summary>
		/// Activate all registered parts.
		/// </summary>
		public void Activate()
		{
			_state = ActivePartManagerState.Activating;
			try
			{
				foreach (var info in _regSvc.GetRegisteredElements())
				{
					IActivePart part;
					if (!_parts.TryGetValue(info.TypeName, out part))
					{
						part = (IActivePart)Type.GetType(info.TypeName, true).CreateInstance(ServiceProvider);
						_parts.Add(info.TypeName, part);
					}
					part.Activate();
				}
			}
			catch
			{
				_state = ActivePartManagerState.Invalid;
				throw;
			}
			_state = ActivePartManagerState.Activated;
		}

		/// <summary>
		/// Passivate all active parts.
		/// </summary>
		public void Passivate()
		{
			_state = ActivePartManagerState.Passivating;
			try
			{
				foreach (var part in _parts.Values)
					part.Passivate();
			}
			catch
			{
				_state = ActivePartManagerState.Invalid;
				throw;
			}
			_state = ActivePartManagerState.Passivated;
		}
		#endregion

		#region Implementation of IDisposable
		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			if (State != ActivePartManagerState.Passivated)
				Passivate();
			_parts
				.Values
				.OfType<IDisposable>()
				.DisposeAll();
		}
		#endregion
	}
}