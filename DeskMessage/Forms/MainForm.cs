using System.Text;
using System.Windows.Forms;
using Elapsus.Usms.Gateway.Factory;
using Elapsus.Usms.Gateway.Interface.Engine;
using Elapsus.Usms.Gateway.Interface.Envelope;
using Elapsus.Usms.Gateway.Interface.Providers.Gateway;

namespace Elapsus.Usms.DeskMessage.Forms
{
	public partial class MainForm : Form
	{
		private static readonly ISmsc _smsc = SmsFactory.GetSmsc();

		public static ISmsc Smsc { get { return _smsc; } }

		public MainForm()
		{
			InitializeComponent();
		}

		private void Main_Load(object sender, System.EventArgs e)
		{
			_bindProviders.DataSource = _smsc.SmsProviders.Values;
			_listProviders.DisplayMember = "Name";
			_listProviders.DataSource = _bindProviders;

			_listSenders.SelectedIndex = 0;
			_listRecipients.SelectedIndex = 0;
		}

		private void _btnSend_Click(object sender, System.EventArgs e)
		{
			ISms sms;
			ISmsStatus status;
			StringBuilder msg;
			ISmsProvider provider;

			msg = new StringBuilder();
			provider = (ISmsProvider) _listProviders.SelectedValue;
			provider.TestMode = false;
			provider.Configuration.Settings["api_id"] = "3125788";
			provider.Configuration.Settings["user"] = "jeden";
			provider.Configuration.Settings["password"] = "kracos4";

			sms = SmsFactory.NewSms(Program.ApplicationId);
			sms.Sender = _listSenders.Text;
			sms.AddRecipient(_listRecipients.Text);
			sms.Message = _editMessage.Text;
			sms.Provider = provider;

			msg.AppendFormat("Provider: {0}\n", provider.Name);

			if (provider.TestMode)
				msg.Append("Test mode enabled\n");

			foreach (string key in provider.Configuration.Settings.Keys)
				msg.AppendFormat("{0}: {1}\n", key, provider.Configuration.Settings[key]);

			msg.AppendLine();

			msg.AppendFormat("Sender: {0}\n", sms.Sender);
			msg.AppendFormat("Recipient: {0}\n", sms.Recipients[0]);

			msg.AppendLine();

			status = _smsc.SendImmediate(sms);
			if (status.Status == EnMtStatus.Accepted)
				msg.AppendFormat(string.Format("Message scheduled for send\n\nReference number: {0:### ### ### ##0}\nProvider Reference: {1}", sms.ReferenceId, status.ProviderReference));
			else
				msg.AppendFormat(string.Format("Message not sent\n\nError Code: {0}\nError Message: {1}", status.ErrorCode, status.ErrorMessage));

			MessageBox.Show(msg.ToString());
		}

		private void _editMessage_TextChanged(object sender, System.EventArgs e)
		{
			_lblCounter.Text = string.Format("{0} / 160", _editMessage.Text.Length);
		}
	}
}
