using System;
using System.Windows.Forms;
using Elapsus.Usms.DeskMessage.Forms;

namespace Elapsus.Usms.DeskMessage
{
	static class Program
	{
		internal static readonly Guid ApplicationId = new Guid("{5CBD875C-9939-4e02-BD92-EECF28192538}");

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
	}
}