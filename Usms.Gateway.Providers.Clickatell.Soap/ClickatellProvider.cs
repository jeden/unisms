using System.Text.RegularExpressions;
using Elapsus.Usms.Gateway.Interface.Envelope;
using Elapsus.Usms.Gateway.Interface.Exceptions;
using Elapsus.Usms.Gateway.Interface.Providers.Gateway;
using Elapsus.Usms.Gateway.Providers.Clickatell.Soap.com.clickatell.api;

namespace Elapsus.Usms.Gateway.Providers.Clickatell.Soap
{
	public sealed class ClickatellProvider : SmsGateway
	{
		private readonly Regex _regexSuccess = new Regex(@"^OK: (?<info>.*)$", RegexOptions.Compiled);
		private readonly Regex _regexErrorMessage = new Regex(@"^ERR: (?<errorcode>\d{3}), (?<errormessage>.*)$", RegexOptions.Compiled);
		private readonly Regex _regexMessageId = new Regex(@"^ID: (?<reference>.*)$", RegexOptions.Compiled);
		private PushServerWS _server;
		private int _apiId;
		private string _sessionId = "";
		private string _user;
		private string _password;

		public ClickatellProvider(ISmsProvider provider) : base(provider)
		{
			IsConnected = false;
		}

		public override void Initialize() { }

		public override void Connect()
		{
			if (Provider.TestMode == false)
			{
				string msg;
				Match match;

				if (_server == null)
					_server = new PushServerWS();

				msg = _server.auth(_apiId, _user, _password);

				match = _regexSuccess.Match(msg);
				_sessionId = match.Result("${info}");
			}

			IsConnected = true;
		}

		public override void Disconnect()
		{
			if (_server != null)
			{
				_sessionId = "";
				_server.Dispose();
				_server = null;
			}
			IsConnected = false;
		}

		/// <summary>
		/// Forward a SMS Send request to the gateway
		/// Made a private method to allow refreshing of the 
		/// session id - if session is expired, it attempts
		/// 1 time only, then if the error persists 
		/// does not retry anymore
		/// </summary>
		/// <param name="sms">The sms to be sent</param>
		/// <param name="refresingSession">
		/// true if the session id is expired and a new send attempt is
		/// being done after reconnecting with a new session id
		/// </param>
		/// <returns></returns>
		private ISmsStatusCollection Send(ISms sms, bool refresingSession)
		{
			ISmsStatusCollection status;
			SmsStatus smsStatus;
			string[] errorMessages;
			bool retry = false;

			if (Provider.TestMode == false)
			{
				string[] recipients;

				recipients = new string[sms.Recipients.Count];
				for (int count = 0; count < sms.Recipients.Count; ++count)
					recipients[count] = sms.Recipients[count];

				lock (_server)
				{
					errorMessages = _server.sendmsg(_sessionId, _apiId, null, null, recipients, sms.Sender, sms.Message, 0, 1, 0, 0, 3, 0, 3, 1, 0, sms.ReferenceId.ToString(), 0, "SMS_TEXT", string.Empty, string.Empty, 1440);
					if ((errorMessages.Length == 1) && (IsSessionError(errorMessages[0]) && (refresingSession == false)))
					{
						Connect();
						retry = true;
					}
				}
			}
			else
				errorMessages = new string[sms.Recipients.Count];

			if (retry)
				status = Send(sms, true);
			else
			{
				status = new SmsStatusCollection(sms);
				for (int count = 0; count < sms.Recipients.Count; ++count)
				{
					smsStatus = new SmsStatus(sms.Recipients[count]);
					smsStatus.TestMode = Provider.TestMode;

					CheckResponse(smsStatus, errorMessages[count]);

					status.Add(smsStatus);
				}
			}

			return status;
		}

		protected override ISmsStatusCollection OnSend(ISms sms)
		{
			return Send(sms, false);
		}

		public override void VerifyConfiguration()
		{
			if (ContainsSetting(Provider.Configuration, "api_id") == false)
				throw ProviderConfigurationException.MissingParameter(Provider.Name, "api_id");

			if (IsNumeric(Provider.Configuration, "api_id") == false)
				throw ProviderConfigurationException.ExpectedNumeric(Provider.Name, "api_id");

			if (ContainsSetting(Provider.Configuration, "user") == false)
				throw ProviderConfigurationException.MissingParameter(Provider.Name, "user");

			if (ContainsSetting(Provider.Configuration, "password") == false)
				throw ProviderConfigurationException.MissingParameter(Provider.Name, "password");

			_apiId = int.Parse(Provider.Configuration.Settings["api_id"]);
			_user = Provider.Configuration.Settings["user"];
			_password = Provider.Configuration.Settings["password"];

		}

		private void CheckResponse(SmsStatus status, string errorMessage)
		{
			if (status.TestMode == false)
			{
				if (errorMessage == null)
					status.SetError(EnMtStatus.Failure, -1, "No response from the server");
				else if (_regexMessageId.IsMatch(errorMessage))
					status.ProviderReference = _regexMessageId.Match(errorMessage).Result("${reference}");
				else if (_regexErrorMessage.IsMatch(errorMessage))
				{
					Match match;
					match = _regexErrorMessage.Match(errorMessage);
					status.SetError(EnMtStatus.Failure, int.Parse(match.Result("${errorcode}")), match.Result("${errormessage}"));
				}
			}
		}

		public bool IsSessionError(string errorMessage)
		{
			bool ret;

			ret = false;

			if ((string.IsNullOrEmpty(errorMessage) == false) && (_regexErrorMessage.IsMatch(errorMessage)))
			{
				Match match;
				string errorCode;

				match = _regexErrorMessage.Match(errorMessage);
				errorCode = match.Result("${errorcode}");
				switch (errorCode)
				{
					case "003": // Session expired
					case "005": // Missing session
						ret = true;
						break;
				}
			}

			return ret;
		}
	}
}