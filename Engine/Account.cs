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
    using System.Xml.Serialization;

    [XmlType(Namespace = Namespaces.GnuCash)]
    [XmlRoot(Namespace = Namespaces.GnuCash, ElementName = "account")]
    public class Account
    {
        [XmlAttribute("version", Namespace = Namespaces.Account)]
        public string Version = "2.0.0";

        // QofInstance inst;
        string accountName;
        AccountId id;
        //        string accountCode;
        string description;
        AccountType type;
        Commodity commodity; Commodity.CommodityId commodityId;
        int commodityScu;
        //        bool nonStandardScu;
        //
        //        AccountGroup parent;
        //        AccountGroup children;
        //
        //        decimal startingBalance;
        //        decimal startingClearedBalance;
        //        decimal startingReconciledBalance;
        //        
        //        decimal balance;
        //        decimal clearedBalance;
        //        decimal reconciledBalance;
        //        
        //        int version;
        //        uint versionCheck;
        //        
        //        List<Split> splits;
        //        List<Lot> lots;
        //        
        //        //Policy policy;
        //        
        //        bool isBalanceDirty;
        //        bool isSortDirty;
        //        
        //        short mark;

        [XmlElement(Namespace = Namespaces.Account, ElementName = "name")]
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

        [XmlElement(Namespace = Namespaces.Account, ElementName = "id")]
        public AccountId Id
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

        [XmlElement(Namespace = Namespaces.Account, ElementName = "type")]
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

        [XmlIgnore()]
        public Commodity Commodity
        {
            get
            {
                return commodity;
            }
            set
            {
                commodity = value;
            }
        }

        [XmlElement(Namespace=Namespaces.Account, ElementName="commodity", Type=typeof(Commodity.CommodityId))]
        public Commodity.CommodityId CommodityId
        {
            // TODO instead of saving the id info, we really want to lookup the actual commodity
            // and change its value.
            get
            {
                return commodityId;
            }
            set
            {
                commodityId = value;
            }
        }

        [XmlElement(Namespace=Namespaces.Account, ElementName="commodity-scu")]
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

        [XmlElement(Namespace = Namespaces.Account, ElementName="description")]
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

        public class AccountId : IXmlSerializable
        {
            #region IXmlSerializable Members
            public Guid Value;
            public System.Xml.Schema.XmlSchema GetSchema()
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public void ReadXml(System.Xml.XmlReader reader)
            {
                reader.MoveToContent();
                reader.ReadStartElement("id", Namespaces.Account);
                Value = new Guid(reader.ReadString());
                reader.ReadEndElement();
            }

            public void WriteXml(System.Xml.XmlWriter writer)
            {
                writer.WriteStartElement("id", Namespaces.Account);
                writer.WriteAttributeString("type", "guid");
                writer.WriteString(Value.ToString("N"));
                writer.WriteEndElement();
            }

            #endregion
        }


    }
}
    