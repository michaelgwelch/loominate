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
    using System.Collections.Generic;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    using Slots = System.Collections.Generic.Dictionary<string, Pair<string, object>>;
    using CommodityDictionary = System.Collections.Generic.Dictionary<string, Commodity>;
    using AccountList = System.Collections.Generic.List<Account>;
    using TransactionList = System.Collections.Generic.List<Transaction>;

    public class Book
    {
        private const string VersionXml = "2.0.0";
        private const string ElementName = "book";

        private Guid id;
        private Slots slots;
        private CommodityDictionary commodities;
        private AccountList accounts;
        private TransactionList transactions;

        #region Original Counts
        int? comms;
        int accts;
        int trans;
        int? strans;
        int? budgts;
        int? custs;
        int? emps;
        int? terms;
        int? invcs;
        int? entrs;
        #endregion
        public Book(Guid id, Slots slots,
            CommodityDictionary commodities,
            AccountList accounts, TransactionList transactions,
            int? numOfCommodities, int numOfAccounts, int numOfTransactions, 
            int? numOfScheduledTrans, int? numOfBudgets, int? numOfCustomers,
            int? numOfEmployees, int? numOfBillTerms, int? numOfInvoices, 
            int? numOfEntries)
        {
            this.id = id;
            this.slots = slots;
            this.commodities = commodities;
            this.accounts = accounts;
            this.transactions = transactions;

            this.comms = numOfCommodities;
            this.accts = numOfAccounts;
            this.trans = numOfTransactions;
            this.strans = numOfScheduledTrans;
            this.budgts = numOfBudgets;
            this.custs = numOfCustomers;
            this.emps = numOfEmployees;
            this.terms = numOfBillTerms;
            this.invcs = numOfInvoices;
            this.entrs = numOfEntries;
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement(ElementName, NameSpace.GnuCash);
            writer.WriteAttributeString("version", "", VersionXml);
            GnuCashXml.WriteIdElement(writer, NameSpace.Book, this.id);
            GnuCashXml.WriteSlots(writer, slots, "slots", NameSpace.Book, false);
            GnuCashXml.WriteCountData(writer, NameSpace.GnuCash,
                CountDataType.Commodity, comms);
            GnuCashXml.WriteCountData(writer, NameSpace.GnuCash,
                CountDataType.Account, accts);
            GnuCashXml.WriteCountData(writer, NameSpace.GnuCash,
                CountDataType.Transaction, trans);
            GnuCashXml.WriteCountData(writer, NameSpace.GnuCash,
                CountDataType.ScheduledTransaction, strans);
            GnuCashXml.WriteCountData(writer, NameSpace.GnuCash,
                CountDataType.Budget, budgts);
            GnuCashXml.WriteCountData(writer, NameSpace.GnuCash,
                CountDataType.Customer, custs);
            GnuCashXml.WriteCountData(writer, NameSpace.GnuCash,
                CountDataType.Employee, emps);
            GnuCashXml.WriteCountData(writer, NameSpace.GnuCash,
                CountDataType.BillTerm, terms);
            GnuCashXml.WriteCountData(writer, NameSpace.GnuCash,
                CountDataType.Invoice, invcs);
            GnuCashXml.WriteCountData(writer, NameSpace.GnuCash,
                CountDataType.Entry, entrs);
            foreach (KeyValuePair<string, Commodity> kvp in commodities) kvp.Value.WriteXml(writer);
            foreach (Account account in accounts) account.WriteXml(writer);
            foreach (Transaction transaction in transactions) transaction.WriteXml(writer);
            writer.WriteEndElement();
        }

        internal static Book ReadXml(XmlGnuCashReader reader)
        {
            if ( ! reader.IsStartElement(ElementName, NameSpace.GnuCash))
                throw new XmlException("Expected the start of a book element");

            if (reader.GetAttribute("version") != VersionXml)
                throw new XmlException("Expected book element to be at version " + VersionXml);

            reader.ReadStartElement(ElementName, NameSpace.GnuCash);

            Guid id = reader.ReadIdElement(NameSpace.Book);
            Slots slots = reader.ReadOptionalSlots("slots", NameSpace.Book);


            // Note: All of the following except numOfAccounts and numOfTransactions is required.
            // Notice the use of ReadCountData for them instead of ReadCountDataOptional.
            int? numOfCommodities = reader.ReadOptionalCountData(CountDataType.Commodity);
            int numOfAccounts = reader.ReadCountData(CountDataType.Account);
            int numOfTransactions = reader.ReadCountData(CountDataType.Transaction);
            int? numOfScheduledTrans = reader.ReadOptionalCountData(CountDataType.ScheduledTransaction);
            int? numOfBudgets = reader.ReadOptionalCountData(CountDataType.Budget);
            int? numOfCustomers = reader.ReadOptionalCountData(CountDataType.Customer);
            int? numOfEmployees = reader.ReadOptionalCountData(CountDataType.Employee);
            int? numOfBillTerms = reader.ReadOptionalCountData(CountDataType.BillTerm);
            int? numOfInvoices = reader.ReadOptionalCountData(CountDataType.Invoice);
            int? numOfEntries = reader.ReadOptionalCountData(CountDataType.Entry);

            CommodityDictionary commodities = new CommodityDictionary();
            ReadCommodities(reader, commodities);

            AccountList accounts = new AccountList(numOfAccounts);
            ReadAccounts(reader, accounts, commodities);

            TransactionList transactions = new TransactionList(numOfTransactions);
            ReadTransactions(reader, transactions, commodities);

            reader.ReadEndElement();
            return new Book(id, slots, commodities, accounts, transactions,
                numOfCommodities, numOfAccounts, numOfTransactions, numOfScheduledTrans,
                numOfBudgets, numOfCustomers, numOfEmployees, numOfBillTerms,
                numOfInvoices, numOfEntries);


        }

        private static void ReadCommodities(XmlGnuCashReader reader, CommodityDictionary commodities)
        {
            while (reader.IsStartElement(Commodity.ElementName, NameSpace.GnuCash))
            {
                Commodity c = Commodity.ReadXml(reader);
                commodities[c.UniqueName] = c;
            }
        }

        private static void ReadAccounts(XmlGnuCashReader reader, AccountList accounts,
            CommodityDictionary commodities)
        {
            while (reader.IsStartElement(Account.ElementName, NameSpace.GnuCash))
            {
                Account a = Account.ReadXml(reader, commodities);
                accounts.Add(a);
            }
        }

        private static void ReadTransactions(XmlGnuCashReader reader, TransactionList transactions,
            CommodityDictionary commodities)
        {
            while (reader.IsStartElement(Transaction.ElementName, NameSpace.GnuCash))
            {
                transactions.Add(Transaction.ReadXml(reader, commodities));
            }
        }
        


    }
}
