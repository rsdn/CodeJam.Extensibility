using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

using CodeJam.Collections;
using CodeJam.Extensibility.Model;

namespace CodeJam.Extensibility
{
	partial class ServicesHelper
	{
		#region Nested type: AssignHelper
		/// <summary>
		/// 
		/// </summary>
		public class AssignHelper
		{
			private static readonly ILazyDictionary<Type, AssignDelegate> _assignMethods =
				LazyDictionary.Create<Type, AssignDelegate>(CreateAssignMethod, true);

			private static readonly MethodInfo _getRequiredServiceMethod =
				typeof (ServicesHelper).GetMethod("GetRequiredService", new[] {typeof (IServiceProvider)});

			private static readonly MethodInfo _getServiceMethod = typeof (ServicesHelper).GetMethod("GetService");

			private static AssignDelegate CreateAssignMethod(Type type)
			{
				var fields =
					GetAllFields(type)
						.Select(info =>
							new
							{
								Info = info,
								Attr = info.GetCustomAttribute<ExpectServiceAttribute>(false)
							})
						.Where(pair => pair.Attr != null)
						.Select(pair =>
							new
							{
								pair.Info,
								Method =
									(pair.Attr.Required ? _getRequiredServiceMethod : _getServiceMethod).MakeGenericMethod(
										pair.Info.FieldType)
							})
						.ToArray();
				if (fields.Length == 0)
					return null;

				var method = new DynamicMethod(
					"AssignMethod@" + type.FullName,
					null,
					new[] {typeof (IServiceProvider), typeof (object)},
					typeof (AssignHelper),
					true);

				var gen = method.GetILGenerator();

				gen.DeclareLocal(type);

				gen.Emit(OpCodes.Ldarg_1);
				gen.Emit(OpCodes.Castclass, type);
				gen.Emit(OpCodes.Stloc_0);

				foreach (var field in fields)
				{
					gen.Emit(OpCodes.Ldloc_0);
					// Get service
					gen.Emit(OpCodes.Ldarg_0);
					gen.Emit(OpCodes.Call, field.Method);
					// Set field
					gen.Emit(OpCodes.Stfld, field.Info);
				}

				gen.Emit(OpCodes.Ret);

				return (AssignDelegate) method.CreateDelegate(typeof (AssignDelegate));
			}

			/// <summary>
			/// 
			/// </summary>
			public static void Assign(object instance, IServiceProvider provider)
			{
				if (instance == null)
					throw new ArgumentNullException(nameof(instance));
				if (provider == null)
					throw new ArgumentNullException(nameof(provider));

				var method = _assignMethods[instance.GetType()];
				method?.Invoke(provider, instance);
			}

			private static IEnumerable<FieldInfo> GetAllFields(Type type)
			{
				if (type == typeof(object))
					yield break;
				foreach (var info in GetAllFields(type.BaseType))
					yield return info;
				foreach (var info in type.GetFields(
						BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
					yield return info;
			}

			#region Nested type: AssignDelegate
			private delegate void AssignDelegate(IServiceProvider provider, object instance);
			#endregion
		}
		#endregion
	}
}