/*
 * Created by SharpDevelop.
 * User: cedlerjo
 * Date: 7/28/2006
 * Time: 11:48 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Windows.Forms;

namespace Gooey.Controls
{
	/// <summary>
	/// Description of AccountOverview.
	/// </summary>
	public class AccountOverview : GroupBox
	{
		private IMainForm form;
		
		public AccountOverview(IMainForm form)
			: base()
		{
			this.form = form;
			
			this.Text = "Accounts";
			this.Dock = DockStyle.Top;
			
			AddAccounts();
		}
		
		private void AddAccounts()
		{
			AddAccountLink("My Checking");
			AddAccountLink("My Savings");
		}
		
		private void AddAccountLink(string name)
		{
			LinkLabel ll = new LinkLabel();
			ll.Text = name;
			
			ll.Height = ll.Font.Height + 2;
			ll.Top = 15 + (this.Controls.Count * ll.Height);
			ll.Left = 5;
			ll.Click += new EventHandler(AccountLinkClickHandler);
			
			this.Controls.Add(ll);
		}
		
		private void AccountLinkClickHandler(object sender, EventArgs e)
		{
			LinkLabel ll = (LinkLabel) sender;
			
			form.DisplayAccount(ll.Text);
		}
	}
}
