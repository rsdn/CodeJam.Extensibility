using System;

using CodeJam.Services;

namespace CodeJam.Extensibility.Demos
{
	internal class Program
	{
		private static void Main()
		{
			var rootMgr = new ServiceContainer();

			// Publish service
			rootMgr.Publish<IService>(new ServiceImpl("Root service"));
			Print(rootMgr);

			// Cascade managers
			var cascadedMgr = new ServiceContainer(rootMgr);
			Print(cascadedMgr);

			// Override service implementation
			using (cascadedMgr.Publish<IService>(new ServiceImpl("Cascaded service")))
				Print(cascadedMgr);

			Print(cascadedMgr);

			// Automatic service publishing
			var extMgr = new ExtensionManager(null);
			extMgr.Scan(
				ServicesHelper.CreateServiceStrategy(cascadedMgr),
				typeof(Program).Assembly.GetTypes());
			Print(cascadedMgr);

			Console.Read();
		}

		private static void Print(IServiceProvider provider)
		{
			provider
				.GetRequiredService<IService>()
				.PrintName(Console.Out);
		}
	}
}
