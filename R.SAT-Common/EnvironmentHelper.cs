using System;
using System.Linq;
using System.Reflection;

namespace Rsdn.SmartApp
{
	/// <summary>
	/// Вспомогательный класс для работы со средой.
	/// </summary>
	public static class EnvironmentHelper
	{
		private static readonly bool _isMono =
			Type.GetType("Mono.Runtime") != null;
		private static readonly ClrType _clrType =
			_isMono ? ClrType.Mono : ClrType.DotNet;

		/// <summary>
		/// Тип рантайма.
		/// </summary>
		public static ClrType ClrType
		{
			get { return _clrType;  }
		}

		/// <summary>
		/// Выполнить ветку в зависимости от типа фреймворка.
		/// </summary>
		public static void MatchClrType(
			Action dotNetBranch,
			Action monoBranch)
		{
			switch (ClrType)
			{
				case ClrType.DotNet:
					dotNetBranch();
					break;
				case ClrType.Mono:
					monoBranch();
					break;
				default:
					throw new ApplicationException(LocalizableStrings.EnvironmentHelper_UnsupportedPlatform);
			}
		}

		/// <summary>
		/// Возвращает директорию, в которой расположена сборка переданного типа.
		/// </summary>
		public static string GetAssemblyDir(this Type asmType)
		{
			var uri = new Uri(Assembly.GetEntryAssembly().CodeBase);
			// Склеиваем все сегменты, кроме первого и последнего.
			return
				uri
					.Segments
					.Skip(1)
					.Take(uri.Segments.Length - 2)
					//.Select(seg => HttpUtility.UrlDecode(seg))
					.Join();
		}

		/// <summary>
		/// Проверить среду выполнения на соответствие 3.5 версии фреймворка.
		/// </summary>
		public static void CheckEnvironment()
		{
			var t = Type.GetType("System.Action, System.Core, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
			if (t == null)
			{
				string msg;
				switch (ClrType)
				{
					case ClrType.DotNet:
						msg = LocalizableStrings.EnvironmentHelper_CheckEnvironmentNet;
						break;

					case ClrType.Mono:
						msg = LocalizableStrings.EnvironmentHelper_CheckEnvironmentMono;
						break;

					default:
						msg = LocalizableStrings.EnvironmentHelper_CheckEnvironmentCli;
						break;
				}
				throw new ApplicationException(msg);
			}
		}
	}
}


