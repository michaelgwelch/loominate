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

    using Slots = System.Collections.Generic.Dictionary<string, Pair<string, object>>;
    using SplitList = System.Collections.Generic.List<Split>;

    public class Transaction
    {

        public const string ElementName = "transaction";
        const string version = "2.0.0";

        Guid id;
        Commodity commodity;
        public string num;
        public DateTime datePosted;
        Pair<DateTime, int?> dateEntered;
        public string description;
        Slots kvps;
        public SplitList splits;

        public Transaction(Guid id, Commodity commodity,
             string num, DateTime posted, Pair<DateTime, int?> entered,
                 string description, Slots kvps,
                 SplitList splits)
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
            writer.WriteStartElement(ElementName, NameSpace.GnuCash);
            writer.WriteAttributeString("version", version);
            GnuCashXml.WriteIdElement(writer, NameSpace.Transaction, this.id);
            GnuCashXml.WriteCommodityId(writer, "currency", NameSpace.Transaction, this.commodity);
            if (num != null) writer.WriteElementString("num", NameSpace.Transaction, this.num);
            WriteDatePosted(writer);
            WriteDateEntered(writer);
            GnuCashXml.WriteElementString(writer, "description", NameSpace.Transaction, description);
            if (this.kvps != null) GnuCashXml.WriteSlots(writer, this.kvps, "slots", NameSpace.Transaction, false);
            writer.WriteStartElement("splits", NameSpace.Transaction);
            foreach (Split split in splits) split.WriteXml(writer);
            writer.WriteEndElement(); // </splits>
            writer.WriteEndElement(); // </transaction>
        }

        internal static Transaction ReadXml(XmlGnuCashReader reader, Dictionary<string, Commodity> commodities)
        {
            if (!reader.IsStartElement(ElementName, NameSpace.GnuCash)) throw new XmlException("Expected transaction");
            if (reader.GetAttribute("version") != version) throw new XmlException("Expected transaction to be version " + version);

            reader.Read(); // reads start element

            using (DefaultNameSpace.Set(NameSpace.Transaction))
            {
                Guid id             = reader.ReadIdElement();
                string commodityId  = reader.ReadCommodityId("currency");
                string num          = reader.ReadOptionalString("num");

                DateTime posted     = reader.ReadDate("date-posted");
                Pair<DateTime, int?> entered = ReadDateEntered(reader);
                string description  = reader.ReadString("description");

                Slots kvps          = reader.ReadOptionalSlots("slots");

                SplitList splits = new SplitList();
                reader.ReadStartElement("splits", NameSpace.Transaction);
                while (reader.AtElement(Split.ElementName))
                {
                    splits.Add(reader.ReadSplit());
                }
                reader.ReadEndElement(); // </splits>
                reader.ReadEndElement(); // </transaction>


                Commodity c = commodities[commodityId];
                return new Transaction(id, c, num, posted, entered, description, kvps, splits);
            }
        }


        private void WriteDatePosted(XmlWriter writer)
        {
            GnuCashXml.WriteDate(writer, "date-posted", NameSpace.Transaction,
                datePosted);
        }


        private void WriteDateEntered(XmlWriter writer)
        {
            string localName = "date-entered";
            string ns = NameSpace.Transaction;
            writer.WriteStartElement(localName, ns);
            writer.WriteElementString("date",
                NameSpace.Timestamp, FormatDateTime(dateEntered.First));
            if (dateEntered.Second != null) writer.WriteElementString("ns", NameSpace.Timestamp, dateEntered.Second.ToString());
            writer.WriteEndElement();
        }

        private static Pair<DateTime, int?> ReadDateEntered(XmlGnuCashReader reader)
        {
            reader.ReadStartElement("date-entered", NameSpace.Transaction);
            using (DefaultNameSpace.Set(NameSpace.Timestamp))
            {
                DateTime entered = DateTime.Parse(reader.ReadString("date"));
                string nsString = reader.ReadOptionalString("ns");

                int? ns = (nsString == null) ? (int?)null : int.Parse(nsString);
                reader.ReadEndElement();
                return new Pair<DateTime, int?>(entered, ns);
            }
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
    
