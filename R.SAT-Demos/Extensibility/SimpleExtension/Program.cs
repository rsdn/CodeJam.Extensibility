using System;

namespace Rsdn.SmartApp.Demos
{
	internal static class Program
	{
		private static void Main()
		{
			var extMgr = new ExtensionManager(null);
			var types = typeof (Program).Assembly.GetTypes();
			extMgr.Scan(new SimpleExtensionStrategy(Console.Out), types);

			Console.Read();
		}
	}
}
