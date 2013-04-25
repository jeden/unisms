using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using ASPSMS.NET;
using Elapsus.Usms.Gateway.Factory;
using Elapsus.Usms.Gateway.Interface.Engine;
using Elapsus.Usms.Gateway.Interface.Envelope;
using Elapsus.Usms.Gateway.Interface.Exceptions;
using Elapsus.Usms.Gateway.Interface.Providers.Gateway;
using NUnit.Framework;
using Test.Shared.Usms.Gateway;
using TypeMock;

namespace Test.Usms.Gateway.SmsProviders
{
	/// <summary>
	/// Test the Sms Handler class and inherited implementations
	/// </summary>
	/// <remarks>
	/// The SmsHandler class is a base class providing a base
	/// set of features which must be implemented for
	/// each SMS gateway.<br/>
	/// Tests in this unit use a fake implementation, instantiated
	/// by mocking the ISmscProvider interface
	/// </remarks>
	[TestFixture]
	public class Test_SmsProvider_Aspsms
	{
		private Mock<ASPSMSNET> _mockAspSmsNet;

		[SetUp]
		public void SetUp()
		{
			MockManager.Init();

			// Mock the call to the SendTextSMS() 
			_mockAspSmsNet = MockManager.Mock<ASPSMSNET>(Constructor.NotMocked);
			_mockAspSmsNet.ExpectAlways("SendTextSMS");
		}

		/// <summary>
		/// Load the Aspsms provider and verify it allows
		/// send of simple textual messages
		/// </summary>
		[Test][VerifyMocks]
		public void TestSimpleSmsCapability()
		{
			ISmsc smsc;
			ISmsProvider provider;
			Dictionary<string, string> settings;
			ObjectState mockProvider;
			ObjectState mockSettings;

			// This call, other than instantiating a Smsc object,
			// loads all registered Sms Providers
			smsc = SmsFactory.GetSmsc();

			// Lookup the Aspsms provider
			provider = smsc.SmsProviders.Lookup("Aspsms");

			// Verify the Aspsms provider is correctly loaded
			Assert.IsNotNull(provider, "Aspsms provider not found");
			Assert.AreEqual("aspsms", provider.Name.ToLower(), "Invalid sms provider name");
			Assert.IsTrue(provider.Capabilities.SimpleSms, "Aspsms provider not enabled to send simple SMS");
			Assert.IsTrue(provider.Capabilities.TestMode, "Aspsms provider not implementing a test mode");

			// Read the internal provider state in order to get access
			// to private members and display some information
			mockProvider = new ObjectState(provider);
			mockSettings = new ObjectState(provider.Configuration);

			settings = (Dictionary<string, string>) mockSettings.GetField("_settings");

			Console.Out.WriteLine("Name:     " + provider.Name);
			Console.Out.WriteLine("Assembly: " + mockProvider.GetField("_assemblyName"));
			Console.Out.WriteLine("Class:    " + mockProvider.GetField("_className"));
			Console.Out.WriteLine("UserKey:  " + settings["userKey"]);
			Console.Out.WriteLine("Password: " + settings["password"]);
		}

		/// <summary>
		/// Verify that the Aspsms provider is loaded 
		/// by sending a test message using the test mode
		/// </summary>
		[Test][VerifyMocks]
		public void LoadAspsmsProvider()
		{
			ISmsc smsc;
			ISms sms;
			ISmsProvider provider;
			ISmsStatus status;

			_mockAspSmsNet.ExpectGetAlways("ErrorCode", 1);

			smsc = SmsFactory.GetSmsc();
			provider = smsc.SmsProviders.Lookup("Aspsms");

			provider.TestMode = true;

			sms = SmsFactory.NewSms(TestConstants.Default_ApplicationId);
			sms.Sender = TestConstants.Default_Sender;
			sms.AddRecipient(TestConstants.Default_Recipient);
			sms.Message = "Simple sms";
			sms.Provider = provider;

			status = smsc.SendImmediate(sms);

			Assert.AreEqual(EnMtStatus.Accepted, status.Status);
			Assert.AreEqual(true, status.TestMode, "Sms not sent in test mode");
		}

		/// <summary>
		/// Verify that an <seealso cref="ProviderConfigurationException"/> exception
		/// is thrown when the userKey parameter is not provided in the 
		/// SmsProviders.xml file.
		/// </summary>
		[Test][VerifyMocks]
		public void ParameterNotFound_UserKey()
		{
			XmlReader xmlReader;

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

			try
			{
				SmsFactory.GetSmsc();

				Assert.Fail("Expected ProviderConfigurationException");
			}
			catch (ProviderConfigurationException)
			{
			}
		}

		/// <summary>
		/// Verify that an <seealso cref="ProviderConfigurationException"/> exception
		/// is thrown when the password parameter is not provided in the 
		/// SmsProviders.xml file.
		/// </summary>
		[Test]
		[VerifyMocks]
		public void ParameterNotFound_Password()
		{
			XmlReader xmlReader;

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

			try
			{
				SmsFactory.GetSmsc();

				Assert.Fail("Expected ProviderConfigurationException");
			}
			catch (ProviderConfigurationException)
			{
			}
		}

