/*
 * Created by SharpDevelop.
 * User: cedlerjo
 * Date: 7/28/2006
 * Time: 12:31 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Drawing;
using System.Windows.Forms;

namespace Gooey.Controls
{
	/// <summary>
	/// Description of AccountPage.
	/// </summary>
	public class AccountPage : TabPage
	{
		public AccountPage(string name, TabControl parent)
		{
			this.Name = name;
			this.Text = name;
			this.TabIndex = parent.Controls.Count + 1;
			this.BackColor = Color.AliceBlue;
			
			Panel panel = new Panel();
			panel.Dock = DockStyle.Fill;
			this.Controls.Add(panel);

			panel.Controls.Add(new CloseTabButton(this));
			
			Label hi = new Label();
			hi.Text = "Hi!";
			panel.Controls.Add(hi);
		}
	}
}
