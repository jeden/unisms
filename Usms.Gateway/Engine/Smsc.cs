using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using Elapsus.Usms.Gateway.Envelope;
using Elapsus.Usms.Gateway.Interface.Engine;
using Elapsus.Usms.Gateway.Interface.Envelope;
using Elapsus.Usms.Gateway.Interface.Exceptions;
using Elapsus.Usms.Gateway.Interface.Providers;
using Elapsus.Usms.Gateway.Interface.Providers.Gateway;
using Elapsus.Usms.Gateway.Interface.Providers.Queue;
using Elapsus.Usms.Gateway.Providers;
using Elapsus.Usms.Gateway.Providers.Gateway;
using Elapsus.Usms.Gateway.Providers.Queue;
using Elapsus.Usms.Queuing;

namespace Elapsus.Usms.Gateway.Engine
{
	internal delegate void ForwarderDelegate(IReadOnlySms message);

	internal class Smsc : BaseSmsc, ISmsc, IDequeueService
	{
		#region Private Data Members

		private SmsProvider _defaultSmsProvider;
		private QueueProvider _defaultQueueProvider;
		private QueueHandler _defaultQueueHandler;
		private readonly SmsProviders _smsProviders;
		private readonly QueueProviders _queueProviders;
		private readonly IDictionary<string, ISmsGateway> _smsGateways;
		private readonly IDictionary<string, IQueueHandler> _queueHandlers;

		private readonly DequeueSynchronizer _dequeueSynchronizer;

		#endregion

		#region Constructors

		/// <summary>
		/// Standard constructor.
		/// New instances must be created using the SmsFactory class.
		/// </summary>
		internal Smsc()
		{
			_smsProviders = new SmsProviders();
			_queueProviders = new QueueProviders();

			_smsGateways = new Dictionary<string, ISmsGateway>();
			_queueHandlers = new Dictionary<string, IQueueHandler>();

			ReadSmsProvidersConfiguration();
			LoadSmsProviders();
			ReadQueueProvidersConfiguration();
			LoadQueueProviders();
		}

		/// <summary>
		/// Constructor for EnqueueService mode
		/// New instances must be created using the SmsFactory class
		/// </summary>
		/// <param name="dequeuers">
		/// Number of dequeuers to be instantiated<br/>
		/// The number of dequeuers cannot be changed after instantiation
		/// </param>
		internal Smsc(int dequeuers) : this()
		{
			_dequeueSynchronizer = new DequeueSynchronizer(dequeuers, _defaultQueueHandler, Forward);
		}

		#endregion

		#region ISmsc Members

		public IProviders<ISmsProvider> SmsProviders { get { return _smsProviders; } }
		public IProviders<IQueueProvider> QueueProviders { get { return _queueProviders; } }

		/// <summary>
		/// Send an immediate sms message to a single recipient
		/// </summary>
		/// <remarks>
		/// This method must be used for a single recipient only.<br/>
		/// For multiple recipients use <see cref="SendMultipleImmediate"/>
		/// </remarks>
		/// <exception cref="InvalidSmsOperationException">
		///		<see cref="EnSmsOperation.AlreadyProcessed" />: 
		///   If the SMS has already been processed
		/// </exception>
		/// <exception cref="InvalidSmsOperationException">
		///   <see cref="EnSmsOperation.MultipleRecipients" />:
		///   If more than 1 recipient has been added
		/// </exception>
		/// <param name="message">The message to be sent</param>
		/// <returns>
		/// <see cref="ISmsStatus"/> instance containing status information
		/// </returns>
		public ISmsStatus SendImmediate(ISms message)
		{

			if (message.Recipients.Count > 1)
				throw new InvalidSmsOperationException(EnSmsOperation.MultipleRecipients, message);

			return SendMultipleImmediate(message)[0];
		}

