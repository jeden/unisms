using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Elapsus.Usms.Gateway.Factory;
using Elapsus.Usms.Gateway.Interface.Engine;
using Elapsus.Usms.Gateway.Interface.Envelope;
using Elapsus.Usms.Gateway.Interface.Exceptions;
using Elapsus.Usms.Gateway.Interface.Providers.Gateway;
using Elapsus.Usms.Gateway.Providers.Clickatell.Soap.com.clickatell.api;
using Test.Shared.Usms.Gateway;
using TypeMock;
using NUnit.Framework;

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
	public class Test_SmsProvider_Clickatell_Soap
	{
		private Mock<PushServerWS> _mockClickatell;

		[SetUp]
		public void SetUp()
		{
			MockManager.Init();

			_mockClickatell = MockManager.Mock<PushServerWS>(Constructor.Mocked);
		}

		/// <summary>
		/// Load the Clickatell provider and verify it allows
		/// send of simple textual messages
		/// </summary>
		[Test,VerifyMocks]
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

			// Lookup the Clickatell provider
			provider = smsc.SmsProviders.Lookup("Clickatell");

			// Verify the Clickatell provider is correctly loaded
			Assert.IsNotNull(provider, "Clickatell provider not found");
			Assert.AreEqual("clickatell", provider.Name.ToLower(), "Invalid sms provider name");
			Assert.IsTrue(provider.Capabilities.SimpleSms, "Clickatell provider not enabled to send simple SMS");
			Assert.IsTrue(provider.Capabilities.TestMode, "Clickatell provider not implementing a test mode");

			// Read the internal provider state in order to get access
			// to private members and display some information
			mockProvider = new ObjectState(provider);
			mockSettings = new ObjectState(provider.Configuration);

			settings = (Dictionary<string, string>)mockSettings.GetField("_settings");

			Console.Out.WriteLine("Name:     " + provider.Name);
			Console.Out.WriteLine("Assembly: " + mockProvider.GetField("_assemblyName"));
			Console.Out.WriteLine("Class:    " + mockProvider.GetField("_className"));
			Console.Out.WriteLine("api_id:   " + settings["api_id"]);
			Console.Out.WriteLine("user:     " + settings["user"]);
			Console.Out.WriteLine("password: " + settings["password"]);
		}

		/// <summary>
		/// Verify that the Clickatell provider is loaded 
		/// by sending a test message using the test mode
		/// </summary>
		[Test,VerifyMocks]
		public void LoadClickatellProvider()
		{
			ISmsc smsc;
			ISms sms;
			ISmsProvider provider;
			ISmsStatus status;

			smsc = SmsFactory.GetSmsc();
			provider = smsc.SmsProviders.Lookup("Clickatell");

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
		/// is thrown when the api_id parameter is not provided in the 
		/// SmsProviders.xml file.
		/// </summary>
		[Test,VerifyMocks]
		public void ParameterNotFound_api_id()
		{
			XmlReader xmlReader;

			xmlReader = XmlReader.Create(new StringReader("" +
			                                              "<?xml version='1.0' encoding='utf-8' ?>\n" +
			                                              "<usmsgw>\n" +
			                                              "<smsProviders>\n" +
			                                              "<smsProvider>\n" +
			                                              "<name>clickatell</name>\n" +
			                                              "<assembly>Elapsus.Usms.Gateway.Providers.Clickatell.Soap</assembly>\n" +
			                                              "<class>Elapsus.Usms.Gateway.Providers.Clickatell.Soap.ClickatellProvider</class>\n" +
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
			catch (ProviderConfigurationException ex)
			{
				Assert.AreEqual(EnConfigurationReason.MissingParameter, ex.Reason, "Unexpected reason");
			}
		}
		
		/// <summary>
		/// Verify that an <seealso cref="ProviderConfigurationException"/> exception
		/// is thrown when the user parameter is not provided in the 
		/// SmsProviders.xml file.
		/// </summary>
		[Test,VerifyMocks]
		public void ParameterNotFound_user()
		{
			XmlReader xmlReader;

			xmlReader = XmlReader.Create(new StringReader("" +
			                                              "<?xml version='1.0' encoding='utf-8' ?>\n" +
			                                              "<usmsgw>\n" +
			                                              "<smsProviders>\n" +
			                                              "<smsProvider>\n" +
			                                              "<name>clickatell</name>\n" +
			                                              "<assembly>Elapsus.Usms.Gateway.Providers.Clickatell.Soap</assembly>\n" +
			                                              "<class>Elapsus.Usms.Gateway.Providers.Clickatell.Soap.ClickatellProvider</class>\n" +
			                                              "<features>\n" +
			                                              "<feature name='simpleSms' value='yes' />\n" +
			                                              "<feature name='testMode' value='yes' />\n" +
			                                              "</features>\n" +
			                                              "<configuration>\n" +
			                                              "<set key='testMode' value='yes' />\n" +
			                                              "<set key='api_id' value='12345' />\n" +
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
			catch (ProviderConfigurationException ex)
			{
				Assert.AreEqual(EnConfigurationReason.MissingParameter, ex.Reason, "Unexpected reason");
			}
		}

		/// <summary>
		/// Verify that an <seealso cref="ProviderConfigurationException"/> exception
		/// is thrown when the password parameter is not provided in the 
		/// SmsProviders.xml file.
		/// </summary>
		[Test,VerifyMocks]
		public void ParameterNotFound_password()
		{
			XmlReader xmlReader;

			xmlReader = XmlReader.Create(new StringReader("" +
			                                              "<?xml version='1.0' encoding='utf-8' ?>\n" +
			                                              "<usmsgw>\n" +
			                                              "<smsProviders>\n" +
			                                              "<smsProvider>\n" +
			                                              "<name>clickatell</name>\n" +
			                                              "<assembly>Elapsus.Usms.Gateway.Providers.Clickatell.Soap</assembly>\n" +
			                                              "<class>Elapsus.Usms.Gateway.Providers.Clickatell.Soap.ClickatellProvider</class>\n" +
			                                              "<features>\n" +
			                                              "<feature name='simpleSms' value='yes' />\n" +
			                                              "<feature name='testMode' value='yes' />\n" +
			                                              "</features>\n" +
			                                              "<configuration>\n" +
			                                              "<set key='testMode' value='yes' />\n" +
			                                              "<set key='api_id' value='12345' />\n" +
			                                              "<set key='user' value='usertest' />\n" +
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
			catch (ProviderConfigurationException ex)
			{
				Assert.AreEqual(EnConfigurationReason.MissingParameter, ex.Reason, "Unexpected reason");
			}
		}

		/// <summary>
		/// Verify that an <seealso cref="ProviderConfigurationException"/> exception
		/// is thrown when the password parameter is not provided in the 
		/// SmsProviders.xml file.
		/// </summary>
		[Test,VerifyMocks]
		public void InvalidFormat_ApiId()
		{
			XmlReader xmlReader;

			xmlReader = XmlReader.Create(new StringReader("" +
			                                              "<?xml version='1.0' encoding='utf-8' ?>\n" +
			                                              "<usmsgw>\n" +
			                                              "<smsProviders>\n" +
			                                              "<smsProvider>\n" +
			                                              "<name>clickatell</name>\n" +
			                                              "<assembly>Elapsus.Usms.Gateway.Providers.Clickatell.Soap</assembly>\n" +
			                                              "<class>Elapsus.Usms.Gateway.Providers.Clickatell.Soap.ClickatellProvider</class>\n" +
			                                              "<features>\n" +
			                                              "<feature name='simpleSms' value='yes' />\n" +
			                                              "<feature name='testMode' value='yes' />\n" +
			                                              "</features>\n" +
			                                              "<configuration>\n" +
			                                              "<set key='testMode' value='yes' />\n" +
			                                              "<set key='api_id' value='apiid' />\n" +
			                                              "<set key='user' value='usertest' />\n" +
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
			catch (ProviderConfigurationException ex)
			{
				Assert.AreEqual(EnConfigurationReason.ExpectedNumeric, ex.Reason, "Unexpected reason");
			}
		}

		/// <summary>
		/// Send a SMS using clickatell
		/// Actual send is mocked, so no real SMS is
		/// submitted for delivery
		/// </summary>
		[Test,VerifyMocks]
		public void Test_SendSms_Single()
		{
			ISmsc smsc;
			ISms sms;
			ISmsStatus status;
			ISmsProvider provider;

			// Mock the call to sendmsg
			// Returning an empty string should mean the sms has been sent
			_mockClickatell.ExpectAndReturn("sendmsg", new[] {""} );
			_mockClickatell.ExpectAndReturn("auth", "OK: testsessionid");

			smsc = SmsFactory.GetSmsc();
			sms = SmsFactory.NewSms(TestConstants.Default_ApplicationId);

			provider = smsc.SmsProviders.Lookup("clickatell");
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
		/// Send a SMS using clickatell
		/// Actual send is mocked, so no real SMS is
		/// submitted for delivery
		/// </summary>
		[Test,VerifyMocks]
		public void Test_SendSms_Multiple()
		{
			ISmsc smsc;
			ISms sms;
			ISmsStatusCollection status;
			ISmsProvider provider;

			// Mock the call to sendmsg
			// Returning an empty string should mean the sms has been sent
			_mockClickatell.ExpectAndReturn("sendmsg", new[] { "", "" });
			_mockClickatell.ExpectAndReturn("auth", "OK: testsessionid");

			smsc = SmsFactory.GetSmsc();
			sms = SmsFactory.NewSms(TestConstants.Default_ApplicationId);

			provider = smsc.SmsProviders.Lookup("clickatell");
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

		[Test,VerifyMocks]
		public void Test_SendSms_WithFailure()
		{
			ISmsc smsc;
			ISms sms;
			ISmsStatus status;
			ISmsProvider provider;

			// Mock the call to sendmsg
			// Returning an empty string should mean the sms has been sent
			_mockClickatell.ExpectAndReturn("sendmsg", new[] { "ERR: 004, Service unavailable" });
			_mockClickatell.ExpectAndReturn("auth", "OK: testsessionid");

			smsc = SmsFactory.GetSmsc();
			sms = SmsFactory.NewSms(TestConstants.Default_ApplicationId);

			provider = smsc.SmsProviders.Lookup("clickatell");
			provider.TestMode = false;

			sms.Sender = TestConstants.Default_Sender;
			sms.AddRecipient(TestConstants.Default_Recipient);
			sms.Message = "Test SMS";
			sms.Provider = provider;

			status = smsc.SendImmediate(sms);

			Assert.AreEqual(EnMtStatus.Failure, status.Status, "Expected send failure");
			Assert.AreEqual(4, status.ErrorCode, "Invalid error code");
			Assert.AreEqual("Service unavailable", status.ErrorMessage, "Invalid error message");
		}

		[Test,VerifyMocks]
		public void Test_SendSms_WithFailure_Multiple()
		{
			ISmsc smsc;
			ISms sms;
			ISmsStatusCollection status;
			ISmsProvider provider;

			// Mock the call to sendmsg
			// Returning an empty string should mean the sms has been sent
			_mockClickatell.ExpectAndReturn("sendmsg", new[] { "ERR: 004, Service unavailable", "ERR: 005, Invalid recipient" });
			_mockClickatell.ExpectAndReturn("auth", "OK: testsessionid");

			smsc = SmsFactory.GetSmsc();
			sms = SmsFactory.NewSms(TestConstants.Default_ApplicationId);

			provider = smsc.SmsProviders.Lookup("clickatell");
			provider.TestMode = false;

			sms.Sender = TestConstants.Default_Sender;
			sms.AddRecipient(TestConstants.Default_Recipient);
			sms.AddRecipient(TestConstants.Default_Recipient_2);
			sms.Message = "Test SMS";
			sms.Provider = provider;

			status = smsc.SendMultipleImmediate(sms);

			Assert.AreEqual(EnMtStatus.Failure, status[0].Status, "Expected send failure");
			Assert.AreEqual(4, status[0].ErrorCode, "Invalid error code");
			Assert.AreEqual("Service unavailable", status[0].ErrorMessage, "Invalid error message");

			Assert.AreEqual(EnMtStatus.Failure, status[1].Status, "Expected send failure");
			Assert.AreEqual(5, status[1].ErrorCode, "Invalid error code");
			Assert.AreEqual("Invalid recipient", status[1].ErrorMessage, "Invalid error message");
		}

		[Test,VerifyMocks]
		public void Test_SessionExpiration_Reconnect()
		{
			ISmsc smsc;
			ISms sms;
			ISmsStatus status;
			ISmsProvider provider;

			// Mock the call to sendmsg
			// Returning an empty string should mean the sms has been sent
			_mockClickatell.ExpectAndReturn("sendmsg", new[] { "" });
			_mockClickatell.ExpectAndReturn("auth", "OK: testsessionid");

			smsc = SmsFactory.GetSmsc();
			sms = SmsFactory.NewSms(TestConstants.Default_ApplicationId);

			provider = smsc.SmsProviders.Lookup("clickatell");
			provider.TestMode = false;

			sms.Sender = TestConstants.Default_Sender;
			sms.AddRecipient(TestConstants.Default_Recipient);
			sms.Message = "Test SMS";
			sms.Provider = provider;

			// Send a message with a valid session id
			// The message must be accepted with no error
			status = smsc.SendImmediate(sms);

			Assert.AreEqual(EnMtStatus.Accepted, status.Status, "Invalid SMS status");
			Assert.AreEqual(false, status.TestMode, "Sms sent in test mode");
			Assert.IsTrue(string.IsNullOrEmpty(status.ErrorMessage), "No error message expected");

			// Change the return value of the sendmsg api to simulate
			// an expired sesssion
			_mockClickatell.ExpectAndReturn("sendmsg", new[] {"ERR: 003, Session expired"});
			_mockClickatell.ExpectAndReturn("auth", "OK: newtestsessionid");
			_mockClickatell.ExpectAndReturn("sendmsg", new[] { "" });

			sms = SmsFactory.NewSms(TestConstants.Default_ApplicationId);

			provider = smsc.SmsProviders.Lookup("clickatell");
			provider.TestMode = false;

			sms.Sender = TestConstants.Default_Sender;
			sms.AddRecipient(TestConstants.Default_Recipient);
			sms.Message = "Test SMS";
			sms.Provider = provider;

			status = smsc.SendImmediate(sms);

			Assert.AreEqual(EnMtStatus.Accepted, status.Status, "Invalid SMS status");
			Assert.AreEqual(false, status.TestMode, "Sms sent in test mode");
			Assert.IsTrue(string.IsNullOrEmpty(status.ErrorMessage), "No error message expected");
		}

		[Test, VerifyMocks]
		public void Test_SessionExpiration_RenewalFailure()
		{
			ISmsc smsc;
			ISms sms;
			ISmsStatus status;
			ISmsProvider provider;

			_mockClickatell.ExpectAndReturn("sendmsg", new[] { "ERR: 003, Session expired" }, 2);
			_mockClickatell.ExpectAndReturn("auth", "OK: testsessionid");
			_mockClickatell.ExpectAndReturn("auth", "OK: newtestsessionid");

			smsc = SmsFactory.GetSmsc();
			sms = SmsFactory.NewSms(TestConstants.Default_ApplicationId);

			provider = smsc.SmsProviders.Lookup("clickatell");
			provider.TestMode = false;

			sms.Sender = TestConstants.Default_Sender;
			sms.AddRecipient(TestConstants.Default_Recipient);
			sms.Message = "Test SMS";
			sms.Provider = provider;

			status = smsc.SendImmediate(sms);

			Assert.AreEqual(EnMtStatus.Failure, status.Status, "Expected send failure");
			Assert.AreEqual(3, status.ErrorCode, "Invalid error code");
			Assert.AreEqual("Session expired", status.ErrorMessage, "Invalid error message");
		}
	}
}