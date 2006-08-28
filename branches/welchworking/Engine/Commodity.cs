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
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    public class Commodity
    {
        private string fullName;
        private string nameSpace;
        private string mnemonic;
        private string cusip;
        private int fraction;
        private string getQuotes;
        private string quoteSource;
        private string quoteTz;

        const string version = "2.0.0";
        public const string ElementName = "commodity";

        public Commodity(string fullName, string nameSpace,
                         string mnemonic, string cusip, int fraction,
                         string get_quotes, string quote_source, string quote_tz)
        {
            this.fullName = fullName;
            this.nameSpace = nameSpace;
            this.mnemonic = mnemonic;
            this.cusip = cusip;
            this.fraction = fraction;
            this.getQuotes = get_quotes;
            this.quoteSource = quote_source;
            this.quoteTz = quote_tz;
        }

        public String Version
        {
            get
            {
                return version;
            }
            set
            {

            }
        }

        public string FullName
        {
            get
            {
                return fullName;
            }
            set
            {
                fullName = value;
            }
        }

        public string Namespace
        {
            get
            {
                return nameSpace;
            }
            set
            {
                nameSpace = value;
            }
        }

        public string Mnemonic
        {
            get
            {
                return mnemonic;
            }
            set
            {
                mnemonic = value;
            }
        }

        public string Cusip
        {
            get
            {
                return cusip;
            }
            set
            {
                cusip = value;
            }
        }

        public string UniqueName
        {
            get
            {
                return CreateUniqueName(nameSpace, mnemonic);
            }
        }

        public int Fraction
        {
            get
            {
                return fraction;
            }
            set
            {
                fraction = value;
            }
        }


        public static string CreateUniqueName(string ns, string mnemonic)
        {
            return ns + "::" + mnemonic;
        }

        public override bool Equals(object other)
        {
            Commodity c = other as Commodity;
            if (c == null) return false;

            return (this.nameSpace == c.nameSpace
                    && this.mnemonic == c.mnemonic);
        }

        public override int GetHashCode()
        {
            return UniqueName.GetHashCode();
        }


        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement(ElementName, Namespaces.GnuCash);
            writer.WriteAttributeString("version", version);
            writer.WriteElementString("space", Namespaces.Commodity, this.nameSpace);
            writer.WriteElementString("id", Namespaces.Commodity, this.mnemonic);
            writer.WriteElementString("name", Namespaces.Commodity, this.fullName);
            if (cusip != null) writer.WriteElementString("xcode", Namespaces.Commodity, this.cusip);
            writer.WriteElementString("fraction", Namespaces.Commodity, this.fraction.ToString());
            if (getQuotes != null) writer.WriteElementString("get_quotes", Namespaces.Commodity, this.getQuotes);
            if (quoteSource != null) writer.WriteElementString("quote_source", Namespaces.Commodity, this.quoteSource);
            if (quoteTz != null) writer.WriteElementString("quote_tz", Namespaces.Commodity, this.quoteTz);

            writer.WriteEndElement();
        }

        public static Commodity ReadXml(XmlReader reader)
        {
            reader.MoveToContent();

            if (!reader.IsStartElement(ElementName, Namespaces.GnuCash)) throw new XmlException("Expected commodity");
            if (reader.GetAttribute("version") != version) throw new XmlException("Expected commodity to be at version " + version);

            reader.ReadStartElement(ElementName, Namespaces.GnuCash);
            
            string ns = reader.ReadElementString("space", Namespaces.Commodity);
            string id = reader.ReadElementString("id", Namespaces.Commodity);
            string name = reader.ReadElementString("name", Namespaces.Commodity);
            string xcode = GnuCashXml.ReadOptionalElementString(reader, "xcode", Namespaces.Commodity);
            string fraction = reader.ReadElementString("fraction", Namespaces.Commodity);
            string get_quotes = GnuCashXml.ReadOptionalElementString(reader, "get_quotes", Namespaces.Commodity);
            string quote_source = GnuCashXml.ReadOptionalElementString(reader, "quote_source", Namespaces.Commodity);
            string quote_tz = GnuCashXml.ReadOptionalElementString(reader, "quote_tz", Namespaces.Commodity);

            /*
             *   <cmdty:get_quotes/>
  <cmdty:quote_source>currency</cmdty:quote_source>
  <cmdty:quote_tz/>*/
            


            reader.ReadEndElement();

            return new Commodity(name, ns, id, xcode, int.Parse(fraction), get_quotes, quote_source, quote_tz);


        }

    }

}
