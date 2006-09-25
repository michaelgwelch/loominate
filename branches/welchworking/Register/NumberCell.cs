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


    public class NumberCell : BasicCell
    {
        long nextNum;
        bool nextNumSet;

        public NumberCell() : base()
        {
            this.nextNum = 0;
            this.nextNumSet = false;
        }

        // numcell in gnucash registers call backs for set_value
        // and modify_verify. That means it overrides these.


        public override string Value
        {
            get
            {
                return base.Value;
            }
            set
            {
                if ( ! this.nextNumSet)
                {
                    long number;
                    if (long.TryParse(value, out number))
                    {
                        this.nextNum = number+1;
                    }
                }
                base.Value = value;
            }
        }



        public override void ModifyVerify(string change, string newValue,
                                            ref int cursorPos, int startSel, int endSel)
        {
            bool accel = false, isNum;
            long number;
            char uc;

            // if we are deleting or entering > 1 char then just accept change
            if (change == null || change == String.Empty || change.Length > 1)
            {
                base.Value = newValue;
                return;
            }


            isNum = long.TryParse(this.Value, out number);
            if (isNum && (number < 0)) isNum = false;

            uc = change[0];
            switch (uc)
            {
                case '+':
                case '=':
                    number++;
                    accel = true;
                    break;

                case '_':
                case '-':
                    number--;
                    accel = true;
                    break;

                case '}':
                case ']':
                    number += 10;
                    accel = true;
                    break;

                case '{':
                case '[':
                    number -= 10;
                    accel = true;
                    break;
            }

            if (number < 0) number = 0;

            //  /* If there is already a non-number there, don't accelerate. */
            if (accel && !isNum && this.Value != String.Empty) accel = false;
            if (accel)
            {
                char[] buff = new char[128];
                if (!isNum) number = this.nextNum;

                if (number.ToString() == string.Empty)
                    return;

                base.Value = number.ToString();

                cursorPos = -1;

                return;
            }

            base.Value = newValue;
        }

        public bool SetLastNumber(string value)
        {
            long number;

            if (long.TryParse(value, out number))
            {
                this.nextNum = number + 1;
                this.nextNumSet = true;
                return true;
            }

            return false;
        }

    }
}
