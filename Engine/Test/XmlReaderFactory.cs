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
#if TEST

namespace Loominate.Engine
{
    using System;
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;

    // used to create readers appropriate for specified xml fragment
    // The reader is pre-populated with appropriate namespaces.
    public static class XmlReaderFactory
    {
        internal static XmlReader CreateReader(string xml)
        {
            NameTable tbl = new NameTable();
            XmlNamespaceManager mgr = new XmlNamespaceManager(tbl);
            mgr.AddNamespace("gnc", Namespaces.GnuCash);
            mgr.AddNamespace("act", Namespaces.Account);
            mgr.AddNamespace("cmdty", Namespaces.Commodity);
            mgr.AddNamespace("slot", Namespaces.Slot);
            mgr.AddNamespace("trn", Namespaces.Transaction);
            mgr.AddNamespace("split", Namespaces.Split);
            XmlParserContext context = new XmlParserContext(tbl, mgr, null, XmlSpace.Default);

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            return XmlReader.Create(new StringReader(xml), settings, context);
        }

    }

}
#endif
