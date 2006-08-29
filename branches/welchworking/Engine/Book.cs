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
            writer.WriteStartElement(ElementName, Namespaces.GnuCash);
            writer.WriteAttributeString("version", "", VersionXml);
            GnuCashXml.WriteIdElement(writer, Namespaces.Book, this.id);
            GnuCashXml.WriteSlots(writer, slots, "slots", Namespaces.Book, false);
            GnuCashXml.WriteCountData(writer, Namespaces.GnuCash,
                CountDataType.Commodity, comms);
            GnuCashXml.WriteCountData(writer, Namespaces.GnuCash,
                CountDataType.Account, accts);
            GnuCashXml.WriteCountData(writer, Namespaces.GnuCash,
                CountDataType.Transaction, trans);
            GnuCashXml.WriteCountData(writer, Namespaces.GnuCash,
                CountDataType.ScheduledTransaction, strans);
            GnuCashXml.WriteCountData(writer, Namespaces.GnuCash,
                CountDataType.Budget, budgts);
            GnuCashXml.WriteCountData(writer, Namespaces.GnuCash,
                CountDataType.Customer, custs);
            GnuCashXml.WriteCountData(writer, Namespaces.GnuCash,
                CountDataType.Employee, emps);
            GnuCashXml.WriteCountData(writer, Namespaces.GnuCash,
                CountDataType.BillTerm, terms);
            GnuCashXml.WriteCountData(writer, Namespaces.GnuCash,
                CountDataType.Invoice, invcs);
            GnuCashXml.WriteCountData(writer, Namespaces.GnuCash,
                CountDataType.Entry, entrs);
            foreach (KeyValuePair<string, Commodity> kvp in commodities) kvp.Value.WriteXml(writer);
            foreach (Account account in accounts) account.WriteXml(writer);
            foreach (Transaction transaction in transactions) transaction.WriteXml(writer);
            writer.WriteEndElement();
        }

        public static Book ReadXml(XmlReader reader)
        {
            reader.MoveToContent();
            if ( ! reader.IsStartElement(ElementName, Namespaces.GnuCash))
                throw new XmlException("Expected the start of a book element");

            if (reader.GetAttribute("version") != VersionXml)
                throw new XmlException("Expected book element to be at version " + VersionXml);

            reader.ReadStartElement(ElementName, Namespaces.GnuCash);

            Guid id = GnuCashXml.ReadIdElement(reader, Namespaces.Book);

            Slots slots = null;
            if (reader.IsStartElement("slots", Namespaces.Book)) {
                slots = GnuCashXml.ReadSlots(reader, Namespaces.Book, "slots");
            }

            // Note: All of the following except numOfAccounts and numOfTransactions is required.
            // Notice the use of ReadCountData for them instead of ReadCountDataOptional.
            int? numOfCommodities = GnuCashXml.ReadCountDataOptional(reader, CountDataType.Commodity);
            int numOfAccounts = GnuCashXml.ReadCountData(reader, CountDataType.Account);
            int numOfTransactions = GnuCashXml.ReadCountData(reader, CountDataType.Transaction);
            int? numOfScheduledTrans = GnuCashXml.ReadCountDataOptional(reader, CountDataType.ScheduledTransaction);
            int? numOfBudgets = GnuCashXml.ReadCountDataOptional(reader, CountDataType.Budget);
            int? numOfCustomers = GnuCashXml.ReadCountDataOptional(reader, CountDataType.Customer);
            int? numOfEmployees = GnuCashXml.ReadCountDataOptional(reader, CountDataType.Employee);
            int? numOfBillTerms = GnuCashXml.ReadCountDataOptional(reader, CountDataType.BillTerm);
            int? numOfInvoices = GnuCashXml.ReadCountDataOptional(reader, CountDataType.Invoice);
            int? numOfEntries = GnuCashXml.ReadCountDataOptional(reader, CountDataType.Entry);

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

        private static void ReadCommodities(XmlReader reader, CommodityDictionary commodities)
        {
            while (reader.IsStartElement(Commodity.ElementName, Namespaces.GnuCash))
            {
                Commodity c = Commodity.ReadXml(reader);
                commodities[c.UniqueName] = c;
            }
        }

        private static void ReadAccounts(XmlReader reader, AccountList accounts,
            CommodityDictionary commodities)
        {
            while (reader.IsStartElement(Account.ElementName, Namespaces.GnuCash))
            {
                Account a = Account.ReadXml(reader, commodities);
                accounts.Add(a);
            }
        }

        private static void ReadTransactions(XmlReader reader, TransactionList transactions,
            CommodityDictionary commodities)
        {
            while (reader.IsStartElement(Transaction.ElementName, Namespaces.GnuCash))
            {
                transactions.Add(Transaction.ReadXml(reader, commodities));
            }
        }
        


    }
}
