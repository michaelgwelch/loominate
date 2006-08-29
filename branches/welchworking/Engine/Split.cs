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
            writer.WriteStartElement(ElementName, Namespaces.Transaction);
            GnuCashXml.WriteIdElement(writer, Namespaces.Split, this.id);
            if (memo != null) writer.WriteElementString("memo", Namespaces.Split, memo);
            if (action != null) writer.WriteElementString("action", Namespaces.Split, action);
            string localName = "reconciled-state";
            switch (this.reconcileState)
            {
                case ReconcileState.Reconciled:
                    writer.WriteElementString(localName, Namespaces.Split, "y");
                    break;
                case ReconcileState.NotReconciled:
                    writer.WriteElementString(localName, Namespaces.Split, "n");
                    break;
                case ReconcileState.Cleared:
                    writer.WriteElementString(localName, Namespaces.Split, "c");
                    break;
                default:
                    throw new Exception();
            }

            if (this.reconcileDate != null)
            {
                GnuCashXml.WriteDate(writer, "reconcile-date", Namespaces.Split,
                (DateTime)this.reconcileDate);
            }
            writer.WriteElementString("value", Namespaces.Split,
                FormatGnumeric(this.value, this.valueFraction));
            writer.WriteElementString("quantity", Namespaces.Split,
                FormatGnumeric(this.quantity, this.qtyFraction));
            GnuCashXml.WriteIdElement(writer, Namespaces.Split, accountId, "account");
            writer.WriteEndElement(); // </split>
        }
        public static Split ReadXml(XmlReader reader)
        {
            reader.ReadStartElement(ElementName, Namespaces.Transaction);

            Guid id = GnuCashXml.ReadIdElement(reader, Namespaces.Split);
            string memo = GnuCashXml.ReadOptionalElementString(reader, "memo", Namespaces.Split);
            string action = GnuCashXml.ReadOptionalElementString(reader, "action", Namespaces.Split);
            string reconcileString = reader.ReadElementString("reconciled-state", Namespaces.Split);
            ReconcileState reconcileState;
            switch (reconcileString)
            {
                case "y":
                    reconcileState = ReconcileState.Reconciled;
                    break;
                case "n":
                    reconcileState = ReconcileState.NotReconciled;
                    break;
                case "c":
                    reconcileState = ReconcileState.Cleared;
                    break;
                default:
                    throw new XmlException("Expected a valid reconcile state");
            }

            DateTime? reconcileDate = GnuCashXml.ReadDate(reader, "reconcile-date", Namespaces.Split);
            string value = GnuCashXml.ReadOptionalElementString(reader, "value", Namespaces.Split);
            string qty = reader.ReadElementString("quantity", Namespaces.Split);

            Guid accountId = GnuCashXml.ReadIdElement(reader, Namespaces.Split, "account");
            reader.ReadEndElement(); // </split>
            return new Split(id, memo, action, reconcileDate, reconcileState, ParseGnumeric(value), ParseGnumeric(qty), accountId);
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
