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


    internal static class GnuCashXml
    {
        const string countDataElementName = "count-data";

        // a map from CountDataTypes to the string used to identify them in xml.
        private static Dictionary<CountDataType, string> countDataTypeToString;

        static GnuCashXml()
        {
            InitializeCountDataTypesDictionary();
        }


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

        internal static int ReadCountData(XmlReader reader, CountDataType type)
        {
            int result = ReadCountDataOptional(reader, type);
            if (result == -1)
                throw new XmlException("Expected to find count-data");
            return result;
        }

        internal static void WriteCountData(XmlWriter writer,

            string ns, CountDataType type, int value)
        {
            writer.WriteStartElement(countDataElementName, ns);
            writer.WriteAttributeString("type", Namespaces.CountData,
                countDataTypeToString[type]);
            writer.WriteValue(value.ToString());
            writer.WriteEndElement();
        }



        internal static void WriteNamespaces(XmlWriter writer)
        {
            WriteNamespace(writer, "gnc", Namespaces.GnuCash);
            WriteNamespace(writer, "act", Namespaces.Account);
            WriteNamespace(writer, "book", Namespaces.Book);
            WriteNamespace(writer, "cd", Namespaces.CountData);
            WriteNamespace(writer, "cmdty", Namespaces.Commodity);
            WriteNamespace(writer, "slot", Namespaces.Slot);
            WriteNamespace(writer, "split", Namespaces.Split);
            WriteNamespace(writer, "trn", Namespaces.Transaction);
            WriteNamespace(writer, "ts", Namespaces.Timestamp);

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



        private static void WriteNamespace(XmlWriter writer, string localName, string value)
        {
            writer.WriteAttributeString("xmlns", localName, null, value);
        }



        /// <summary>
        /// Reads a count data element and returns the integer value. Returns -1 if the element is not found,
        /// because count data is sometimes optional.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static int ReadCountDataOptional(XmlReader reader, CountDataType type)
        {
            reader.MoveToContent();
            // if this isn't a count-data element then return -1, it's possible this
            // was an optional instance of this element. Let the caller sort it out.

            if (!reader.IsStartElement(countDataElementName, Namespaces.GnuCash)) return -1;



            // if count-data type is incorrect then return -1, it's possible this

            // was an optional instance of this element. Let the caller sort it out.

            string expectedType = countDataTypeToString[type];

            string actualType = reader.GetAttribute("type", Namespaces.CountData);

            if (expectedType != actualType) return -1;



            string val = reader.ReadElementString(countDataElementName, Namespaces.GnuCash);

            return int.Parse(val);

        }



        /// <summary>

        /// Helper method to read id elements. These always have the same structure but a different namespace,

        /// so the namespace must be specified.

        /// </summary>

        /// <param name="ns">The namespace of the expected id element</param>

        /// <returns></returns>

        internal static Guid ReadIdElement(XmlReader reader, string ns)
        {

            return ReadIdElement(reader, ns, "id");

        }



        internal static void WriteIdElement(XmlWriter writer, string ns, Guid value, string localName)
        {

            writer.WriteStartElement(localName, ns);

            writer.WriteStartAttribute("type");

            writer.WriteValue("guid");

            writer.WriteEndAttribute();

            writer.WriteValue(value.ToString("N"));

            writer.WriteEndElement();

        }



        internal static void WriteIdElement(XmlWriter writer, string ns, Guid value)
        {

            WriteIdElement(writer, ns, value, "id");

        }



        internal static Guid ReadIdElement(XmlReader reader, string ns, string localName)
        {

            reader.MoveToContent();

            string idString = reader.ReadElementString(localName, ns);

            return new Guid(idString);

        }



        internal static Dictionary<string, string> ReadSlots(XmlReader reader, string ns)
        {

            Dictionary<string, string> slots = new Dictionary<string, string>();



            reader.ReadStartElement("slots", ns);

            while (reader.IsStartElement("slot"))
            {

                reader.Read();

                string key = reader.ReadElementString("key", Namespaces.Slot);

                string value = reader.ReadElementString("value", Namespaces.Slot);

                slots[key] = value;

                reader.ReadEndElement();

            }

            reader.ReadEndElement();



            return slots;

        }



        internal static string ReadOptionalElementString(XmlReader reader, string localName, string ns)
        {

            if (reader.IsStartElement(localName, ns)) return reader.ReadElementString();

            return null;

        }



        /// <summary>

        /// Reads commodity id information from reader, and then uses that to select the appropriate

        /// commodity from commodities.

        /// </summary>

        /// <param name="reader"></param>

        /// <param name="ns">The namespace that the commodity element is part of.</param>

        /// <param name="commodities"></param>

        /// <param name="localName">"currency" or "commodity" depending on the parent element. You must specify the correct one.</param>

        /// <returns></returns>

        internal static Commodity GetCommodity(XmlReader reader, string localName, string ns, Dictionary<string, Commodity> commodities)
        {
            reader.ReadStartElement(localName, ns);
            string commodityns = reader.ReadElementString("space", Namespaces.Commodity);
            string commodityid = reader.ReadElementString("id", Namespaces.Commodity);
            string uniqueId = Commodity.CreateUniqueName(commodityns, commodityid);
            Commodity c = commodities[uniqueId];
            reader.ReadEndElement();
            return c;
        }



        internal static void WriteCommodityId(XmlWriter writer, string localName, string ns, Commodity c)
        {
            writer.WriteStartElement(localName, ns);
            writer.WriteElementString("space", Namespaces.Commodity, c.Namespace);
            writer.WriteElementString("id", Namespaces.Commodity, c.Mnemonic);
            writer.WriteEndElement();
        }



    }

}
