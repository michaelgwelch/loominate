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
    /// The base class of the EventArg classes used in BasicCell events
    /// </summary>
    public abstract class BasicCellEventArgs : System.EventArgs
    {
        int cursorPosition;
        int startSelection;
        int endSelection;

        public int CursorPosition 
        {
            set
            {
                cursorPosition = value;
            }
            get
            {
                return cursorPosition;
            }
        }
        
        public int StartSelection
        {
            set
            {
                startSelection = value;
            }
            
            get
            {
                return startSelection;
            }
        }
        
        public int EndSelection
        {
            set
            {
                startSelection = value;
            }
            
            get
            {
                return EndSelection;
            }
        }

        protected BasicCellEventArgs(int curPos, int startSel, 
                int endSel)
        {
            this.cursorPosition = curPos;
            this.startSelection = startSel;
            this.endSelection = endSel;
        }
                      
    }
}