		/// <summary>
		/// Send an immediate sms message to multiple recipients
		/// </summary>
		/// <remarks>
		/// To be used when the number of recipients is greater than 1
		/// </remarks>
		/// <exception cref="InvalidSmsOperationException">
		///		<see cref="EnSmsOperation.AlreadyProcessed"/>:
		///   If the SMS has been already processed
		/// </exception>
		/// <param name="message">The message to be sent</param>
		/// <returns>
		/// A collection of <see cref="ISmsStatus" /> containing
		/// the status for each recipient
		/// </returns>
		public ISmsStatusCollection SendMultipleImmediate(ISms message)
		{
			ISmsStatusCollection status;
			ISmsProvider smsProvider;
			ISmsGateway smsGateway;
			Sms sms;

			sms = (Sms) message;

			PreProcessSms(sms);

			smsProvider = sms.Provider ?? _defaultSmsProvider;

			if (smsProvider == null)
				throw new ProviderNotFoundException();

			sms.Provider = smsProvider;

			NotifyAcceptance(sms);

			smsGateway = _smsGateways[smsProvider.Name];
			status = smsGateway.Send(sms);

			if (status.Successful)
				NotifySubmission(sms, status);
			else
				NotifySubmissionFailure(sms, status);

			sms.SetProcessed();

			return status;
		}

		public void Enqueue(ISms message)
		{
			Sms sms;

			sms = (Sms) message;

			PreProcessSms(sms);

			//NotifyAcceptance(sms);

			_defaultQueueHandler.Enqueue(sms);

			NotifyEnqueuement(sms);
		}

		public IDequeuer GetDequeuer()
		{
			return _defaultQueueHandler.GetDequeuer();
		}

		private void Forward(IReadOnlySms message)
		{
			ISmsStatusCollection status;
			ISmsProvider smsProvider;
			ISmsGateway smsGateway;
			Sms sms;

			sms = Sms.SafeCast(message);

			smsProvider = sms.Provider ?? _defaultSmsProvider;

			sms.Provider = smsProvider;

			smsGateway = _smsGateways[smsProvider.Name];
			status = smsGateway.Send(sms);

			if (status.Successful)
				NotifySubmission(sms, status);
			else
				NotifySubmissionFailure(sms, status);

			sms.SetProcessed();
		}


		#endregion

		#region IDequeueService Members

		public bool IsQueueEmpty()
		{
			return _defaultQueueHandler.IsEmpty();
		}

		public int RunningWorkers { get { return _dequeueSynchronizer.RunningWorkers; } }
		public int TotalWorkers { get { return _dequeueSynchronizer.TotalWorkers; } }

		public void Start()
		{
			_dequeueSynchronizer.ResumeWorkers();			
		}

		public void Suspend()
		{
			_dequeueSynchronizer.SuspendWorkers();
		}

		public void Terminate()
		{
			_dequeueSynchronizer.TerminateWorkers();
		}

		#endregion

		#region Private Members

		/// <summary>
		/// Read the SmsProviders.xml file
		/// </summary>
		private void ReadSmsProvidersConfiguration()
		{
			XmlReaderSettings settings;
			XmlReader config;
			SmsProvider smsProvider;
			IConfiguration smsConfiguration;
			SmsProviderCapabilities providerCapabilities;
			string key, value;

			settings = new XmlReaderSettings();
			settings.Schemas.Add(null, Constants.SMS_PROVIDERS_CONFIG_SCHEMA);
			settings.ValidationType = ValidationType.Schema;
			settings.ValidationEventHandler += delegate(object sender, ValidationEventArgs args)
			{
				throw args.Exception;
			};
			config = XmlReader.Create(Constants.SMS_PROVIDERS_CONFIG, settings);

			config.ReadStartElement("usmsgw");
			config.ReadStartElement("smsProviders");
			while (config.ReadToFollowing("smsProvider"))
			{
				value = config.GetAttribute("default");

				config.Read();

				smsProvider = new SmsProvider
				{
					Name = config.ReadElementString("name"),
					AssemblyName = config.ReadElementString("assembly"),
					ClassName = config.ReadElementString("class")
				};

				if (IsFeatureEnabled(value))
					_defaultSmsProvider = smsProvider;

				_smsProviders.Add(smsProvider.Name.Trim().ToLower(), smsProvider);
				providerCapabilities = (SmsProviderCapabilities)smsProvider.Capabilities;
				smsConfiguration = smsProvider.Configuration;

				////
				/// Features
				///
				config.ReadStartElement("features");
				config.ReadToFollowing("feature");
				do
				{
					key = config.GetAttribute("name");
					value = config.GetAttribute("value");

					switch (key)
					{
						case "simpleSms":
							providerCapabilities.SimpleSms = IsFeatureEnabled(value);
							break;

						case "testMode":
							providerCapabilities.TestMode = IsFeatureEnabled(value);
							break;
					}
				} while (config.ReadToNextSibling("feature"));

				////
				/// Configuration
				/// 
				config.ReadToFollowing("configuration");
				config.ReadToFollowing("set");
				do
				{
					key = config.GetAttribute("key");
					value = config.GetAttribute("value");

					if ((string.IsNullOrEmpty(key) == false) && (string.IsNullOrEmpty(value) == false))
					{
						switch (key)
						{
							case "testMode":
								smsProvider.TestMode = IsFeatureEnabled(value);
								break;

							default:
								smsConfiguration.Settings[key] = value;
								break;
						}
					}
				} while (config.ReadToNextSibling("set"));
			}
		}


