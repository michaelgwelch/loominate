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
using System.Drawing;
using System.Windows.Forms;

using Loominate.Gooey.Controls.Register;

namespace Loominate.Gooey.Controls
{
	/// <summary>
	/// Description of AccountPage.
	/// </summary>
	public class AccountPage : GroupBox
	{
	    private string name;
	    
		public AccountPage(string name)
		{
		    this.name = name;
		}
		
		protected override void InitLayout()
		{
		    SuspendLayout();

		    base.InitLayout();
		    
			this.Text = name;
			
			this.Dock = DockStyle.Fill;
			
			this.Controls.Add(new RegisterControl());
			
			ResumeLayout();
		}
	}
}
