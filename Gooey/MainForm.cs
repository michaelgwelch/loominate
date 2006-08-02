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
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Loominate.Gooey.Controls;

namespace Loominate.Gooey
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
			splitContainer.Panel2.Controls.Clear();
			splitContainer.Panel2.Controls.Add(new AccountPage(name));
		}
		#endregion
	}
}
