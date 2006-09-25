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
    using System;
    using System.Collections.Generic;
    using System.Text;

    public delegate string ReconcileStringGetterDelegate(char flag);
    public delegate bool ReconcileCellConfirmDelegate(char oldFlag, object data);

    public class ReconcileCell : BasicCell
    {
        char flag;              /* The actual flag value */

        char[] validFlags;  /* The list of valid flags */
        char[] flagOrder;   /* Automatic flag selection order */
        char defaultFlag;       /* Default flag for unknown user input */

        ReconcileStringGetterDelegate getString;
        ReconcileCellConfirmDelegate confirmCb;
        object confirmData;

        static string str = String.Empty;
        public static string GetString(ReconcileCell cell, char flag)
        {
            if (cell.getString != null) return cell.getString(flag);
            return flag.ToString();
        }

        public ReconcileCell()
            : base()
        {
            this.Flag = '\0';
            this.validFlags = new char[0];
            this.flagOrder = new char[0];
        }
    

        public override bool EnterCell(ref int cursorPos, 
            ref int startSel, ref int endSel)
        {
            int index;
            char localFlag;
            if (this.confirmCb != null &&
                !(this.confirmCb(this.flag, this.confirmData))) return false;

            // Find the current flag in the list of flags
            index = Array.IndexOf<char>(flagOrder, this.flag);
            // if it's not there use default value
            if (index == -1) localFlag = this.defaultFlag;
            else
            {
                // It is in the list -- choose the next item in the list (wrapping
                // around as necessary.
                index++;
                if (index >= this.flagOrder.Length) localFlag = flagOrder[0];
                else localFlag = flagOrder[index];
            }
            Flag = localFlag;
            return false;
        }

        public override string Value
        {
            get
            {
                return base.Value;
            }
            set
            {
                char flag;

                if (value == null || value == string.Empty)
                {
                    this.flag = this.defaultFlag;
                    this.Value = String.Empty;
                    return;
                }

                flag = this.defaultFlag;
                if (Array.IndexOf(this.validFlags, value[0]) >= 0) flag = value[0];

                this.Flag = flag;
            }
        }

        public char Flag
        {
            get
            {
                return this.flag;
            }
            set
            {
                this.flag = value;
                string str = GetString(this, value);
                base.Value = str;
            }
        }

        public ReconcileStringGetterDelegate StringGetter
        {
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                this.getString = value;
            }
        }

        public ReconcileCellConfirmDelegate Confirm
        {
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                this.confirmCb = value;
            }
        }

        public object ConfirmData
        {
            set
            {
                this.confirmData = value;
            }
        }

        public char[] ValidFlags
        {
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                this.validFlags = value;
            }
        }

        public char DefaultFlag
        {
            set
            {
                this.defaultFlag = value;
            }
        }

        public char[] FlagOrder
        {
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                this.flagOrder = value;
            }
        }

    }
}
