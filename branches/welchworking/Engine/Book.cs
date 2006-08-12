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
        //private Dictionary<string, Commodity> commodities;

        public Book(Guid id)
        {
            this.id = id;
        }

        public static Book ReadXml(XmlReader reader)
        {
            reader.MoveToContent();
            if ( ! reader.IsStartElement(ElementName, Namespaces.GnuCash))
                throw new XmlException("Expected the start of a book element");

            if (reader.GetAttribute("version") != VersionXml)
                throw new XmlException("Expected book element to be at version " + VersionXml);

            reader.ReadStartElement(ElementName, Namespaces.GnuCash);

            Guid id = GnuCashReader.ReadIdElement(reader, Namespaces.Book);

            Dictionary<string, string> slots = null;
            if (reader.IsStartElement("slots", Namespaces.Book)) {
                slots = GnuCashReader.ReadSlots(reader, Namespaces.Book);
            }

            // Note: All of the following except numOfAccounts and numOfTransactions is required.
            // Notice the use of ReadCountData for them instead of ReadCountDataOptional.
            int numOfCommodities = GnuCashReader.ReadCountDataOptional(reader, CountDataType.Commodity);
            int numOfAccounts = GnuCashReader.ReadCountData(reader, CountDataType.Account);
            int numOfTransactions = GnuCashReader.ReadCountData(reader, CountDataType.Transaction);
            int numOfScheduledTrans = GnuCashReader.ReadCountDataOptional(reader, CountDataType.ScheduledTransaction);
            int numOfBudgets = GnuCashReader.ReadCountDataOptional(reader, CountDataType.Budget);
            int numOfCustomers = GnuCashReader.ReadCountDataOptional(reader, CountDataType.Customer);
            int numOfEmployees = GnuCashReader.ReadCountDataOptional(reader, CountDataType.Employee);
            int numOfBillTerms = GnuCashReader.ReadCountDataOptional(reader, CountDataType.BillTerm);
            int numOfInvoices = GnuCashReader.ReadCountDataOptional(reader, CountDataType.Invoice);
            int numOfEntries = GnuCashReader.ReadCountDataOptional(reader, CountDataType.Entry);

            Dictionary<string, Commodity> commodities = new Dictionary<string, Commodity>();
            ReadCommodities(reader, commodities);

            List<Account> accounts = new List<Account>(numOfAccounts);
            ReadAccounts(reader, accounts, commodities);

            return new Book(id);


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


    }
}
