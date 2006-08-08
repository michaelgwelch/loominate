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
    using System.Xml.Serialization;

    [XmlType(IncludeInSchema=false, AnonymousType=true)]
    public abstract class Id : IXmlSerializable
    {
        #region IXmlSerializable Members
        public Guid Value;
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void ReadXml(XmlReader reader)
        {
            reader.MoveToContent();
            ReadStartElement(reader);
            Value = new Guid(reader.ReadString());
            reader.ReadEndElement();
        }

        public abstract void ReadStartElement(XmlReader reader);

        public void WriteXml(XmlWriter writer)
        {
            //WriteStartElement(writer);
            writer.WriteAttributeString("type", "guid");
            writer.WriteString(Value.ToString("N"));
            //writer.WriteEndElement();
        }

        public abstract void WriteStartElement(XmlWriter writer);

        #endregion
    }
}
