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
    using System.Globalization;
    public class PriceCell : BasicCell
    {
        decimal amount;                 /* the amount associated with this cell */

        int fraction;                   /* fraction used for rounding, if 0 no rounding */

        bool blankZero;                 /* controls printing of zero values */

        GNCPrintAmountInfo printInfo;  /* amount printing context */

        bool needToParse;             /* internal */

        public PriceCell()
            : base()
        {
            this.blankZero = true;
            this.printInfo = GNCPrintAmountInfo.Default;
        }

        public override bool EnterCell(ref int cursorPos,
            ref int startSel, ref int endSel)
        {
            cursorPos = -1;
            startSel = 0;
            endSel = -1;
            return true;
        }

        public override void ModifyVerify(string change, string newValue,
            ref int cursorPos, int startSel, int endSel)
        {
            string tokens = "+-*/=()_";

            if (change == null)
            {
                // accept the newval string if the action was delete
                base.Value = newValue;
                this.needToParse = true;
                return;
            }

            CultureInfo cultureInfo = CultureInfo.CurrentCulture;
            NumberFormatInfo numberInfo = cultureInfo.NumberFormat;

            string decimalPoint;
            string groupSeperator;
            if (this.printInfo.monetary)
            {
                decimalPoint = numberInfo.CurrencyDecimalSeparator;
                groupSeperator = numberInfo.CurrencyGroupSeparator;
            }
            else
            {
                decimalPoint = numberInfo.NumberDecimalSeparator;
                groupSeperator = numberInfo.NumberGroupSeparator;
            }

            // make sure we have valid characters
            for (int i = 0; i < change.Length; i++)
            {
                char c = change[i];
                if (!char.IsDigit(c) &&
                    !char.IsWhiteSpace(c) &&
                    !char.IsLetter(c) &&
                    (decimalPoint != c.ToString()) &&
                    (groupSeperator != c.ToString()) &&
                    (tokens.IndexOf(c) == -1)) return;
            }

            base.Value = newValue;
            this.needToParse = true;
        }

        static int Parse(PriceCell cell, bool updateValue)
        {
            if (!cell.needToParse) return -1;

            string oldValue = cell.Value;
            if (oldValue == null) oldValue = String.Empty;

            decimal newAmount;
            if (oldValue.Trim().Length == 0) cell.Amount = decimal.Zero;
            else if (decimal.TryParse(oldValue, out newAmount))
            {
                if (cell.fraction > 0)
                    newAmount = Math.Round(newAmount, cell.fraction);
                cell.Amount = newAmount;
            }
            else
            {
                return 0; // we should be returning the location of the
                // error but we don't have a function to determine that yet.
                // So just flag the first character
            }


            if (!updateValue) return -1;

            string newValue = FormatCellAmount(cell);

            if (oldValue.Equals(newValue))
                return -1;

            cell.Value = newValue;
            return -1;

        }

        public override void LeaveCell()
        {
            if (Parse(this, true) != -1) throw new Exception(String.Format("Could not parse {0} when leaving cell", this.Value));
        }

        public static string FormatCellAmount(PriceCell cell)
        {
            return cell.amount.ToString();
        }


        public decimal Amount
        {
            get
            {
                Parse(this, false);
                return this.amount;
            }
            set
            {
                if (this.fraction > 0)
                    value = Math.Round(value, this.fraction);

                this.amount = value;
                string strValue = FormatCellAmount(this);
                this.needToParse = false;
                if (strValue.Equals(this.Value)) return;

                base.Value = strValue;
            }
        }
    }



    public class GNCPrintAmountInfo
    {
        //const gnc_commodity *commodity;  /* may be NULL */

        public byte max_decimal_places;
        public byte min_decimal_places;

        public bool use_separators; /* Print thousands separators */
        public bool use_symbol;     /* Print currency symbol */
        public bool use_locale;     /* Use locale for some positioning */
        public bool monetary;       /* Is a monetary quantity */
        public bool force_fit;      /* Don't print more than max_dp places */
        public bool round;          /* Round at max_dp instead of truncating */

        public GNCPrintAmountInfo(byte maxDecimal, byte minDecimal,
            bool useSeperators, bool useSymbol, bool useLocale,
            bool monetary, bool forceFit, bool round)
        {
            NumberFormatInfo numberInfo = CultureInfo.CurrentCulture.NumberFormat;

            this.max_decimal_places = (byte) numberInfo.NumberDecimalDigits;
            this.min_decimal_places = (byte) numberInfo.NumberDecimalDigits;
            this.use_separators = useSeperators;
            this.use_symbol = useSymbol;
            this.use_locale = useLocale;
            this.monetary = monetary;
            this.force_fit = forceFit;
            this.round = round;
        }
        //        static GNCPrintAmountInfo info;
        //static gboolean got_it = FALSE;
        //struct lconv *lc;

        ///* These must be updated each time. */
        //info.use_symbol = use_symbol ? 1 : 0;
        //info.commodity = gnc_default_currency ();

        //if (got_it)
        //  return info;

        //lc = gnc_localeconv ();

        //info.max_decimal_places = lc->frac_digits;
        //info.min_decimal_places = lc->frac_digits;

        //info.use_separators = 1;
        //info.use_locale = 1;
        //info.monetary = 1;
        //info.force_fit = 0;
        //info.round = 0;

        //got_it = TRUE;

        //return info;

        public static readonly GNCPrintAmountInfo Default =
            new GNCPrintAmountInfo(4, 2, true, false, true, true, false, false);
    }
}
