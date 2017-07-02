using System;

using CodeJam.Extensibility.Demos.FruitModel;
using CodeJam.Extensibility.Registration;
using CodeJam.Services;

namespace CodeJam.Extensibility.Demos
{
	using FruitSvc = IRegElementsService<FruitInfo>;
	using KeyedFruitSvc = IRegKeyedElementsService<string, FruitInfo>;

	internal class Program
	{
		static void Main()
		{
			var extMgr = new ExtensionManager(null);
			var types = typeof(Program).Assembly.GetTypes();

			// Elements registration
			var regSvcMgr = new ServiceContainer();
			extMgr.Scan(new RegFruitStrategy(regSvcMgr), types);
			var fruitSvc = regSvcMgr.GetRequiredService<FruitSvc>();
			foreach (var fi in fruitSvc.GetRegisteredElements())
				Console.WriteLine("{0}({1})", fi.Name, fi.Type);

			// Keyed elements registration
			var keyedRegSvcMgr = new ServiceContainer();
			extMgr.Scan(new RegKeyedFruitStrategy(keyedRegSvcMgr), types);
			var keyedFruitSvc = keyedRegSvcMgr.GetRequiredService<KeyedFruitSvc>();
			Console.WriteLine("lemon type is '{0}'",
				keyedFruitSvc.GetElement("lemon").Type);

			Console.Read();
		}
	}
}
