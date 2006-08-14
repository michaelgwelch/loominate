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

    public class Account
    {
        public const string ElementName = "account";
        private const string Version = "2.0.0";

        Dictionary<string, string> kvps;


        string accountName;
        Guid id;
        string description;
        AccountType type;
        string typeString;
        int commodityScu;
        Commodity commodity;
        Guid parent;


        public Account(String name, Guid id, String type, Commodity commodity, 
            int commodityScu, string code, string description, Guid parent, Dictionary<string, string> kvps)
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
                if (kvps.ContainsKey("placeholder"))
                    return bool.Parse(kvps["placeholder"]);

                return false;
            }
        }

        [XmlArrayItem(Namespace = "", ElementName = "slot")]
        [XmlArray(Namespace = Namespaces.Account, ElementName = "slots")]
        public Slot[] Slots
        {
            get
            {
                if (kvps != null)
                {
                    Slot[] retVal = new Slot[kvps.Count];
                    int i = 0;
                    foreach (KeyValuePair<string, string> kvp in kvps)
                    {
                        Slot newSlot = new Slot();
                        newSlot.Key = kvp.Key;
                        newSlot.Value = kvp.Value;
                        retVal[i] = newSlot;
                        i++;
                    }
                    return retVal;
                }
                return null;
            }
            set
            {
                kvps = new Dictionary<string, string>();
                if (value != null)
                    foreach (Slot slot in value)
                    {
                        kvps[slot.Key] = slot.Value;
                    }
            }
        }

        public static Account ReadXml(XmlReader reader, Dictionary<string, Commodity> commodities)
        {

            if ( ! reader.IsStartElement(ElementName, Namespaces.GnuCash)) throw new XmlException("Expected account");
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
            
            Dictionary<string, string> slots = null;
            if (reader.IsStartElement("slots", Namespaces.Account)) {
                slots = GnuCashXml.ReadSlots(reader, Namespaces.Account);
            }

            Guid parent = new Guid();
            if (reader.IsStartElement("parent", Namespaces.Account)) {
                parent = GnuCashXml.ReadIdElement(reader, Namespaces.Account, "parent");
            }

            List<Lot> lots = new List<Lot>();
            if (reader.IsStartElement("lots", Namespaces.Account))
            {
                throw new Exception("haven't implemented lots in Account parsing yet");
            }

            reader.ReadEndElement();

            return new Account(name, id, type, c, int.Parse(commodityscu), code, description, parent, slots);
        }



    }
}
    
