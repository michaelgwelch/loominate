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

        static GnuCashXml()
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




        internal static int ReadCountData(XmlReader reader, CountDataType type)
        {
            int result = ReadCountDataOptional(reader, type);
            if (result == -1)
                throw new XmlException("Expected to find count-data");

            return result;
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
            if ( ! reader.IsStartElement(countDataElementName, Namespaces.GnuCash)) return -1;

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

        internal static void WriteIdElement(XmlWriter writer, string ns, Guid value)
        {
            writer.WriteStartElement("id", ns);
            writer.WriteStartAttribute("type");
            writer.WriteValue("guid");
            writer.WriteEndAttribute();
            writer.WriteValue(value.ToString("N"));
            writer.WriteEndElement();
            
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



    }
}
