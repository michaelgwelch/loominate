/*
 * Created by SharpDevelop.
 * User: cedlerjo
 * Date: 7/28/2006
 * Time: 12:22 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Drawing;
using System.Windows.Forms;

namespace Gooey.Controls
{
	/// <summary>
	/// Description of CloseTabButton.
	/// </summary>
	public class CloseTabButton : Button
	{
		private TabPage page;
		
		public CloseTabButton(TabPage page)
		{
			this.page = page;

			this.Text = "X";
			Graphics g = Graphics.FromHwnd(this.Handle);
			this.Width = (int) g.MeasureString(this.Text, this.Font).Width + 10;
			
			page.SizeChanged += new EventHandler(PageSizeChangedHandler);
			this.Click += new EventHandler(CloseTabClickHandler);
		}
		
		private void CloseTabClickHandler(object sender, EventArgs e)
		{
			page.Parent.Controls.Remove(page);
		}

		private void PageSizeChangedHandler(object sender, EventArgs e)
		{
			this.Left = page.Width - this.Width;
			this.Top = 0;
		}		
	}
}
