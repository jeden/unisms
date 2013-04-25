using System.IO;
using System.Xml;
using Elapsus.Usms.Gateway.Factory;
using Elapsus.Usms.Gateway.Interface.Engine;
using Elapsus.Usms.Gateway.Interface.Envelope;
using Elapsus.Usms.Gateway.Interface.Exceptions;
using NUnit.Framework;
using Test.Shared.Usms.Gateway;
using TypeMock;

namespace Test.Usms.Gateway.SmsProviders
{
	[TestFixture]
	public class Test_SmsProvider
	{
		/// <summary>
		/// Verify that if a provider is not found a ProviderNotFoundException is thrown
		/// </summary>
		[Test, ExpectedException(typeof(ProviderNotFoundException))]
		public void ProviderNotFound()
		{
			ISmsc smsc;

			smsc = SmsFactory.GetSmsc();
			smsc.SmsProviders.Lookup("UnknownProvider");
		}

		/// <summary>
		/// If no Sms Provider is found, an exception is thrown whenever 
		/// an sms send is attempted (with 1 recipient)
		/// </summary>
		[Test, VerifyMocks, ExpectedException(typeof(ProviderNotFoundException))]
		public void Test_ThrowExceptionIfNoProviderLoaded()
		{
			ISmsc smsc;
			ISms sms;
			XmlReader xmlReader;

			MockManager.Init();

			// Mock a configuration file with no providers specified
			xmlReader = XmlReader.Create(new StringReader("" +
			                                              "<?xml version='1.0' encoding='utf-8' ?>\n" +
			                                              "<usmsgw>\n" +
			                                              "<smsProviders>\n" +
			                                              "</smsProviders>\n" +
			                                              "</usmsgw>"
			                             	));

			using (RecordExpectations rec = RecorderManager.StartRecording())
			{
				XmlReader.Create("", null);
				rec.Return(xmlReader).WhenArgumentsMatch(Check.IsEqualIgnoreCase("SmsProviders.xml"), Check.IsAny());
			}

			smsc = SmsFactory.GetSmsc();
			sms = SmsFactory.NewSms(TestConstants.Default_ApplicationId);
			sms.AddRecipient(TestConstants.Default_Recipient);
			sms.Sender = TestConstants.Default_Sender;
			sms.Message = "TestMessage";

			smsc.SendImmediate(sms);
		}

		/// <summary>
		/// If no Sms Provider is found, an exception is thrown whenever 
		/// an sms send is attempted (with multiple recipients)
		/// </summary>
		[Test, VerifyMocks, ExpectedException(typeof(ProviderNotFoundException))]
		public void Test_ThrowExceptionIfNoProviderLoaded_MultipleRecipients()
		{
			ISmsc smsc;
			ISms sms;
			XmlReader xmlReader;

			MockManager.Init();

			// Mock a configuration file with no providers specified
			xmlReader = XmlReader.Create(new StringReader("" +
			                                              "<?xml version='1.0' encoding='utf-8' ?>\n" +
			                                              "<usmsgw>\n" +
			                                              "<smsProviders>\n" +
			                                              "</smsProviders>\n" +
			                                              "</usmsgw>"
			                             	));

			using (RecordExpectations rec = RecorderManager.StartRecording())
			{
				XmlReader.Create("", null);
				rec.Return(xmlReader).WhenArgumentsMatch(Check.IsEqualIgnoreCase("SmsProviders.xml"), Check.IsAny());
			}

			smsc = SmsFactory.GetSmsc();
			sms = SmsFactory.NewSms(TestConstants.Default_ApplicationId);
			sms.AddRecipient(TestConstants.Default_Recipient);
			sms.AddRecipient(TestConstants.Default_Recipient_2);
			sms.Sender = TestConstants.Default_Sender;
			sms.Message = "TestMessage";

			smsc.SendMultipleImmediate(sms);
		}

		/// <summary>
		/// If no default provider is specified in configuration
		/// and no provider is explicitly set in the sms
		/// a <see cref="ProviderNotFoundException"/> exception
		/// is thrown
		/// </summary>
		[Test, ExpectedException(typeof(ProviderNotFoundException))]
		public void Test_NoDefaultProvider_Single()
		{
			ISmsc smsc;
			ISms sms;
			XmlReader xmlReader;

			MockManager.Init();

			// Mock a configuration file with no default provider specified
			xmlReader = XmlReader.Create(new StringReader("" +
			                                              "<?xml version='1.0' encoding='utf-8' ?>\n" +
			                                              "<usmsgw>\n" +
			                                              "<smsProviders>\n" +
			                                              "<smsProvider>\n" +
			                                              "<name>aspsms</name>\n" +
			                                              "<assembly>Elapsus.Usms.Gateway.Providers.Aspsms</assembly>\n" +
			                                              "<class>Elapsus.Usms.Gateway.Providers.Aspsms.AspsmsProvider</class>\n" +
			                                              "<features>\n" +
			                                              "<feature name='simpleSms' value='yes' />\n" +
			                                              "<feature name='testMode' value='yes' />\n" +
			                                              "</features>\n" +
			                                              "<configuration>\n" +
			                                              "<set key='testMode' value='yes' />\n" +
			                                              "<set key='userKey' value='test' />\n" +
			                                              "<set key='password' value='pwdtest' />\n" +
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

			smsc = SmsFactory.GetSmsc();
			sms = SmsFactory.NewSms(TestConstants.Default_ApplicationId);
			sms.AddRecipient(TestConstants.Default_Recipient);
			sms.Sender = TestConstants.Default_Sender;
			sms.Message = "TestMessage";

			smsc.SendImmediate(sms);
		}

		[Test, ExpectedException(typeof(ProviderNotFoundException))]
		public void Test_NoDefaultProvider_Multiple()
		{
			ISmsc smsc;
			ISms sms;
			XmlReader xmlReader;

			MockManager.Init();

			// Mock a configuration file with no default provider specified
			xmlReader = XmlReader.Create(new StringReader("" +
			                                              "<?xml version='1.0' encoding='utf-8' ?>\n" +
			                                              "<usmsgw>\n" +
			                                              "<smsProviders>\n" +
			                                              "<smsProvider>\n" +
			                                              "<name>aspsms</name>\n" +
			                                              "<assembly>Elapsus.Usms.Gateway.Providers.Aspsms</assembly>\n" +
			                                              "<class>Elapsus.Usms.Gateway.Providers.Aspsms.AspsmsProvider</class>\n" +
			                                              "<features>\n" +
			                                              "<feature name='simpleSms' value='yes' />\n" +
			                                              "<feature name='testMode' value='yes' />\n" +
			                                              "</features>\n" +
			                                              "<configuration>\n" +
			                                              "<set key='testMode' value='yes' />\n" +
			                                              "<set key='userKey' value='test' />\n" +
			                                              "<set key='password' value='pwdtest' />\n" +
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

			smsc = SmsFactory.GetSmsc();
			sms = SmsFactory.NewSms(TestConstants.Default_ApplicationId);
			sms.AddRecipient(TestConstants.Default_Recipient);
			sms.AddRecipient(TestConstants.Default_Recipient_2);
			sms.Sender = TestConstants.Default_Sender;
			sms.Message = "TestMessage";

			smsc.SendMultipleImmediate(sms);
		}
	}
}