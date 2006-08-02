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

namespace Gooey.Controls.Register
{
	/// <summary>
	/// Description of RegisterRow.
	/// </summary>
	public class RegisterRow : Control
	{
	    private int i = 0;
	    private RegisterControl register;
	    
	    private const int DATE_COL = 0;
	    
		public RegisterRow(int i, RegisterControl register)
		{
		    this.i = i;
		    this.register = register;
		    
		    //++register.RowCount;
		    
		    //register.Controls.Add(new DateRegisterControl(), DATE_COL, i);
		}
	}
}
