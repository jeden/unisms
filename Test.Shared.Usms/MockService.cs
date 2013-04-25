using System.IO;
using System.Xml;
using TypeMock;

namespace Test.Shared.Usms.Gateway
{
	public static class MockService
	{
		public static void MockQueueProvider()
		{
			XmlReader xmlReader;

			// Mock a configuration file with no providers specified
			xmlReader = XmlReader.Create(new StringReader("" +
																										"<?xml version='1.0' encoding='utf-8' ?>\n" +
																										"<usmsgw>\n" +
																										"<queueProviders>\n" +
																										"<queueProvider enabled='true' default='true'>\n" +
																										"<name>SqlServerQueue</name>\n" +
																										"<assembly>Elapsus.Usms.Queuing.SqlServer</assembly>\n" +
																										"<class>Elapsus.Usms.Queuing.SqlServer.Queue</class>\n" +
																										"</queueProvider>\n" +
																										"</queueProviders>\n" +
																										"</usmsgw>"
																		));

			using (RecordExpectations rec = RecorderManager.StartRecording())
			{
				XmlReader.Create("", null);
				rec.Return(xmlReader).WhenArgumentsMatch(Check.IsEqualIgnoreCase("QueueProviders.xml"), Check.IsAny());
			}
		}

		public static void MockTextLogProvider()
		{
			XmlReader xmlReader;

			// Mock a configuration file with no providers specified
			xmlReader = XmlReader.Create(new StringReader("" +
																										"<?xml version='1.0' encoding='utf-8' ?>\n" +
																										"<usmsgw>\n" +
																										"<logProviders>\n" +
																										"<logProvider enabled='true'>\n" +
																										"<name>TextLogger</name>\n" +
																										"<assembly>Elapsus.Usms.Logging.Text</assembly>\n" +
																										"<class>Elapsus.Usms.Logging.Text.TextLogger</class>\n" +
																										"<configuration>\n" +
																										@"<set key='fileName' value='C:\temp\sms.log' />\n" +
																										"</configuration>\n" +
																										"</logProvider>\n" +
																										"</logProviders>\n" +
																										"</usmsgw>"
																		));

			using (RecordExpectations rec = RecorderManager.StartRecording())
			{
				XmlReader.Create("", null);
				rec.Return(xmlReader).WhenArgumentsMatch(Check.IsEqualIgnoreCase("LogProviders.xml"), Check.IsAny());
			}
		}

		public static void MockSqlServerLogProvider()
		{
			XmlReader xmlReader;

			// Mock a configuration file with no providers specified
			xmlReader = XmlReader.Create(new StringReader("" +
																										"<?xml version='1.0' encoding='utf-8' ?>\n" +
																										"<usmsgw>\n" +
																										"<logProviders>\n" +
																										"<logProvider enabled='true'>\n" +
																										"<name>SqlServerLogger</name>\n" +
																										"<assembly>Elapsus.Usms.Logging.SqlServer</assembly>\n" +
																										"<class>Elapsus.Usms.Logging.SqlServer.SqlServerLogProvider</class>\n" +
																										"<configuration>\n" +
																										"</configuration>\n" +
																										"</logProvider>\n" +
																										"</logProviders>\n" +
																										"</usmsgw>"
																		));

			using (RecordExpectations rec = RecorderManager.StartRecording())
			{
				XmlReader.Create("", null);
				rec.Return(xmlReader).WhenArgumentsMatch(Check.IsEqualIgnoreCase("LogProviders.xml"), Check.IsAny());
			}
		}

		public static void MockFakeSmsProvider()
		{
			XmlReader xmlReader;

			// Mock a configuration file with no providers specified
			xmlReader = XmlReader.Create(new StringReader("" +
																										"<?xml version='1.0' encoding='utf-8' ?>\n" +
																										"<usmsgw>\n" +
																										"<smsProviders>\n" +
																										"<smsProvider default='true'>\n" +
																										"<name>Fake</name>\n" +
																										"<assembly>Elapsus.Usms.Gateway.Providers.Fake</assembly>\n" +
																										"<class>Elapsus.Usms.Gateway.Providers.Fake.FakeProvider</class>\n" +
																										"<features>\n" +
																										"<feature name='simpleSms' value='yes' />\n" +
																										"<feature name='testMode' value='yes' />\n" +
																										"</features>\n" +
																										"<configuration>\n" +
																										"<set key='testMode' value='yes' />\n" +
																										"</configuration>\n" +
																										"</smsProvider>\n" +
																										"</smsProviders>\n" +
																										"</usmsgw>"
																		));

			using (RecordExpectations rec = RecorderManager.StartRecording())
			{
				XmlReader.Create("", null);
				rec.Return(xmlReader).WhenArgumentsMatch(Check.IsEqualIgnoreCase("SmsProviders.xml"), Check.IsAny());
			}
		}
	}
}
