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
        string code;
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
            this.code = code;
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
            writer.WriteStartElement(ElementName, NameSpace.GnuCash);
            writer.WriteAttributeString("version", Version);
            writer.WriteElementString("name", NameSpace.Account, this.accountName);
            GnuCashXml.WriteIdElement(writer, NameSpace.Account, this.id);
            writer.WriteElementString("type", NameSpace.Account, this.typeString);
            GnuCashXml.WriteCommodityId(writer, "commodity", NameSpace.Account, this.commodity);
            writer.WriteElementString("commodity-scu", NameSpace.Account, this.commodityScu.ToString());
            if (code != null) writer.WriteElementString("code", NameSpace.Account, this.code);
            if (description != null) writer.WriteElementString("description", NameSpace.Account, this.description);
            if (kvps != null) GnuCashXml.WriteSlots(writer, this.kvps, "slots", NameSpace.Account, false);
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
                GnuCashXml.WriteIdElement(writer, NameSpace.Account, parent, "parent");
            writer.WriteEndElement();
        }

        internal static Account ReadXml(XmlGnuCashReader reader, 
            Dictionary<string, Commodity> commodities)
        {

            if (!reader.IsStartElement(ElementName, NameSpace.GnuCash)) throw new XmlException("Expected account");
            if (reader.GetAttribute("version") != Version) throw new XmlException("Expected Account to be version " + Version);
            reader.Read(); // start element

            using (DefaultNameSpace.Set(NameSpace.Account))
            {
                string name = reader.ReadString("name");
                Guid id = reader.ReadIdElement();
                string type = reader.ReadString("type");

                // Read the commodity identifier information
                string commodityId = reader.ReadCommodityId(Commodity.ElementName);
                string commodityscu = reader.ReadString("commodity-scu");
                string code = reader.ReadOptionalString("code");
                string nonstandardscu = reader.ReadOptionalString("non-standard-scu");
                string description = reader.ReadOptionalString("description");

                Slots slots = reader.ReadOptionalSlots("slots"); ;

                Guid parent = new Guid();
                if (reader.AtElement("parent"))
                {
                    parent = reader.ReadIdElement("parent");
                }

                List<Lot> lots = new List<Lot>();
                if (reader.AtElement("lots"))
                {
                    throw new Exception("haven't implemented lots in Account parsing yet");
                }

                reader.ReadEndElement();

                Commodity c = commodities[commodityId];
                return new Account(name, id, type, c, int.Parse(commodityscu), code,
                    description, slots, parent);
            }
        }



    }
}
    