		private void ReadQueueProvidersConfiguration()
		{
			XmlReaderSettings settings;
			XmlReader config;
			QueueProvider queueProvider;
			//string key, value;
			string enabled, def;

			settings = new XmlReaderSettings();
			settings.Schemas.Add(null, Constants.QUEUE_PROVIDERS_CONFIG_SCHEMA);
			settings.ValidationType = ValidationType.Schema;
			config = XmlReader.Create(Constants.QUEUE_PROVIDERS_CONFIG, settings);

			config.ReadStartElement("usmsgw");
			config.ReadStartElement("queueProviders");
			while (config.ReadToFollowing("queueProvider"))
			{
				def = config.GetAttribute("default");
				enabled = config.GetAttribute("enabled");

				config.Read();
				queueProvider = new QueueProvider
				{
					Name = config.ReadElementString("name"),
					AssemblyName = config.ReadElementString("assembly"),
					ClassName = config.ReadElementString("class"),
					Enabled = IsFeatureEnabled(enabled)
				};

				_queueProviders.Add(queueProvider.Name.Trim().ToLower(), queueProvider);

				if (IsFeatureEnabled(def))
				{
					if (_defaultQueueProvider != null)
						throw ProviderConfigurationException.MultipleDefaultProviders(EnProviderType.QueueProvider);
					_defaultQueueProvider = queueProvider;
				}
				/*
				///
				/// Configuration
				/// 
				config.ReadToFollowing("configuration");
				config.ReadToFollowing("set");
				do
				{
					key = config.GetAttribute("key");
					value = config.GetAttribute("value");

					if ((string.IsNullOrEmpty(key) == false) && (string.IsNullOrEmpty(value) == false))
						queueProvider.Configuration.Settings[key] = value;

				} while (config.ReadToNextSibling("set"));
				 */
			}

			if (_defaultQueueProvider == null)
				throw ProviderConfigurationException.MissingDefaultProvider(EnProviderType.QueueProvider);
		}

		/// <summary>
		/// Instantiate the Sms Providers
		/// </summary>
		private void LoadSmsProviders()
		{
			Assembly assembly;
			ISmsGateway smsGateway;

			foreach (SmsProvider provider in _smsProviders.Values)
			{
				var assemblyName = new AssemblyName(provider.AssemblyName);
				assembly = Assembly.Load(assemblyName);
				smsGateway = assembly.CreateInstance(provider.ClassName, false, BindingFlags.Public | BindingFlags.Instance | BindingFlags.CreateInstance, null, new object[] { provider }, null, null) as ISmsGateway;
				if (smsGateway != null)
				{
					smsGateway.VerifyConfiguration();
					smsGateway.Initialize();
					_smsGateways[provider.Name] = smsGateway;
				}
			}
		}

		/// <summary>
		/// Instantiate the Sms Providers
		/// </summary>
		private void LoadQueueProviders()
		{
			Assembly assembly;
			QueueHandler queueHandler;

			foreach (QueueProvider provider in _queueProviders.Values)
			{
				var assemblyName = new AssemblyName(provider.AssemblyName);
				assembly = Assembly.Load(assemblyName);
				queueHandler = assembly.CreateInstance(provider.ClassName, false, BindingFlags.Public | BindingFlags.Instance | BindingFlags.CreateInstance, null, new object[] { provider }, null, null) as QueueHandler;
				if (queueHandler != null)
				{
					queueHandler.VerifyConfiguration();
					queueHandler.Initialize();
					_queueHandlers[provider.Name] = queueHandler;
					if (provider == _defaultQueueProvider)
						_defaultQueueHandler = queueHandler;
				}
			}
		}

		private static void PreProcessSms(Sms sms)
		{
			if (sms.Processed)
				throw new InvalidSmsOperationException(EnSmsOperation.AlreadyProcessed, sms);

			sms.VerifyMessage();
		}

		#endregion
	}
}
