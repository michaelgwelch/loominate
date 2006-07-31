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
	
	[XmlType(Namespace="http://www.gnucash.org/XML/gnc")]
	[XmlRoot(Namespace="http://www.gnucash.org/XML/gnc", ElementName="commodity")]
	public class Commodity
	{
		private string fullName;
		private string nameSpace;
		private string mnemonic;
		private string cusip;
		private string uniqueName;
		private int fraction;
		const string version = "2.0.0";
		const string cmdtyNs = "http://www.gnucash.org/XML/cmdty";
		public Commodity() {}
		
		public Commodity(Object book, string fullName, string nameSpace,
		                 string mnemonic, string cusip, int fraction)
		{
			this.fullName = fullName;
			this.nameSpace = nameSpace;
			this.mnemonic = mnemonic;
			this.cusip = cusip;
			this.uniqueName = nameSpace + "::" + mnemonic;
			this.fraction = fraction;
		}
		
		[XmlAttribute("version", Namespace=cmdtyNs)]
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
		
		[XmlElement("name", Namespace=cmdtyNs)]
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
		
		[XmlElement("space", Namespace=cmdtyNs)]
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

		[XmlElement("id", Namespace=cmdtyNs)]
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
		
		[XmlElement("xcode", Namespace=cmdtyNs)]
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
				return uniqueName;
			}
		}
		
		[XmlElement("fraction", Namespace=cmdtyNs)]
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
		
		public override bool Equals(object other)
		{
			Commodity c = other as Commodity;
			if (c == null) return false;
			
			return (this.nameSpace == c.nameSpace
			        && this.mnemonic == c.mnemonic);
		}
		
		public override int GetHashCode()
		{
			return uniqueName.GetHashCode();
		}
	
//		public void ReadXml(XmlReader reader)
//		{
//
//		
//			for(reader.MoveToContent(); reader.IsStartElement(); reader.MoveToContent())
//			{
//				if (reader.IsStartElement("cmdty:space")) nameSpace=reader.ReadElementString();
//				if (reader.IsStartElement("cmdty:name")) fullName=reader.ReadElementString();
//				if (reader.IsStartElement("cmdty:fraction")) 
//				else reader.Skip();
//			}
//		
//		}
		

	}
	
}
