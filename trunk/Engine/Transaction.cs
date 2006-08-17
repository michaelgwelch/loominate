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

    public class Transaction
    {

        public const string ElementName = "transaction";
        const string version = "2.0.0";

        Guid id;
        Commodity commodity;
        string num;
        DateTime datePosted;
        DateTime dateEntered;
        string description;
        Dictionary<string, string> kvps;
        List<Split> splits;

        public Transaction(Guid id, Commodity commodity,
             string num, DateTime posted, DateTime entered, 
                 string description, Dictionary<string, string> kvps,
                 List<Split> splits)
        {
            this.id = id;
            this.commodity = commodity;
            this.num = num;
            this.datePosted = posted;
            this.dateEntered = entered;
            this.description = description;
            this.kvps = kvps;
            this.splits = splits;

        }

        public Guid Id
        {
            get
            {
                return this.id;
            }
        }

        public Commodity Commodity
        {
            get
            {
                return this.commodity;
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement(ElementName, Namespaces.GnuCash);
            GnuCashXml.WriteIdElement(writer, Namespaces.Transaction, this.id);
            GnuCashXml.WriteCommodityId(writer, "currency", Namespaces.Transaction, this.commodity);
            WriteDatePosted(writer);
            WriteDateEntered(writer);
            writer.WriteElementString("description", Namespaces.Transaction, description);
            writer.WriteStartElement("splits", Namespaces.Transaction);
            foreach (Split split in splits) split.WriteXml(writer);
            writer.WriteEndElement(); // </splits>
            writer.WriteEndElement(); // </transaction>
        }

        public static Transaction ReadXml(XmlReader reader, Dictionary<string, Commodity> commodities)
        {
            if (!reader.IsStartElement(ElementName, Namespaces.GnuCash)) throw new XmlException("Expected transaction");
            if (reader.GetAttribute("version") != version) throw new XmlException("Expected transaction to be version " + version);

            reader.Read(); // reads start element

            Guid id = GnuCashXml.ReadIdElement(reader, Namespaces.Transaction);
            Commodity c = GnuCashXml.GetCommodity(reader, "currency", Namespaces.Transaction, commodities);
            string num = GnuCashXml.ReadOptionalElementString(reader, "num", Namespaces.Transaction);

            DateTime posted = ReadDatePosted(reader);
            DateTime entered = ReadDateEntered(reader);
            string description = reader.ReadElementString("description", Namespaces.Transaction);

            Dictionary<string, string> kvps = null;
            if (reader.IsStartElement("slots", Namespaces.Transaction))
            {
                kvps = GnuCashXml.ReadSlots(reader, Namespaces.Transaction);
            }

            List<Split> splits = new List<Split>();
            reader.ReadStartElement("splits", Namespaces.Transaction);
            while (reader.IsStartElement(Split.ElementName, Namespaces.Transaction))
            {
                splits.Add(Split.ReadXml(reader));
            }
            reader.ReadEndElement(); // </splits>
            reader.ReadEndElement(); // </transaction>

            return new Transaction(id, c, num, posted, entered, description, kvps, splits);
        }


        private void WriteDatePosted(XmlWriter writer)
        {
            writer.WriteStartElement("date-posted", Namespaces.Transaction);
            writer.WriteElementString("date", Namespaces.Timestamp, FormatDateTime(datePosted));
            writer.WriteEndElement();
        }

        private static DateTime ReadDatePosted(XmlReader reader)
        {
            reader.ReadStartElement("date-posted", Namespaces.Transaction);
            DateTime posted = DateTime.Parse(reader.ReadElementString("date", Namespaces.Timestamp));
            reader.ReadEndElement();
            return posted;
        }


        private void WriteDateEntered(XmlWriter writer)
        {
            writer.WriteStartElement("date-entered", Namespaces.Transaction);
            writer.WriteElementString("date", 
                Namespaces.Timestamp, FormatDateTime(dateEntered));
            writer.WriteEndElement();
        }

        private static DateTime ReadDateEntered(XmlReader reader)
        {
            reader.ReadStartElement("date-entered", Namespaces.Transaction);
            DateTime posted = DateTime.Parse(reader.ReadElementString("date", Namespaces.Timestamp));
            reader.ReadEndElement();
            return posted;
        }

        private static string FormatDateTime(DateTime dt)
        {
            System.Text.StringBuilder bldr = new System.Text.StringBuilder();
            bldr.Append(dt.ToString("yyyy-MM-dd HH:mm:ss zzz"));
            bldr.Replace(":", "", 20, 4);
            return bldr.ToString();
        }


    }
}
    
