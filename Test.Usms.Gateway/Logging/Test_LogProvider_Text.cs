using System.IO;
using System.Xml;
using Elapsus.Usms.Gateway.Factory;
using Elapsus.Usms.Gateway.Interface.Engine;
using Elapsus.Usms.Gateway.Interface.Envelope;
using Elapsus.Usms.Gateway.Interface.Exceptions;
using Elapsus.Usms.Gateway.Interface.Providers.Gateway;
using Elapsus.Usms.Gateway.Interface.Providers.Log;
using Elapsus.Usms.Gateway.Interface.Providers.Queue;
using Elapsus.Usms.Logging.Text;
using NUnit.Framework;
using Test.Shared.Usms.Gateway;
using TypeMock;

namespace Test.Usms.Gateway.Logging
{
	[TestFixture]
	public class Test_LogProvider_Text
	{
		[SetUp]
		public void SetUp()
		{
			MockManager.Init();
		}

		/// <summary>
		/// Load the test log provider
		/// </summary>
		[Test][VerifyMocks]
		public void LoadProviderConfiguration()
		{
			ISmsc smsc;
			ILogProvider provider;

			MockService.MockTextLogProvider();

			// This call loads the registered providers
			smsc = SmsFactory.GetSmsc();

			// Lookup the text log provider
			provider = smsc.LogProviders.Lookup("TextLogger");

			Assert.IsNotNull(provider, "Log provider not loaded");
		}

		/// <summary>
		/// Verify that an <seealso cref="ProviderConfigurationException"/> exception
		/// is thrown when the fileName parameter is not provided in the 
		/// LogProviders.xml file.
		/// </summary>
		[Test,VerifyMocks]
		public void ParameterNotFound_FileName()
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

			try
			{
				SmsFactory.GetSmsc();
			}
			catch(ProviderConfigurationException ex)
			{
				if (ex.Reason != EnConfigurationReason.MissingParameter)
					Assert.Fail(ex.Message);
			}
		}

		///
		/// The following tests verify that logging events are 
		/// fired and processed
		/// 

		[Test, VerifyMocks]
		public void LogSmsSubmission()
		{
			ISmsc smsc;
			ISms sms;
			ISmsProvider provider;
			Mock<TextLogger> mockTextLogger;

			MockService.MockFakeSmsProvider();
			MockService.MockTextLogProvider();

			mockTextLogger = MockManager.Mock<TextLogger>(Constructor.NotMocked);
			mockTextLogger.ExpectCall("OnAcceptance");
			mockTextLogger.ExpectCall("OnSubmission");

			smsc = SmsFactory.GetSmsc();
			sms = SmsFactory.NewSms(TestConstants.Default_ApplicationId);

			provider = smsc.SmsProviders.Lookup("Fake");

			sms.Sender = TestConstants.Default_Sender;
			sms.AddRecipient(TestConstants.Default_Recipient);
			sms.Message = "TestMessage";
			sms.Provider = provider;

			smsc.SendImmediate(sms);
		}

		/// <summary>
		/// Verify that if a message is enqueued, the corresponding
		/// OnEnqueuent event is triggered
		/// 
		/// The OnSubmission event is not triggered as actual submission
		/// is made asynchronously
		/// </summary>
		[Test, VerifyMocks]
		public void LogSmsEnqueuement()
		{
			ISmsc smsc;
			ISms sms;
			IReadOnlySms enqueuedSms;
			IDequeuer dequeuer;
			Mock<TextLogger> mockTextLogger;
			ISmsProvider provider;

			MockService.MockFakeSmsProvider();
			MockService.MockTextLogProvider();

			mockTextLogger = MockManager.Mock<TextLogger>(Constructor.NotMocked);
			mockTextLogger.ExpectCall("OnEnqueuement");
			//mockTextLogger.ExpectCall("OnAcceptance");

			smsc = SmsFactory.GetSmsc();
			sms = SmsFactory.NewSms(TestConstants.Default_ApplicationId);

			provider = smsc.SmsProviders.Lookup("Fake");

			sms.Sender = TestConstants.Default_Sender;
			sms.AddRecipient(TestConstants.Default_Recipient);
			sms.Message = "TestMessage";
			sms.Provider = provider;

			smsc.Enqueue(sms);
			dequeuer = smsc.GetDequeuer();
			enqueuedSms = dequeuer.Dequeue();
			dequeuer.DequeueCommit(true);

			Helpers.CompareSms(sms, enqueuedSms);
		}

		[Test, VerifyMocks]
		public void LogSmsFailure()
		{
			ISmsc smsc;
			ISms sms;
			ISmsProvider provider;
			Mock<TextLogger> mockTextLogger;

			MockService.MockFakeSmsProvider();
			MockService.MockTextLogProvider();

			mockTextLogger = MockManager.Mock<TextLogger>(Constructor.NotMocked);
			mockTextLogger.ExpectCall("OnAcceptance");
			mockTextLogger.ExpectCall("OnSubmissionFailure");

			smsc = SmsFactory.GetSmsc();
			sms = SmsFactory.NewSms(TestConstants.Default_ApplicationId);

			provider = smsc.SmsProviders.Lookup("Fake");

			sms.Sender = TestConstants.Default_Sender;
			sms.AddRecipient(TestConstants.Default_Recipient);
			sms.Message = "TestMessage";
			sms.Provider = provider;
			sms.CustomProperties["FailCode"] = -1;

			smsc.SendImmediate(sms);
		}

		[Test, VerifyMocks]
		public void LogSmsSubmission_Multiple()
		{
			ISmsc smsc;
			ISms sms;
			ISmsProvider provider;
			Mock<TextLogger> mockTextLogger;

			MockService.MockFakeSmsProvider();
			MockService.MockTextLogProvider();

			mockTextLogger = MockManager.Mock<TextLogger>(Constructor.NotMocked);
			mockTextLogger.ExpectCall("OnAcceptance");
			mockTextLogger.ExpectCall("OnSubmission");

			smsc = SmsFactory.GetSmsc();
			sms = SmsFactory.NewSms(TestConstants.Default_ApplicationId);

			provider = smsc.SmsProviders.Lookup("Fake");

			sms.Sender = TestConstants.Default_Sender;
			sms.AddRecipient(TestConstants.Default_Recipient);
			sms.AddRecipient(TestConstants.Default_Recipient_2);
			sms.Message = "TestMessage";
			sms.Provider = provider;

			smsc.SendMultipleImmediate(sms);
		}

		[Test, VerifyMocks]
		public void LogSmsFailure_Multiple()
		{
			ISmsc smsc;
			ISms sms;
			ISmsProvider provider;
			Mock<TextLogger> mockTextLogger;

			MockService.MockFakeSmsProvider();
			MockService.MockTextLogProvider();

			mockTextLogger = MockManager.Mock<TextLogger>(Constructor.NotMocked);
			mockTextLogger.ExpectCall("OnAcceptance");
			mockTextLogger.ExpectCall("OnSubmissionFailure");

			smsc = SmsFactory.GetSmsc();
			sms = SmsFactory.NewSms(TestConstants.Default_ApplicationId);

			provider = smsc.SmsProviders.Lookup("Fake");

			sms.Sender = TestConstants.Default_Sender;
			sms.AddRecipient(TestConstants.Default_Recipient);
			sms.AddRecipient(TestConstants.Default_Recipient_2);
			sms.Message = "TestMessage";
			sms.Provider = provider;
			sms.CustomProperties["FailCode"] = -1;

			smsc.SendMultipleImmediate(sms);
		}
	}
}