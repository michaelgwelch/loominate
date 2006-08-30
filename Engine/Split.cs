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


namespace Loominate.Engine
{
    using System;
    using System.Diagnostics;
    using System.Xml;
    using System.Xml.Serialization;

    /// <summary>
    /// A ledger entry
    /// </summary>
    public class Split
    {
        public const string ElementName = "split";


        Guid id;
        string memo;
        string action;
        DateTime? reconcileDate;
        ReconcileState reconcileState;
        decimal value;
        int valueFraction;

        decimal quantity;
        int qtyFraction;

        Guid accountId;

        public Split(Guid id, string memo, string action,
            DateTime? reconcileDate, ReconcileState reconcileState,
            Pair<decimal, int> value, Pair<decimal, int> quantity,
            Guid accountId)
        {
            this.id = id;
            this.memo = memo;
            this.action = action;
            this.reconcileDate = reconcileDate;
            this.reconcileState = reconcileState;
            this.value = value.First;
            this.valueFraction = value.Second;

            this.quantity = quantity.First;
            this.qtyFraction = quantity.Second;

            this.accountId = accountId;
        }

        public Guid Id
        {
            get
            {
                return this.id;
            }
        }

        public ReconcileState ReconcileState
        {
            get
            {
                return this.reconcileState;
            }
        }

        public decimal Value
        {
            get
            {
                return this.value;
            }
        }

        public decimal Quantity
        {
            get
            {
                return this.quantity;
            }
        }

        public Guid AccountId
        {
            get
            {
                return this.accountId;
            }
        }


        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement(ElementName, NameSpace.Transaction);
            GnuCashXml.WriteIdElement(writer, NameSpace.Split, this.id);
            if (memo != null) writer.WriteElementString("memo", NameSpace.Split, memo);
            if (action != null) writer.WriteElementString("action", NameSpace.Split, action);
            string localName = "reconciled-state";
            switch (this.reconcileState)
            {
                case ReconcileState.Reconciled:
                    writer.WriteElementString(localName, NameSpace.Split, "y");
                    break;
                case ReconcileState.NotReconciled:
                    writer.WriteElementString(localName, NameSpace.Split, "n");
                    break;
                case ReconcileState.Cleared:
                    writer.WriteElementString(localName, NameSpace.Split, "c");
                    break;
                default:
                    throw new Exception();
            }

            if (this.reconcileDate != null)
            {
                GnuCashXml.WriteDate(writer, "reconcile-date", NameSpace.Split,
                (DateTime)this.reconcileDate);
            }
            writer.WriteElementString("value", NameSpace.Split,
                FormatGnumeric(this.value, this.valueFraction));
            writer.WriteElementString("quantity", NameSpace.Split,
                FormatGnumeric(this.quantity, this.qtyFraction));
            GnuCashXml.WriteIdElement(writer, NameSpace.Split, accountId, "account");
            writer.WriteEndElement(); // </split>
        }

        /// <summary>
        /// Creates an instance of <see cref="Split"/> from the data
        /// at the current location in reader.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        internal static Split ReadXml(XmlGnuCashReader reader)
        {
            reader.ReadStartElement(ElementName, NameSpace.Transaction);

            using (DefaultNameSpace.Set(NameSpace.Split))
            {
                Guid id = reader.ReadIdElement();
                string memo = reader.ReadOptionalString("memo");
                string action = reader.ReadOptionalString("action");
                ReconcileState reconcileState = reader.ReadReconcileState();

                DateTime? reconcileDate = reader.ReadOptionalDate("reconcile-date");
                string value = reader.ReadOptionalString("value");
                string qty = reader.ReadString("quantity");

                Guid accountId = reader.ReadIdElement("account");

                reader.ReadEndElement(); // </split>
                return new Split(id, memo, action, reconcileDate, reconcileState, ParseGnumeric(value), ParseGnumeric(qty), accountId);
            }
        }

        /// <summary>
        /// Parses the numeric strings used by gnucash into the 
        /// value (stored in First) and denominator (stored in Second).
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static Pair<decimal, int> ParseGnumeric(string str)
        {
            string[] nums = str.Split('/');
            decimal numerator = decimal.Parse(nums[0]);
            int denominator = int.Parse(nums[1]);
            decimal value = numerator / denominator;

            return new Pair<decimal, int>(value, denominator);

        }

        private static string FormatGnumeric(decimal value, int fraction)
        {
            return ((int)(value * fraction)).ToString() + "/" + fraction.ToString();
        }
    }
}
