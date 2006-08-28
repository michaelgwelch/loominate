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
    using System.Xml.Serialization;

    using Slots = System.Collections.Generic.Dictionary<string, Pair<string, object>>;

    public class Account
    {
        public const string ElementName = "account";
        private const string Version = "2.0.0";

        Slots kvps;


        string accountName;
        Guid id;
        string description;
        AccountType type;
        string typeString;
        int commodityScu;
        Commodity commodity;
        Guid parent;


        public Account(String name, Guid id, String type, Commodity commodity,
            int commodityScu, string code, string description,
            Slots kvps, Guid parent)
        {
            this.accountName = name;
            this.id = id;
            this.typeString = type;
            this.commodity = commodity;
            this.commodityScu = commodityScu;
            this.description = description;
            this.parent = parent;
            this.kvps = kvps;
        }

        public string Name
        {
            get
            {
                return accountName;
            }
            set
            {
                accountName = value;
            }
        }

        public Guid Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        public AccountType AccountType
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }

        //[XmlIgnore()]
        //public Commodity Commodity
        //{
        //    get
        //    {
        //        return commodity;
        //    }
        //    set
        //    {
        //        commodity = value;
        //    }
        //}

        //[XmlElement(Namespace = Namespaces.Account, ElementName = "commodity", Type = typeof(Commodity.CommodityId))]
        //public Commodity.CommodityId CommodityId
        //{
        //    // TODO instead of saving the id info, we really want to lookup the actual commodity
        //    // and change its value.
        //    get
        //    {
        //        return commodityId;
        //    }
        //    set
        //    {
        //        commodityId = value;
        //    }
        //}

        [XmlElement(Namespace = Namespaces.Account, ElementName = "commodity-scu")]
        public int CommodityScu
        {
            get
            {
                return commodityScu;
            }
            set
            {
                commodityScu = value;
            }
        }

        [XmlElement(Namespace = Namespaces.Account, ElementName = "description")]
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
            }
        }

        public bool IsPlaceholder
        {
            get
            {
                if (kvps == null) return false;
                if (kvps.ContainsKey("placeholder"))
                    return bool.Parse(kvps["placeholder"].Second as string);

                return false;
            }
        }



        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement(ElementName, Namespaces.GnuCash);
            writer.WriteAttributeString("version", Version);
            writer.WriteElementString("name", Namespaces.Account, this.accountName);
            GnuCashXml.WriteIdElement(writer, Namespaces.Account, this.id);
            writer.WriteElementString("type", Namespaces.Account, this.typeString);
            GnuCashXml.WriteCommodityId(writer, "commodity", Namespaces.Account, this.commodity);
            writer.WriteElementString("commodity-scu", Namespaces.Account, this.commodityScu.ToString());
            if (description != null) writer.WriteElementString("description", Namespaces.Account, this.description);
            if (kvps != null) GnuCashXml.WriteSlots(writer, this.kvps, "slots", Namespaces.Account, false);
            //if (IsPlaceholder)
            //{
            //    writer.WriteStartElement("slots", Namespaces.Account);
            //    writer.WriteStartElement("slot");
            //    writer.WriteElementString("key", Namespaces.Slot, "placeholder");
            //    writer.WriteStartElement("value", Namespaces.Slot);
            //    writer.WriteAttributeString("type", "string");
            //    writer.WriteValue("true");
            //    writer.WriteEndElement(); // </value>
            //    writer.WriteEndElement(); // </slot>
            //    writer.WriteEndElement(); // </slots>
            //}

            if (parent != Guid.Empty)
                GnuCashXml.WriteIdElement(writer, Namespaces.Account, parent, "parent");
            writer.WriteEndElement();
        }

        public static Account ReadXml(XmlReader reader, Dictionary<string, Commodity> commodities)
        {

            if (!reader.IsStartElement(ElementName, Namespaces.GnuCash)) throw new XmlException("Expected account");
            if (reader.GetAttribute("version") != Version) throw new XmlException("Expected Account to be version " + Version);
            reader.Read(); // start element

            string name = reader.ReadElementString("name", Namespaces.Account);
            Guid id = GnuCashXml.ReadIdElement(reader, Namespaces.Account);
            string type = reader.ReadElementString("type", Namespaces.Account);

            // Read the commodity identifier information
            Commodity c = GnuCashXml.GetCommodity(reader, Commodity.ElementName, Namespaces.Account, commodities);

            string commodityscu = reader.ReadElementString("commodity-scu", Namespaces.Account);
            string code = GnuCashXml.ReadOptionalElementString(reader, "code", Namespaces.Account);
            string nonstandardscu = GnuCashXml.ReadOptionalElementString(reader, "non-standard-scu", Namespaces.Account);
            string description = GnuCashXml.ReadOptionalElementString(reader, "description", Namespaces.Account);

            Slots slots = null;
            if (reader.IsStartElement("slots", Namespaces.Account))
            {
                slots = GnuCashXml.ReadSlots(reader, Namespaces.Account, "slots");
            }

            Guid parent = new Guid();
            if (reader.IsStartElement("parent", Namespaces.Account))
            {
                parent = GnuCashXml.ReadIdElement(reader, Namespaces.Account, "parent");
            }

            List<Lot> lots = new List<Lot>();
            if (reader.IsStartElement("lots", Namespaces.Account))
            {
                throw new Exception("haven't implemented lots in Account parsing yet");
            }

            reader.ReadEndElement();

            return new Account(name, id, type, c, int.Parse(commodityscu), code, 
                description, slots, parent);
        }



    }
}
    
