namespace Elapsus.Usms.DeskMessage.Forms
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this._listProviders = new System.Windows.Forms.ComboBox();
			this._bindProviders = new System.Windows.Forms.BindingSource(this.components);
			this._lblProvider = new System.Windows.Forms.Label();
			this._listSenders = new System.Windows.Forms.ComboBox();
			this._lblSender = new System.Windows.Forms.Label();
			this._listRecipients = new System.Windows.Forms.ComboBox();
			this._lblRecipient = new System.Windows.Forms.Label();
			this._btnSend = new System.Windows.Forms.Button();
			this._editMessage = new System.Windows.Forms.TextBox();
			this._lblMesssage = new System.Windows.Forms.Label();
			this._lblCounter = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this._bindProviders)).BeginInit();
			this.SuspendLayout();
			// 
			// _listProviders
			// 
			this._listProviders.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._listProviders.FormattingEnabled = true;
			this._listProviders.Location = new System.Drawing.Point(107, 30);
			this._listProviders.Name = "_listProviders";
			this._listProviders.Size = new System.Drawing.Size(234, 21);
			this._listProviders.TabIndex = 0;
			// 
			// _lblProvider
			// 
			this._lblProvider.AutoSize = true;
			this._lblProvider.Location = new System.Drawing.Point(34, 33);
			this._lblProvider.Name = "_lblProvider";
			this._lblProvider.Size = new System.Drawing.Size(46, 13);
			this._lblProvider.TabIndex = 1;
			this._lblProvider.Text = "Provider";
			// 
			// _listSenders
			// 
			this._listSenders.FormattingEnabled = true;
			this._listSenders.Items.AddRange(new object[] {
            "0048 668 405 761"});
			this._listSenders.Location = new System.Drawing.Point(107, 57);
			this._listSenders.Name = "_listSenders";
			this._listSenders.Size = new System.Drawing.Size(179, 21);
			this._listSenders.TabIndex = 2;
			// 
			// _lblSender
			// 
			this._lblSender.AutoSize = true;
			this._lblSender.Location = new System.Drawing.Point(34, 60);
			this._lblSender.Name = "_lblSender";
			this._lblSender.Size = new System.Drawing.Size(41, 13);
			this._lblSender.TabIndex = 3;
			this._lblSender.Text = "Sender";
			// 
			// _listRecipients
			// 
			this._listRecipients.FormattingEnabled = true;
			this._listRecipients.Items.AddRange(new object[] {
            "0048 668 405 761"});
			this._listRecipients.Location = new System.Drawing.Point(107, 84);
			this._listRecipients.Name = "_listRecipients";
			this._listRecipients.Size = new System.Drawing.Size(179, 21);
			this._listRecipients.TabIndex = 4;
			// 
			// _lblRecipient
			// 
			this._lblRecipient.AutoSize = true;
			this._lblRecipient.Location = new System.Drawing.Point(34, 87);
			this._lblRecipient.Name = "_lblRecipient";
			this._lblRecipient.Size = new System.Drawing.Size(52, 13);
			this._lblRecipient.TabIndex = 5;
			this._lblRecipient.Text = "Recipient";
			// 
			// _btnSend
			// 
			this._btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._btnSend.Location = new System.Drawing.Point(297, 243);
			this._btnSend.Name = "_btnSend";
			this._btnSend.Size = new System.Drawing.Size(75, 23);
			this._btnSend.TabIndex = 6;
			this._btnSend.Text = "Send";
			this._btnSend.UseVisualStyleBackColor = true;
			this._btnSend.Click += new System.EventHandler(this._btnSend_Click);
			// 
			// _editMessage
			// 
			this._editMessage.AcceptsReturn = true;
			this._editMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
									| System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this._editMessage.Location = new System.Drawing.Point(107, 111);
			this._editMessage.Multiline = true;
			this._editMessage.Name = "_editMessage";
			this._editMessage.Size = new System.Drawing.Size(265, 126);
			this._editMessage.TabIndex = 7;
			this._editMessage.TextChanged += new System.EventHandler(this._editMessage_TextChanged);
			// 
			// _lblMesssage
			// 
			this._lblMesssage.AutoSize = true;
			this._lblMesssage.Location = new System.Drawing.Point(34, 114);
			this._lblMesssage.Name = "_lblMesssage";
			this._lblMesssage.Size = new System.Drawing.Size(50, 13);
			this._lblMesssage.TabIndex = 8;
			this._lblMesssage.Text = "Message";
			// 
			// _lblCounter
			// 
			this._lblCounter.AutoSize = true;
			this._lblCounter.Location = new System.Drawing.Point(107, 243);
			this._lblCounter.Name = "_lblCounter";
			this._lblCounter.Size = new System.Drawing.Size(42, 13);
			this._lblCounter.TabIndex = 9;
			this._lblCounter.Text = "0 / 160";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(384, 278);
			this.Controls.Add(this._lblCounter);
			this.Controls.Add(this._lblMesssage);
			this.Controls.Add(this._editMessage);
			this.Controls.Add(this._btnSend);
			this.Controls.Add(this._lblRecipient);
			this.Controls.Add(this._listRecipients);
			this.Controls.Add(this._lblSender);
			this.Controls.Add(this._listSenders);
			this.Controls.Add(this._lblProvider);
			this.Controls.Add(this._listProviders);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "MainForm";
			this.Text = "DeskMessage";
			this.Load += new System.EventHandler(this.Main_Load);
			((System.ComponentModel.ISupportInitialize)(this._bindProviders)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox _listProviders;
		private System.Windows.Forms.Label _lblProvider;
		private System.Windows.Forms.ComboBox _listSenders;
		private System.Windows.Forms.Label _lblSender;
		private System.Windows.Forms.ComboBox _listRecipients;
		private System.Windows.Forms.Label _lblRecipient;
		private System.Windows.Forms.BindingSource _bindProviders;
		private System.Windows.Forms.Button _btnSend;
		private System.Windows.Forms.TextBox _editMessage;
		private System.Windows.Forms.Label _lblMesssage;
		private System.Windows.Forms.Label _lblCounter;
	}
}