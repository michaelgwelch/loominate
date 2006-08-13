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
        ReconcileState reconcileState;
        decimal value;
        decimal quantity;
        Guid accountId;

        public Split(Guid id, ReconcileState reconcileState, decimal value, decimal quantity,
            Guid accountId)
        {
            this.id = id;
            this.reconcileState = reconcileState;
            this.value = value;
            this.quantity = quantity;
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

        public static Split ReadXml(XmlReader reader)
        {
            reader.ReadStartElement(ElementName, Namespaces.Transaction);

            Guid id = GnuCashXml.ReadIdElement(reader, Namespaces.Split);
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

            string value = GnuCashXml.ReadOptionalElementString(reader, "value", Namespaces.Split);
            string qty = reader.ReadElementString("quantity", Namespaces.Split);

            Guid accountId = GnuCashXml.ReadIdElement(reader, Namespaces.Split, "account");

            return new Split(id, reconcileState, ParseGnumeric(value), ParseGnumeric(qty), accountId);
        }

        private static decimal ParseGnumeric(string value)
        {
            string[] nums = value.Split('/');
            return decimal.Parse(nums[0]) / decimal.Parse(nums[1]);

        }
    }
}
