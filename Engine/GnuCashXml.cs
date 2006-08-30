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
    using System.Collections;
    using System.Collections.Generic;
    using System.Xml;

    using Slots = System.Collections.Generic.Dictionary<string, Pair<string, object>>;
    using Slot = System.Collections.Generic.KeyValuePair<string, Pair<string, object>>;
    using SlotValuePair = Pair<string, object>;

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

        //internal static int ReadCountData(XmlReader reader, CountDataType type)
        //{
        //    int? result = ReadOptionalCountData(reader, type);
        //    if (result == null)
        //        throw new XmlException("Expected to find count-data");
        //    return (int) result;
        //}

        internal static void WriteCountData(XmlWriter writer,

            string ns, CountDataType type, int? value)
        {

            if (value != null)
            {
                writer.WriteStartElement(countDataElementName, ns);
                writer.WriteAttributeString("type", NameSpace.CountData,
                    countDataTypeToString[type]);
                writer.WriteValue(value.ToString());
                writer.WriteEndElement(); 
            }
        }



        internal static void WriteNamespaces(XmlWriter writer)
        {
            WriteNamespace(writer, "gnc", NameSpace.GnuCash);
            WriteNamespace(writer, "act", NameSpace.Account);
            WriteNamespace(writer, "book", NameSpace.Book);
            WriteNamespace(writer, "cd", NameSpace.CountData);
            WriteNamespace(writer, "cmdty", NameSpace.Commodity);
            WriteNamespace(writer, "slot", NameSpace.Slot);
            WriteNamespace(writer, "split", NameSpace.Split);
            WriteNamespace(writer, "trn", NameSpace.Transaction);
            WriteNamespace(writer, "ts", NameSpace.Timestamp);

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





        internal static void WriteSlots(XmlWriter writer,
            Slots slots, string localName, string ns, bool isFrame)
        {
            writer.WriteStartElement(localName, ns);
            if (isFrame) writer.WriteAttributeString("type", "frame");

            foreach (Slot slot in slots)
            {
                string key = slot.Key;
                SlotValuePair value = slot.Value;
                string type = value.First;
                writer.WriteStartElement("slot");
                writer.WriteElementString("key", NameSpace.Slot, key);

                if (type == "frame")
                {
                    WriteSlots(writer, value.Second as Slots,
                        "value", NameSpace.Slot, true);
                }
                else
                {
                    writer.WriteStartElement("value", NameSpace.Slot);
                    writer.WriteAttributeString("type", type);
                    if (value.Second.ToString() == String.Empty) writer.WriteRaw(String.Empty); // forces a start and end element to be written
                    else writer.WriteString(value.Second.ToString());
                    writer.WriteEndElement(); // </value>
                }
                writer.WriteEndElement(); // </slot>

            }

            writer.WriteEndElement();
        }



 




        internal static void WriteCommodityId(XmlWriter writer, string localName, string ns, Commodity c)
        {
            writer.WriteStartElement(localName, ns);
            writer.WriteElementString("space", NameSpace.Commodity, c.Namespace);
            writer.WriteElementString("id", NameSpace.Commodity, c.Mnemonic);
            writer.WriteEndElement();
        }

        internal static void WriteDate(XmlWriter writer, string localName,
            string ns, DateTime value)
        {
            writer.WriteStartElement(localName, ns);
            writer.WriteElementString("date",
                NameSpace.Timestamp, FormatDateTime(value));
            writer.WriteEndElement();
        }

        private static string FormatDateTime(DateTime dt)
        {
            System.Text.StringBuilder bldr = new System.Text.StringBuilder();
            bldr.Append(dt.ToString("yyyy-MM-dd HH:mm:ss zzz"));
            bldr.Replace(":", "", 20, 4);
            return bldr.ToString();
        }

        internal static void WriteElementString(XmlWriter writer,
            string localName, string ns, string value)
        {
            writer.WriteStartElement(localName, ns);
            if (value == String.Empty) writer.WriteRaw(String.Empty); // Forces a start and end element to be written.
            else writer.WriteString(value);
            writer.WriteEndElement();
        }

    }

}
