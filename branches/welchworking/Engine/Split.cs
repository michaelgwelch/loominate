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
    using System.Xml.Serialization;
    /// <summary>
    /// A ledger entry
    /// </summary>
    [XmlType(Namespace=Namespaces.Transaction)]
    [XmlRoot(Namespace=Namespaces.Transaction, ElementName="split")]
    public class Split
    {
        // QofInstance inst;
        //SplitId id;
//        Account acct;
//        Account orighAcct;
//        Lot lot;
//        Transaction parent;
//        Transaction origParent; //originating parent?
//        
//        
        string memo;
//        string action;
//        DateTime dateReconciled;
//        bool reconciled;
        ReconcileState reconcileState;
//        
//        // unsigned char gains
//        
//        Split gainsSplit;
        decimal value;      // the value expressed in a known commodity
        string valueString;
        decimal amount;     // the ammount of the commodity involved
        string amountString;



        [XmlElement(Namespace=Namespaces.Split, ElementName="memo")]
        public String Memo
        {
            get
            {
                return memo;
            }
            set
            {
                memo = value;
            }
        }

        [XmlElement(Namespace = Namespaces.Split, ElementName = "reconciled-state")]
        public ReconcileState ReconcileState
        {
            get
            {
                return reconcileState;
            }
            set
            {
                reconcileState = value;
            }
        }

        [XmlIgnore]
        public decimal Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }

        /// <summary>
        /// Used for serialization. Not for external use.
        /// </summary>
        [XmlElement(Namespace=Namespaces.Split, ElementName="value")]
        public String ValueString
        {
            get
            {
                return valueString;
            }
            set
            {
                valueString = value;
                string[] nums = valueString.Split('/');
                if (nums.Length != 2) throw new ApplicationException("Trouble parsing a split value");
                else this.value = decimal.Parse(nums[0]) / decimal.Parse(nums[1]);
            }
        }

        [XmlIgnore]
        public decimal Amount
        {
            get
            {
                return amount;
            }
            set
            {
                amount = value;
            }
        }

        [XmlIgnore]
        public string AmountString
        {
            get
            {
                return amountString;
            }
            set
            {
                amountString = value;
                string[] nums = value.Split('/');
                if (nums.Length != 2) throw new ApplicationException("Trouble parsing a split amount");
                amount = decimal.Parse(nums[0]) / decimal.Parse(nums[1]);
            }
        }

    }
}
