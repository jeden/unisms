using System;
using System.IO;
using System.Xml;
using Elapsus.Usms.Gateway.Factory;
using Elapsus.Usms.Gateway.Interface.Engine;
using Elapsus.Usms.Gateway.Interface.Exceptions;
using NUnit.Framework;
using TypeMock;

namespace Test.Usms.Gateway.Queuing
{
	[TestFixture]
	public class Test_QueueProvider
	{
		[SetUp]
		public void SetUp()
		{
			MockManager.Init();
		}

		/// <summary>
		/// Verify that if a provider is not found a ProviderNotFoundException is thrown
		/// </summary>
		[Test, VerifyMocks]
		[ExpectedException(typeof(ProviderNotFoundException))]
		public void ProviderNotFound()
		{
			ISmsc smsc;
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

			smsc = SmsFactory.GetSmsc();
			smsc.QueueProviders.Lookup("UnknownProvider");
		}

		/// <summary>
		/// If no default queue provider is found a <see cref="ProviderConfigurationException"/> 
		/// exception is thrown
		/// </summary>
		[Test, VerifyMocks]
		public void NoDefaultQueue()
		{
			XmlReader xmlReader;

			// Mock a configuration file with no providers specified
			xmlReader = XmlReader.Create(new StringReader("" +
			                                              "<?xml version='1.0' encoding='utf-8' ?>\n" +
			                                              "<usmsgw>\n" +
			                                              "<queueProviders>\n" +
			                                              "<queueProvider enabled='true' default='false'>\n" +
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

			try
			{
				SmsFactory.GetSmsc();
				Assert.Fail("Expected ProviderConfigurationException");
			}
			catch (ProviderConfigurationException ex)
			{
				if (ex.Reason != EnConfigurationReason.MissingDefaultProvider)
					Assert.Fail("Unexpected reason");

				Console.Out.WriteLine(ex.Message);
			}
		}

		/// <summary>
		/// If more than one default queue provider are specified
		/// a <see cref="ProviderConfigurationException"/> exception is thrown
		/// </summary>
		[Test, VerifyMocks]
		public void MultipleDefaultQueue()
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
			                                              "<queueProvider enabled='true' default='true'>\n" +
			                                              "<name>SqlServerQueue2</name>\n" +
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

			try
			{
				SmsFactory.GetSmsc();
				Assert.Fail("Expected ProviderConfigurationException");
			}
			catch (ProviderConfigurationException ex)
			{
				if (ex.Reason != EnConfigurationReason.MultipleDefaultProviders)
					Assert.Fail(string.Format("Unexpected reason: {0}", ex.Reason));

				Console.Out.WriteLine(ex.Message);
			}
		}
	}
}