		/// <summary>
		/// Send a SMS using aspsms
		/// Actual send is mocked, so no real SMS is
		/// submitted for delivery
		/// </summary>
		[Test][VerifyMocks]
		public void Test_SendSms_Single()
		{
			ISmsc smsc;
			ISms sms;
			ISmsStatus status;
			ISmsProvider provider;

			// Force error code 1, which means no error
			_mockAspSmsNet.ExpectGet("ErrorCode", 1);

			smsc = SmsFactory.GetSmsc();
			sms = SmsFactory.NewSms(TestConstants.Default_ApplicationId);

			provider = smsc.SmsProviders.Lookup("aspsms");
			provider.TestMode = false;

			sms.Sender = TestConstants.Default_Sender;
			sms.AddRecipient(TestConstants.Default_Recipient);
			sms.Message = "Test SMS";
			sms.Provider = provider;

			status = smsc.SendImmediate(sms);
			Assert.AreEqual(EnMtStatus.Accepted, status.Status, "Invalid SMS status");
			Assert.AreEqual(false, status.TestMode, "Sms sent in test mode");
			Assert.IsTrue(string.IsNullOrEmpty(status.ErrorMessage), "No error message expected");

			MockManager.Verify();
		}

		/// <summary>
		/// Send a SMS using aspsms
		/// Actual send is mocked, so no real SMS is
		/// submitted for delivery
		/// </summary>
		[Test]
		[VerifyMocks]
		public void Test_SendSms_Multiple()
		{
			ISmsc smsc;
			ISms sms;
			ISmsStatusCollection status;
			ISmsProvider provider;

			// Force error code 1, which means no error
			_mockAspSmsNet.ExpectGetAlways("ErrorCode", 1);

			smsc = SmsFactory.GetSmsc();
			sms = SmsFactory.NewSms(TestConstants.Default_ApplicationId);

			provider = smsc.SmsProviders.Lookup("aspsms");
			provider.TestMode = false;

			sms.Sender = TestConstants.Default_Sender;
			sms.AddRecipient(TestConstants.Default_Recipient);
			sms.AddRecipient(TestConstants.Default_Recipient_2);
			sms.Message = "Test SMS";
			sms.Provider = provider;

			status = smsc.SendMultipleImmediate(sms);

			Assert.AreEqual(2, status.Count, "Unexpected number of sms statuses");

			Assert.AreEqual(EnMtStatus.Accepted, status[0].Status, "Invalid SMS status");
			Assert.AreEqual(false, status[0].TestMode, "Sms sent in test mode");
			Assert.IsTrue(string.IsNullOrEmpty(status[0].ErrorMessage), "No error message expected");
			Assert.AreEqual(TestConstants.Default_Recipient_Normalized, status[0].Recipient);

			Assert.AreEqual(EnMtStatus.Accepted, status[1].Status, "Invalid SMS status");
			Assert.AreEqual(false, status[1].TestMode, "Sms sent in test mode");
			Assert.IsTrue(string.IsNullOrEmpty(status[1].ErrorMessage), "No error message expected");
			Assert.AreEqual(TestConstants.Default_Recipient_2_Normalized, status[1].Recipient);
		}

		[Test][VerifyMocks]
		public void Test_SendSms_WithFailure()
		{
			ISmsc smsc;
			ISms sms;
			ISmsStatus status;
			ISmsProvider provider;

			_mockAspSmsNet.ExpectGetAlways("ErrorCode", -1);
			_mockAspSmsNet.ExpectGetAlways("ErrorDescription", "Service unavailable");

			smsc = SmsFactory.GetSmsc();
			sms = SmsFactory.NewSms(TestConstants.Default_ApplicationId);

			provider = smsc.SmsProviders.Lookup("aspsms");
			provider.TestMode = false;

			sms.Sender = TestConstants.Default_Sender;
			sms.AddRecipient(TestConstants.Default_Recipient);
			sms.Message = "Test SMS";
			sms.Provider = provider;

			status = smsc.SendImmediate(sms);

			Assert.AreEqual(EnMtStatus.Failure, status.Status, "Expected send failure");
			Assert.AreEqual(-1, status.ErrorCode, "Unexpected error code");
			Assert.AreEqual("Service unavailable", status.ErrorMessage, "Invalid error message");
		}

		[Test]
		[VerifyMocks]
		public void Test_SendSms_WithFailure_Multiple()
		{
			ISmsc smsc;
			ISms sms;
			ISmsStatusCollection status;
			ISmsProvider provider;

			_mockAspSmsNet.ExpectGetAlways("ErrorCode", -1);
			_mockAspSmsNet.ExpectGetAlways("ErrorDescription", "Service unavailable");

			smsc = SmsFactory.GetSmsc();
			sms = SmsFactory.NewSms(TestConstants.Default_ApplicationId);

			provider = smsc.SmsProviders.Lookup("aspsms");
			provider.TestMode = false;

			sms.Sender = TestConstants.Default_Sender;
			sms.AddRecipient(TestConstants.Default_Recipient);
			sms.AddRecipient(TestConstants.Default_Recipient_2);
			sms.Message = "Test SMS";
			sms.Provider = provider;

			status = smsc.SendMultipleImmediate(sms);
      
			Assert.AreEqual(EnMtStatus.Failure, status[0].Status, "Expected send failure");
			Assert.AreEqual(-1, status[0].ErrorCode, "Unexpected error code");
			Assert.AreEqual("Service unavailable", status[0].ErrorMessage, "Invalid error message");

			Assert.AreEqual(EnMtStatus.Failure, status[1].Status, "Expected send failure");
			Assert.AreEqual(-1, status[0].ErrorCode, "Unexpected error code");
			Assert.AreEqual("Service unavailable", status[1].ErrorMessage, "Invalid error message");
		}
	}
}