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

    public class Book
    {
        private const string VersionXml = "2.0.0";
        private const string ElementName = "book";

        private Guid id;
        private Dictionary<string, Commodity> commodities;
        private List<Account> accounts;
        private List<Transaction> transactions;

        public Book(Guid id, Dictionary<string, Commodity> commodities,
            List<Account> accounts, List<Transaction> transactions)
        {
            this.id = id;
            this.commodities = commodities;
            this.accounts = accounts;
            this.transactions = transactions;
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement(ElementName, Namespaces.GnuCash);
            writer.WriteAttributeString("version", "", VersionXml);
            GnuCashXml.WriteIdElement(writer, Namespaces.Book, this.id);
            GnuCashXml.WriteCountData(writer, Namespaces.GnuCash,
                CountDataType.Account, accounts.Count);
            GnuCashXml.WriteCountData(writer, Namespaces.GnuCash,
                CountDataType.Transaction, transactions.Count);
            foreach (KeyValuePair<string, Commodity> kvp in commodities) kvp.Value.WriteXml(writer);
            foreach (Account account in accounts) account.WriteXml(writer);
            //foreach (Transaction transaction in transactions) transaction.WriteXml(writer);
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

            Dictionary<string, string> slots = null;
            if (reader.IsStartElement("slots", Namespaces.Book)) {
                slots = GnuCashXml.ReadSlots(reader, Namespaces.Book);
            }

            // Note: All of the following except numOfAccounts and numOfTransactions is required.
            // Notice the use of ReadCountData for them instead of ReadCountDataOptional.
            int numOfCommodities = GnuCashXml.ReadCountDataOptional(reader, CountDataType.Commodity);
            int numOfAccounts = GnuCashXml.ReadCountData(reader, CountDataType.Account);
            int numOfTransactions = GnuCashXml.ReadCountData(reader, CountDataType.Transaction);
            int numOfScheduledTrans = GnuCashXml.ReadCountDataOptional(reader, CountDataType.ScheduledTransaction);
            int numOfBudgets = GnuCashXml.ReadCountDataOptional(reader, CountDataType.Budget);
            int numOfCustomers = GnuCashXml.ReadCountDataOptional(reader, CountDataType.Customer);
            int numOfEmployees = GnuCashXml.ReadCountDataOptional(reader, CountDataType.Employee);
            int numOfBillTerms = GnuCashXml.ReadCountDataOptional(reader, CountDataType.BillTerm);
            int numOfInvoices = GnuCashXml.ReadCountDataOptional(reader, CountDataType.Invoice);
            int numOfEntries = GnuCashXml.ReadCountDataOptional(reader, CountDataType.Entry);

            Dictionary<string, Commodity> commodities = new Dictionary<string, Commodity>();
            ReadCommodities(reader, commodities);

            List<Account> accounts = new List<Account>(numOfAccounts);
            ReadAccounts(reader, accounts, commodities);

            List<Transaction> transactions = new List<Transaction>(numOfTransactions);
            ReadTransactions(reader, transactions, commodities);

            reader.ReadEndElement();
            return new Book(id, commodities, accounts, transactions);


        }

        private static void ReadCommodities(XmlReader reader, Dictionary<string, Commodity> commodities)
        {
            while (reader.IsStartElement(Commodity.ElementName, Namespaces.GnuCash))
            {
                Commodity c = Commodity.ReadXml(reader);
                commodities[c.UniqueName] = c;
            }
        }

        private static void ReadAccounts(XmlReader reader, List<Account> accounts,
            Dictionary<string, Commodity> commodities)
        {
            while (reader.IsStartElement(Account.ElementName, Namespaces.GnuCash))
            {
                Account a = Account.ReadXml(reader, commodities);
                accounts.Add(a);
            }
        }

        private static void ReadTransactions(XmlReader reader, List<Transaction> transactions,
            Dictionary<string, Commodity> commodities)
        {
            while (reader.IsStartElement(Transaction.ElementName, Namespaces.GnuCash))
            {
                transactions.Add(Transaction.ReadXml(reader, commodities));
            }
        }
        


    }
}
