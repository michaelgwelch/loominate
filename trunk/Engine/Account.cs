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

    [XmlType(Namespace=Namespaces.GnuCash)]
    [XmlRoot(Namespace=Namespaces.GnuCash, ElementName="account")]
    public class Account
    {
        [XmlAttribute("version", Namespace=Namespaces.Account)]
        public string Version = "2.0.0";

        // QofInstance inst;
        string accountName;
        Guid id;
//        string accountCode;
//        string description;
//        AccountType type;
//        Commodity commodity;
//        int commodityScu;
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

        [XmlElement(Namespace=Namespaces.Account, ElementName="name")]
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

        [XmlElement(DataType="guid", Namespace=Namespaces.Account, ElementName="id", Type=typeof(Guid))]
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

    }
}
    
