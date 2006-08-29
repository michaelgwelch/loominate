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
    using System.IO;
    using Namespace = Loominate.Engine.NameSpace;
    using Slots = System.Collections.Generic.Dictionary<string, Pair<string, object>>;

    internal class XmlGnuCashReader : XmlTextReader
    {
        const string countDataElementName = "count-data";
                // a map from CountDataTypes to the string used to identify them in xml.
        private static Dictionary<CountDataType, string> countDataTypeToString;


        static XmlGnuCashReader()
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
        public XmlGnuCashReader(Stream stream)
            : base(stream)
        {
        }

        public int ReadCountData(CountDataType type)
        {
            int? result = ReadOptionalCountData(type);
            if (result == null)
                throw new XmlException("Expected to find count-data");
            return (int) result;
        }

        /// <summary>
        /// Reads a count data element and returns the integer value. Returns -1 if the element is not found,
        /// because count data is sometimes optional.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public int? ReadOptionalCountData(CountDataType type)
        {
            MoveToContent();

            // if this isn't a count-data element then return null, 
            if ( ! IsStartElement(countDataElementName, Namespace.GnuCash))
                return null;

            // if count-data type is incorrect then return null, it's possible this
            // was an optional instance of this element. Let the caller sort it out.
            string expectedType = countDataTypeToString[type];
            string actualType = GetAttribute("type", Namespace.CountData);
            if (expectedType != actualType) return null;

            string val = ReadElementString(countDataElementName, Namespace.GnuCash);
            return int.Parse(val);
        }

        #region Classes 

        public Book ReadBook()
        {
            return Book.ReadXml(this);
        }

        #endregion


        #region GnuCash Elements

        #region ReadIdElement

        public Guid ReadIdElement()
        {
            DefaultNameSpace.AssertSet();
            return ReadIdElement(DefaultNameSpace.Current);
        }

        public Guid ReadIdElement(string localName)
        {
            DefaultNameSpace.AssertSet();
            return ReadIdElement(localName, DefaultNameSpace.Current);
        }


        public Guid ReadIdElement(string localName, NameSpace ns)
        {
            string idString = ReadElementString(localName, ns);
            return new Guid(idString);
        }

        /// <summary>
        /// Helper method to read id elements. These always have the same structure but a different namespace,
        /// so the namespace must be specified.
        /// </summary>
        /// <param name="ns">The namespace of the expected id element</param>
        /// <returns></returns>
        public Guid ReadIdElement(NameSpace ns)
        {
            return ReadIdElement("id", ns);
        }

        #endregion

        public Slots
            ReadSlots(string localName, NameSpace ns)
        {
            Slots slots = new Slots();

            MoveToContent();
            bool isEmpty = IsEmptyElement;
            ReadStartElement(localName, ns);
            while (IsStartElement("slot"))
            {
                Read();
                string key = ReadElementString("key", NameSpace.Slot);

                MoveToContent();
                string type = GetAttribute("type");
                object value;
                if (type == "frame") // recursively call ourselves
                {
                    value = ReadSlots("value", NameSpace.Slot);
                }
                else
                {
                    value = ReadElementString("value", NameSpace.Slot);
                }
                slots[key] = new Pair<string, object>(type, value);
                ReadEndElement();
            }

            if (!isEmpty) ReadEndElement();
            return slots;

        }

        public Slots ReadOptionalSlots(string localName, NameSpace ns)
        {
            MoveToContent();
            if (IsStartElement(LocalName, ns)) return ReadSlots(localName, ns);
            else return null;
        }

        public string ReadString(string localName)
        {
            DefaultNameSpace.AssertSet();
            return ReadElementString(localName, DefaultNameSpace.Current);
        }

        public string ReadOptionalString(string localName, NameSpace ns)
        {
            if (IsStartElement(localName, ns)) return ReadElementString();
            return null;
        }

        public string ReadOptionalString(string localName)
        {
            DefaultNameSpace.AssertSet();
            return ReadOptionalString(localName, DefaultNameSpace.Current);
        }

        public ReconcileState ReadReconcileState()
        {
            string reconcileString = 
                ReadElementString("reconciled-state", NameSpace.Split);
            ReconcileState reconcileState;
            switch (reconcileString)
            {
                case "y":
                    reconcileState = ReconcileState.Reconciled;
                    break;
                case "n":
                    reconcileState = ReconcileState.NotReconciled;
                    break;
                case "c":
                    reconcileState = ReconcileState.Cleared;
                    break;
                default:
                    throw new XmlException("Expected a valid reconcile state");
            }
            return reconcileState;
        }

        public DateTime? ReadOptionalDate(string localName, NameSpace ns)
        {
            if (IsStartElement(localName, ns))
            {
                Read();
                DateTime date = DateTime.Parse(ReadElementString("date", NameSpace.Timestamp));
                ReadEndElement();
                return date;
            }

            return null;
        }

        public DateTime? ReadOptionalDate(string localName)
        {
            DefaultNameSpace.AssertSet();
            return ReadOptionalDate(localName, DefaultNameSpace.Current);
        }

        #endregion
    }
}
