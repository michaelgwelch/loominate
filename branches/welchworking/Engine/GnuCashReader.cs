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

    internal static class GnuCashReader
    {
        // a map from CountDataTypes to the string used to identify them in xml.
        private static Dictionary<CountDataType, string> countDataTypes;

        static GnuCashReader()
        {
            InitializeCountDataTypesDictionary();

        }

        private static void InitializeCountDataTypesDictionary()
        {
            countDataTypes = new Dictionary<CountDataType, string>();
            countDataTypes[CountDataType.Account] = "account";
            countDataTypes[CountDataType.BillTerm] = "gnc:GncBillTerm";
            countDataTypes[CountDataType.Book] = "book";
            countDataTypes[CountDataType.Budget] = "budget";
            countDataTypes[CountDataType.Commodity] = "commodity";
            countDataTypes[CountDataType.Customer] = "gnc:GncCustomer";
            countDataTypes[CountDataType.Employee] = "gnc:GncEmployee";
            countDataTypes[CountDataType.Entry] = "gnc:GncEntry";
            countDataTypes[CountDataType.Invoice] = "gnc:GncInvoice";
            countDataTypes[CountDataType.ScheduledTransaction] = "schedxaction";
            countDataTypes[CountDataType.Transaction] = "transaction";
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
            const string elementName = "count-data";

            reader.MoveToContent();

            // if this isn't a count-data element then return -1, it's possible this
            // was an optional instance of this element. Let the caller sort it out.
            if ( ! reader.IsStartElement(elementName, Namespaces.GnuCash)) return -1;

            // if count-data type is incorrect then return -1, it's possible this
            // was an optional instance of this element. Let the caller sort it out.
            string expectedType = countDataTypes[type];
            string actualType = reader.GetAttribute("type", Namespaces.CountData);
            if (expectedType != actualType) return -1;

            string val = reader.ReadElementString(elementName, Namespaces.GnuCash);
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

    }
}
