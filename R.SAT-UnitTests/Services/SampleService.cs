namespace Rsdn.SmartApp.Services
{
	[Service(typeof (ISampleService))]
	internal class SampleService : ISampleService
	{
		private readonly string _name;

		[DefaultConstructor]
		public SampleService() : this("")
		{
		}

		public SampleService(string name)
		{
			_name = name;
		}

		#region ISampleService Members
		public string GetName()
		{
			return _name;
		}
		#endregion
	}
}