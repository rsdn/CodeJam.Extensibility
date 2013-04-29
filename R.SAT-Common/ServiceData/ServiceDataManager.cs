using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;

using JetBrains.Annotations;

using Rsdn.SmartApp;

//[assembly: InternalsVisibleTo(ServiceDataManager.ImplementationTypesAssemblyName)]

namespace Rsdn.SmartApp
{
	using IValsDic = IDictionary<string, object> ;
	using ValsDic = Dictionary<string, object>;

	/// <summary>
	/// Стандартная реализация <see cref="IServiceDataManager"/>
	/// </summary>
	public class ServiceDataManager : IServiceDataManager
	{
		/// <summary>
		/// Постфикс имени файла с данными.
		/// </summary>
		public const string FileNamePostfix = ".ServiceData";

		internal const string ImplementationTypesAssemblyName = "@@ServiceDataManagerDynamicAssembly";

		private readonly string _dataFolder;

		private static readonly ElementsCache<Type, Type> _implTypes =
			new ElementsCache<Type, Type>(CreateImplType);

		private static readonly ElementsCache<Type, IValsDic> _defValues =
			new ElementsCache<Type, IValsDic>(CreateDefValues);

		private static AssemblyBuilder _dynAsm;
		private static ModuleBuilder _dynModule;

		private static readonly MethodInfo _propChangedMethod =
			typeof (IChangeNotificationConsumer).GetMethod("PropertyChanged");

		private static readonly MethodInfo _checkVersionMethod =
			typeof(IChangeNotificationConsumer).GetMethod("CheckVersion");

		private static readonly PropertyInfo _valsItemProperty =
			typeof (IValsDic).GetProperty("Item");

		private static readonly MethodInfo _getTypeMethod =
			typeof (Type).GetMethod("GetTypeFromHandle", BindingFlags.Public | BindingFlags.Static);

		private static readonly PropertyInfo _valuesProperty =
			typeof (IDataInstance).GetProperty("Values");

		private readonly ElementsCache<Type, object> _instances;
		private ChangeNotificationConsumer _notificationConsumer;
		private readonly BinaryFormatter _formatter = new BinaryFormatter();
		private int _cacheVersion;

