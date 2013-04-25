using System;
using System.IO;
using System.Web;
using System.Web.Hosting;

namespace Test.Shared.Usms.Gateway.Web
{
	[Serializable]
	public sealed class FakeHost : MarshalByRefObject
	{
		public string CreateRequest(string page, string query)
		{
			StringWriter writer;
			SimpleWorkerRequest worker;

			writer = new StringWriter();
			worker = new SimpleWorkerRequest(page, query, writer);
			HttpRuntime.ProcessRequest(worker);
			writer.Flush();

			return writer.GetStringBuilder().ToString();
		}

		public static FakeHost CreateHost(string virtualPath, string physicalDir)
		{
			return (FakeHost) ApplicationHost.CreateApplicationHost(typeof (FakeHost), virtualPath, physicalDir);
		}
	}
}
