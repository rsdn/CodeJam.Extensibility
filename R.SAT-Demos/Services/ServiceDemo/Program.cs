using System;

namespace Rsdn.SmartApp.Demos
{
	internal class Program
	{
		private static void Main()
		{
			var rootMgr = new ServiceManager();

			// Publish service
			rootMgr.Publish<IService>(new ServiceImpl("Root service"));
			Print(rootMgr);

			// Cascade managers
			var cascadedMgr = new ServiceManager(rootMgr);
			Print(cascadedMgr);

			// Override service implementation
			var cookie = cascadedMgr.Publish<IService>(
				new ServiceImpl("Cascaded service"));
			Print(cascadedMgr);

			// Remove service implementation
			cascadedMgr.Unpublish(cookie);
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
