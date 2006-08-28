/*******************************************************************************
    Copyright 2006 Josh Edler
    
    This file is part of Loominate.

    Loominate is free software; you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation; either version 2 of the License, or
    (at your option) any later version.

    Loominate is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with Loominate; if not, write to the Free Software
    Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
*******************************************************************************/

using System;
using System.Windows.Forms;

namespace Loominate.Gooey.Controls
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
			this.Dock = DockStyle.Fill;
			
			AddAccounts();
		}
		
		private void AddAccounts()
		{
			AddAccountLink("My Checking");
			AddAccountLink("My Savings");
			
			form.DisplayAccount(this.Controls[0].Text);
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
