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
    /* typedef void (*CellModifyVerifyFunc) (BasicCell *cell,
                                      const char *add_str,
                                      int add_str_len,
                                      const char *new_value,
                                      int new_value_len,
                                      int *cursor_position,
                                      int *start_selection,
                                      int *end_selection);
    */
    
    public class ModifyVerifyArgs : BasicCellEventArgs
    {
        string addStr;
        string newValue;

        public ModifyVerifyArgs(string addStr, string newValue,
                int curPos, int startSel, int endSel) :
            base(curPos, startSel, endSel)
        {
            this.addStr = addStr;
            this.newValue = newValue;
        }
    }
}
