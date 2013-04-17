namespace Rsdn.SmartApp.Services
{
	public class ServiceUser
	{
#pragma warning disable 0649
		[ExpectService]
		private ISampleService _svc1;

		[ExpectService]
		internal ISampleService _svc2;

		[ExpectService]
		public ISampleService _svc3;
#pragma warning restore 0649

		public ISampleService Svc1
		{
			get { return _svc1; }
		}

		public ISampleService Svc2
		{
			get { return _svc2; }
		}

		public ISampleService Svc3
		{
			get { return _svc3; }
		}
	}
}