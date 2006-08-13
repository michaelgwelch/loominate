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

        public Transaction(Guid id, Commodity commodity)
        {
            this.id = id;
            this.commodity = commodity;
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

        public static Transaction ReadXml(XmlReader reader, Dictionary<string, Commodity> commodities)
        {
            if (!reader.IsStartElement(ElementName, Namespaces.GnuCash)) throw new XmlException("Expected transaction");
            if (reader.GetAttribute("version") != version) throw new XmlException("Expected transaction to be version " + version);

            reader.Read(); // reads start element

            Guid id = GnuCashXml.ReadIdElement(reader, Namespaces.Transaction);
            Commodity c = GnuCashXml.GetCommodity(reader, Namespaces.Transaction, commodities);

            return new Transaction(id, c);
        }
    }
}
    
