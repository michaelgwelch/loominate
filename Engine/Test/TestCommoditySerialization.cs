
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
    using System.Text;
    using System.Xml.Serialization;
    using System.Xml;
    using NUnit.Framework;

    [TestFixture]
    public class TestCommoditySerialization
    {

        [Test]
        public void TestRead()
        {
            string xml = @"<gnc:commodity version='2.0.0'>
                    <cmdty:space>ISO4217</cmdty:space>
                    <cmdty:id>USD</cmdty:id>
                    <cmdty:name>US Dollar</cmdty:name>
                    <cmdty:xcode>840</cmdty:xcode>
                    <cmdty:fraction>100</cmdty:fraction>
                    <cmdty:get_quotes/>
                    <cmdty:quote_source>currency</cmdty:quote_source>
                    <cmdty:quote_tz/>
                </gnc:commodity>";
            XmlReader reader = XmlReaderFactory.CreateReader(xml);
            XmlSerializer s = new XmlSerializer(typeof(Commodity));
 
            Commodity c = (Commodity) s.Deserialize(reader);
            
            Assert.AreEqual("US Dollar", c.FullName);
            Assert.AreEqual("ISO4217", c.Namespace);
            Assert.AreEqual("USD", c.Mnemonic);
            Assert.AreEqual("840", c.Cusip);
            Assert.AreEqual(100, c.Fraction);

            StringBuilder bldr = new StringBuilder();
            XmlWriter writer = XmlWriterFactory.Create(bldr);
            XmlSerializerNamespaces nms = new XmlSerializerNamespaces();
            nms.Add("gnc", Namespaces.GnuCash);
            nms.Add("act", Namespaces.Account);
            nms.Add("cmdty", Namespaces.Commodity);
            s.Serialize(writer, c, nms);
            string newXml = bldr.ToString();
            Console.WriteLine(newXml);

        }
    }
}
#endif
