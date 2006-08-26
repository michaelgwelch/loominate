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
    using System.IO;
    using System.Text;
    using System.Xml;
    using Spaces = Loominate.Engine.Namespaces;

    internal class XmlGnuCashWriter : XmlTextWriter
    {

        const string countDataElementName = "count-data";

        // a map from CountDataTypes to the string used to identify them in xml.
        private static Dictionary<CountDataType, string> countDataTypeToString;

        private static void MapEnumsToStrings<T1, T2>(T1[] t1s, T2[] t2s,
            out Dictionary<T1, T2> map1)
        {
            if (t1s.Length != t2s.Length) throw new ArgumentException("length of t1 != length of t2");

            map1 = new Dictionary<T1, T2>();

            for (int i = 0; i < t1s.Length; i++)
            {
                map1[t1s[i]] = t2s[i];
            }
        }

        static XmlGnuCashWriter()
        {
            InitializeCountDataTypesDictionary();
        }

        private static void InitializeCountDataTypesDictionary()
        {
            CountDataType[] countDatas = new CountDataType[] {
                CountDataType.Account,
                CountDataType.BillTerm,
                CountDataType.Book,
                CountDataType.Budget,
                CountDataType.Commodity,
                CountDataType.Customer,
                CountDataType.Employee,
                CountDataType.Entry,
                CountDataType.Invoice,
                CountDataType.ScheduledTransaction,
                CountDataType.Transaction };

            string[] strings = new string[] {
                "account",
                "gnc:GncBillTerm",
                "book",
                "budget",
                "commodity",
                "gnc:GncCustomer",
                "gnc:GncEmployee",
                "gnc:GncEntry",
                "gnc:GncInvoice",
                "schedxaction",
                "transaction" };

            MapEnumsToStrings(countDatas, strings,
                out countDataTypeToString);

        }
        public XmlGnuCashWriter(Stream stream) 
            : base(stream, new UTF8Encoding(false))
        {
            this.Formatting = Formatting.Indented;
        }

        public void Start()
        {
            WriteStartElement(GnuCashFile.ElementName);
            WriteNamespaces();
        }

        public void End()
        {
            WriteEndElement();
            Flush();
        }

        private void WriteNamespaces()
        {
            WriteNamespace("gnc", Spaces.GnuCash);
            WriteNamespace("act", Spaces.Account);
            WriteNamespace("book", Spaces.Book);
            WriteNamespace("cd", Spaces.CountData);
            WriteNamespace("cmdty", Spaces.Commodity);
            WriteNamespace("slot", Spaces.Slot);
            WriteNamespace("split", Spaces.Split);
            WriteNamespace("trn", Spaces.Transaction);
            WriteNamespace("ts", Spaces.Timestamp);
            /*
             *      xmlns:gnc="http://www.gnucash.org/XML/gnc"
     xmlns:act="http://www.gnucash.org/XML/act"
     xmlns:book="http://www.gnucash.org/XML/book"
     xmlns:cd="http://www.gnucash.org/XML/cd"
     xmlns:cmdty="http://www.gnucash.org/XML/cmdty"
     xmlns:price="http://www.gnucash.org/XML/price"
     xmlns:slot="http://www.gnucash.org/XML/slot"
     xmlns:split="http://www.gnucash.org/XML/split"
     xmlns:sx="http://www.gnucash.org/XML/sx"
     xmlns:trn="http://www.gnucash.org/XML/trn"
     xmlns:ts="http://www.gnucash.org/XML/ts"
     xmlns:fs="http://www.gnucash.org/XML/fs"
     xmlns:bgt="http://www.gnucash.org/XML/bgt"
     xmlns:recurrence="http://www.gnucash.org/XML/recurrence"
     xmlns:lot="http://www.gnucash.org/XML/lot"
     xmlns:cust="http://www.gnucash.org/XML/cust"
     xmlns:job="http://www.gnucash.org/XML/job"
     xmlns:addr="http://www.gnucash.org/XML/addr"
     xmlns:owner="http://www.gnucash.org/XML/owner"
     xmlns:taxtable="http://www.gnucash.org/XML/taxtable"
     xmlns:tte="http://www.gnucash.org/XML/tte"
     xmlns:employee="http://www.gnucash.org/XML/employee"
     xmlns:order="http://www.gnucash.org/XML/order"
     xmlns:billterm="http://www.gnucash.org/XML/billterm"
     xmlns:bt-days="http://www.gnucash.org/XML/bt-days"
     xmlns:bt-prox="http://www.gnucash.org/XML/bt-prox"
     xmlns:invoice="http://www.gnucash.org/XML/invoice"
     xmlns:entry="http://www.gnucash.org/XML/entry"
     xmlns:vendor="http://www.gnucash.org/XML/vendor"> */
        }

        private void WriteNamespace(string localName, string value)
        {
            WriteAttributeString("xmlns", localName, null, value);
        }

        /// <summary>
        /// Writes a count-data element.
        /// </summary>
        /// <param name="ns">The namespace used to qualify this instance of the count-data.</param>
        /// <param name="type">The type of this count-data, defined by the <see cref="CountDataType"/> enumeration.</param>
        /// <param name="value">The count to use as the value of the element.</param>
        public void WriteCountData(string ns, 
            CountDataType type, int value)
        {
            WriteStartElement(countDataElementName, ns);
            WriteAttributeString("type", Spaces.CountData,
                countDataTypeToString[type]);
            WriteValue(value.ToString());
            WriteEndElement();
        }

        /// <summary>
        /// Writes a Book.
        /// </summary>
        /// <param name="book">The <see cref="Book"/> to write.</param>
        public void Write(Book book)
        {
            book.WriteXml(this);
        }

        public void WriteCommodityId(string localName, string ns, Commodity c)
        {
            WriteStartElement(localName, ns);
            WriteCommodityElementString("space", c.Namespace);
            WriteCommodityElementString("id", c.Mnemonic);
            WriteEndElement();
        }

        /// <summary>
        /// Writes an element string with the given local name
        /// and value and the Commodity namespace.
        /// </summary>
        /// <param name="localName"></param>
        /// <param name="value"></param>
        public void WriteCommodityElementString(
            string localName, string value)
        {
            WriteElementString(localName, Spaces.Commodity, value);
        }

    }
}