		[MethodImpl(MethodImplOptions.Synchronized)]
		private static Type CreateImplType(Type ifaceType)
		{
			if (_dynAsm == null)
			{
				_dynAsm = AppDomain.CurrentDomain.DefineDynamicAssembly(
					new AssemblyName(ImplementationTypesAssemblyName),
					AssemblyBuilderAccess.Run);
				_dynModule = _dynAsm.DefineDynamicModule("Main");
			}
			var type = _dynModule.DefineType(
				"Impl." + ifaceType.FullName,
				TypeAttributes.Serializable);
			type.AddInterfaceImplementation(ifaceType);
			type.AddInterfaceImplementation(typeof (IDataInstance));

			// Constructor
			var ctor = type.DefineConstructor(
				MethodAttributes.Public,
				CallingConventions.Standard,
				new[] {typeof (IChangeNotificationConsumer), typeof (IValsDic)});
			var notConsField = type.DefineField(
				"_notificationConsumer",
				typeof (IChangeNotificationConsumer),
				FieldAttributes.Private);
			var valsField = type.DefineField(
				"_values",
				typeof (IValsDic),
				FieldAttributes.Private);
			var ctorGen = ctor.GetILGenerator();
			ctorGen.Emit(OpCodes.Ldarg_0); // this
			ctorGen.Emit(OpCodes.Ldarg_1); // consumer
			ctorGen.Emit(OpCodes.Stfld, notConsField);
			ctorGen.Emit(OpCodes.Ldarg_0); // this
			ctorGen.Emit(OpCodes.Ldarg_2); // values
			ctorGen.Emit(OpCodes.Stfld, valsField);
			ctorGen.Emit(OpCodes.Ret);

			// IDataInstance.Values implementation
			var valsGetMethod = type.DefineMethod(
				"get_Values",
				MethodAttributes.Private | MethodAttributes.Virtual,
				typeof (IValsDic),
				new Type[0]);
			type.DefineMethodOverride(valsGetMethod, _valuesProperty.GetGetMethod());
			var valsGetGen = valsGetMethod.GetILGenerator();
			valsGetGen.Emit(OpCodes.Ldarg_0); // this
			valsGetGen.Emit(OpCodes.Ldfld, valsField);
			valsGetGen.Emit(OpCodes.Ret);

			// user properties
			foreach (var propertyInfo in ifaceType.GetProperties())
			{
				var getMethod = type.DefineMethod(
					"get_" + propertyInfo.Name,
					MethodAttributes.Private | MethodAttributes.SpecialName | MethodAttributes.Virtual,
					propertyInfo.PropertyType,
					new Type[0]);
				type.DefineMethodOverride(getMethod, propertyInfo.GetGetMethod());
				var getGen = getMethod.GetILGenerator();
				getGen.Emit(OpCodes.Ldarg_0); // this
				getGen.Emit(OpCodes.Ldfld, notConsField);
				getGen.Emit(OpCodes.Callvirt, _checkVersionMethod);
				getGen.Emit(OpCodes.Ldarg_0); // this
				getGen.Emit(OpCodes.Ldfld, valsField);
				getGen.Emit(OpCodes.Ldstr, propertyInfo.Name);
				getGen.Emit(OpCodes.Callvirt, _valsItemProperty.GetGetMethod());
				getGen.Emit(
					propertyInfo.PropertyType.IsValueType ? OpCodes.Unbox_Any : OpCodes.Castclass,
					propertyInfo.PropertyType);
				getGen.Emit(OpCodes.Ret);

				var setMethod = type.DefineMethod(
					"set_" + propertyInfo.Name,
					MethodAttributes.Private | MethodAttributes.SpecialName | MethodAttributes.Virtual,
					typeof (void),
					new[] {propertyInfo.PropertyType});
				type.DefineMethodOverride(setMethod, propertyInfo.GetSetMethod());
				var setGen = setMethod.GetILGenerator();
				setGen.Emit(OpCodes.Ldarg_0); // this
				setGen.Emit(OpCodes.Ldfld, valsField);
				setGen.Emit(OpCodes.Ldstr, propertyInfo.Name);
				setGen.Emit(OpCodes.Ldarg_1);
				if (propertyInfo.PropertyType.IsValueType)
					setGen.Emit(OpCodes.Box, propertyInfo.PropertyType);
				setGen.Emit(OpCodes.Callvirt, _valsItemProperty.GetSetMethod());
				setGen.Emit(OpCodes.Ldarg_0);
				setGen.Emit(OpCodes.Ldfld, notConsField);
				setGen.Emit(OpCodes.Ldtoken, ifaceType); // ifaceType parameter
				setGen.Emit(OpCodes.Call, _getTypeMethod);
				setGen.Emit(OpCodes.Ldarg_0); // instance parameter - this
				setGen.Emit(OpCodes.Ldstr, propertyInfo.Name); // propertyName parameter
				setGen.Emit(OpCodes.Callvirt, _propChangedMethod);
				setGen.Emit(OpCodes.Ret);

				var prop = type.DefineProperty(
					propertyInfo.Name,
					PropertyAttributes.None,
					propertyInfo.PropertyType,
					new Type[0]);
				prop.SetGetMethod(getMethod);
				prop.SetSetMethod(setMethod);
			}

			return type.CreateType();
		}

		private static IValsDic CreateDefValues(Type type)
		{
			return
				type
					.GetProperties()
					.ToDictionary(
						propertyInfo => propertyInfo.Name,
						propertyInfo =>
							propertyInfo.PropertyType.IsValueType
								? Activator.CreateInstance(propertyInfo.PropertyType)
								: null);
		}

		/// <summary>
		/// Инициализирует экземпляр.
		/// </summary>
		public ServiceDataManager(string dataFolder)
		{
			if (dataFolder == null)
				throw new ArgumentNullException("dataFolder");

			if (!Directory.Exists(dataFolder))
				Directory.CreateDirectory(dataFolder);
			_dataFolder = dataFolder;
			_notificationConsumer = new ChangeNotificationConsumer(this);
			_instances = new ElementsCache<Type, object>(CreateNewInstance);
		}

