using System.IO;
using System.Xml;
using Elapsus.Usms.Gateway.Factory;
using Elapsus.Usms.Gateway.Interface.Engine;
using Elapsus.Usms.Gateway.Interface.Exceptions;
using NUnit.Framework;
using TypeMock;

namespace Test.Usms.Gateway.Logging
{
	/// <summary>
	/// Verify that logging works.
	/// Logging is implemented using plug-ins.
	/// A plug-in can be inserted in the logging pipeline and
	/// called for each registerable event. This means
	/// that different logging capabilities can 
	/// be enabled, for example a database logging
	/// and text logging
	/// </summary>
	[TestFixture]
	public class Test_LogProvider
	{
		[SetUp]
		public void SetUp()
		{
			MockManager.Init();
		}

		/// <summary>
		/// Verify that if a provider is not found a ProviderNotFoundException is thrown
		/// </summary>
		[Test][VerifyMocks]
		[ExpectedException(typeof(ProviderNotFoundException))]
		public void ProviderNotFound()
		{
			ISmsc smsc;
			XmlReader xmlReader;

			// Mock a configuration file with no providers specified
			xmlReader = XmlReader.Create(new StringReader("" +
			                                              "<?xml version='1.0' encoding='utf-8' ?>\n" +
			                                              "<usmsgw>\n" +
			                                              "<logProviders>\n" +
			                                              "</logProviders>\n" +
			                                              "</usmsgw>"
			                             	));

			using (RecordExpectations rec = RecorderManager.StartRecording())
			{
				XmlReader.Create("", null);
				rec.Return(xmlReader).WhenArgumentsMatch(Check.IsEqualIgnoreCase("LogProviders.xml"), Check.IsAny());
			}

			smsc = SmsFactory.GetSmsc();
			smsc.LogProviders.Lookup("UnknownProvider");
		}
	}
}