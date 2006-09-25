using System;
using System.Collections.Generic;
using System.Text;

namespace Loominate.Register
{
    class CheckBoxCell : BasicCell
    {
        private bool isChecked;

        public CheckBoxCell()
            : base()
        {
            Checked = false;
        }

        public override string Value
        {
            get
            {
                return base.Value;
            }
            set
            {
                SetChecked(StringToBoolean(value), value);
            }
        }

        private void SetChecked(bool isChecked, string value)
        {
            this.isChecked = isChecked;
            base.Value = value;
        }



        public bool Checked
        {
            get
            {
                return this.isChecked;
            }
            set
            {
                SetChecked(value, BooleanToString(value));
            }
        }

        public override bool EnterCell(ref int cursorPos, 
            ref int startSel, ref int endSel)
        {
            Checked = !this.isChecked;
            return false;
        }

        private static bool StringToBoolean(string value)
        {
            if (value != null && value == "X")
                return true;
            return false;
        }

        private static string BooleanToString(bool isChecked)
        {
            return isChecked ? "X" : " ";
        }

    }
}