		/// <summary>
		/// Возвращает имя файла с данными.
		/// </summary>
		public string GetDataFileName(Type type)
		{
			return Path.Combine(_dataFolder, type.FullName + FileNamePostfix);
		}

		/// <summary>
		/// Вызывает событие <see cref="PropertyChanged"/>
		/// </summary>
		protected void OnPropertyChanged(Type ifaceType, object instance, string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, ifaceType, instance, propertyName);
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		private object CreateNewInstance(Type type)
		{
			return Activator.CreateInstance(
				_implTypes.Get(type),
				_notificationConsumer,
				LoadData(type));
		}

		private void PropertyChangedFired(Type ifaceType, object instance, string propertyName)
		{
			SaveData(ifaceType, instance);
			OnPropertyChanged(ifaceType, instance, propertyName);
		}

		private ValsDic LoadData(Type type)
		{
			var fileName = GetDataFileName(type);
			if (!File.Exists(fileName))
				return new ValsDic(_defValues.Get(type));
			try
			{
				using (var fs = new FileStream(fileName, FileMode.Open))
					return (ValsDic)_formatter.Deserialize(fs);
			}
			catch
			{
				// Load failed. return default.
				return new ValsDic(_defValues.Get(type));
			}
		}

		private void SaveData(Type type, object data)
		{
			var dataInst = (IDataInstance)data;
			using (var fs = new FileStream(GetDataFileName(type), FileMode.Create))
				_formatter.Serialize(fs, dataInst.Values);
		}

		#region IServiceDataManager Members
		/// <summary>
		/// Вызывается при изменении свойства в данных.
		/// </summary>
		public event PropertyChangedHandler PropertyChanged;

		/// <summary>
		/// Получить экземпляр сервисных данных.
		/// </summary>
		public T GetServiceDataInstance<T>() where T : class
		{
			var type = typeof (T);
			if (!type.IsInterface)
				throw new ArgumentException("Type must be an interface");
			return (T)_instances.Get(type);
		}

		/// <summary>
		/// Сбросить кеш экземпляров.
		/// </summary>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public void ResetCache()
		{
			_instances.Reset();
			_cacheVersion++;
			_notificationConsumer = new ChangeNotificationConsumer(this);
		}
		#endregion

		#region IChangeNotificationConsumer interface
		/// <include file='../CommonXmlDocs.xml' path='common-docs/service-code/*'/>
		[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
		internal interface IChangeNotificationConsumer
		{
			/// <include file='../CommonXmlDocs.xml' path='common-docs/service-code/*'/>
			[UsedImplicitly]
			void PropertyChanged(Type ifaceType, object instance, string propertyName);

			/// <include file='../CommonXmlDocs.xml' path='common-docs/service-code/*'/>
			[UsedImplicitly]
			void CheckVersion();
		}
		#endregion

		#region ChangeNotificationConsumer class
		private class ChangeNotificationConsumer : IChangeNotificationConsumer
		{
			private readonly ServiceDataManager _manager;
			private readonly int _cacheVersion;

			public ChangeNotificationConsumer(ServiceDataManager manager)
			{
				_manager = manager;
				_cacheVersion = _manager._cacheVersion;
			}

			#region IChangeNotificationConsumer Members
			/// <summary>
			/// Служебный интерфейс.
			/// </summary>
			public void PropertyChanged(Type ifaceType, object instance, string propertyName)
			{
				CheckVersion();
				_manager.PropertyChangedFired(ifaceType, instance, propertyName);
			}

			/// <include file='../CommonXmlDocs.xml' path='common-docs/service-code/*'/>
			public void CheckVersion()
			{
				if (_manager._cacheVersion != _cacheVersion)
					throw new InvalidOperationException("Manager cache vas resetted. Query data instance again.");
			}
			#endregion
		}
		#endregion

		#region IDataInstance interface
		/// <include file='../CommonXmlDocs.xml' path='common-docs/service-code/*'/>
		[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
		internal interface IDataInstance
		{
			/// <include file='../CommonXmlDocs.xml' path='common-docs/service-code/*'/>
			IValsDic Values { get; }
		}
		#endregion
	}
}
