using System;
using NUnit.Framework;

namespace Test.Shared.Usms.Gateway.Web
{
	[TestFixture]
	public abstract class AspnetHostBase
	{
		private ApplicationHostEnvironment _hostEnv;
		private FakeHost _fakeHost;
		private readonly string _virtualPath;
		private readonly string _physicalDir;

		protected FakeHost Host { get { return _fakeHost; } }

		protected AspnetHostBase(string virtualPath, string physicalDir)
		{
			_virtualPath = virtualPath;
			_physicalDir = physicalDir;
		}

		[TestFixtureSetUp]
		public virtual void TestFixtureSetUp()
		{
			_hostEnv = new ApplicationHostEnvironment(Environment.CurrentDirectory);
			_fakeHost = FakeHost.CreateHost(_virtualPath, _physicalDir);
		}

		[TestFixtureTearDown]
		public virtual void TestFixtureTearDown()
		{
			_fakeHost = null;
			_hostEnv.Dispose();
			_hostEnv = null;
		}

		[SetUp]
		public void SetUp()
		{
			MockService.MockFakeSmsProvider();
			MockService.MockSqlServerLogProvider();
		}
	}
}
