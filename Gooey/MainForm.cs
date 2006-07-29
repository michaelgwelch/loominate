/*
 * Created by SharpDevelop.
 * User: cedlerjo
 * Date: 7/28/2006
 * Time: 9:56 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Gooey.Controls;

namespace Gooey
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : IMainForm
	{
		[STAThread]
		public static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
		
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			LoadData();
		}
		
		private void LoadData()
		{
			LoadAccounts();
		}
		
		private void LoadAccounts()
		{
			splitContainer.Panel1.Controls.Add(new AccountOverview(this));
		}
		
		private void SafeExit()
		{
			Application.Exit();
		}
		
		private void ExitToolStripMenuItemClick(object sender, System.EventArgs e)
		{
			SafeExit();
		}
		
		#region IMainForm methods
		public void DisplayAccount(string name)
		{
			TabPage page = null;
			
			if (mainTabControl.Controls.ContainsKey(name))
			{
				page = (TabPage) mainTabControl.Controls[name];
			}
			else
			{
				page = new AccountPage(name, mainTabControl);
				mainTabControl.Controls.Add(page);
			}

			mainTabControl.SelectedIndex = page.TabIndex - 1;
		}
		#endregion
	}
}
