using System.IO;

namespace Rsdn.SmartApp.Demos
{
	/// <summary>
	/// ServiceImpl implementation.
	/// </summary>
	[Service(typeof (IService))]
	internal class ServiceImpl : IService
	{
		private readonly string _name;

		public ServiceImpl(string name)
		{
			_name = name;
		}

		public ServiceImpl() : this("Default service")
		{}

		public void PrintName(TextWriter writer)
		{
			writer.WriteLine(_name);
		}
	}
}
