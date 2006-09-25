/*******************************************************************************
    Copyright 2006 Michael Welch
    
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


namespace Loominate.Register
{
    /// <summary>
    /// </summary>
    public class  EnterArgs : BasicCellEventArgs
    {
        // An event handler should set this if the cell should allow
        // direct editing by the user.
        bool allowDirectEditing;    
        bool changeCursor;          // set to true to indicate that cursor and selection values are changed
        
        public EnterArgs(int curPos, int startSel,
                int endSel) : base(curPos, startSel, endSel)
        {
        }
                      
        public bool AllowDirectEditing 
        {
            get { return allowDirectEditing; }
            set { allowDirectEditing = value; }
        }
        
        public bool ChangeCursor
        {
            get { return this.changeCursor; }
            set { this.changeCursor = value; }
        }
        
    }
}
