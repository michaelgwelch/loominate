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
    using System.Xml.Serialization;
    using System.Xml;
    using NUnit.Framework;

    [TestFixture]
    public class TestCommoditySerialization
    {

        [Test]
        public void TestRead()
        {
//            string xml = @" <cmdty:space>ISO4217</cmdty:space> 
//				<cmdty:fraction>10000</cmdty:fraction> 
//				<cmdty:name>US Dollar</cmdty:name> 
//				<cmdty:id>USD</cmdty:id> 
//				<cmdty:xcode>849</cmdty:xcode>";

            //XmlReader reader = XmlReaderFactory.CreateReader(xml);

            XmlSerializerNamespaces nms = new XmlSerializerNamespaces();
            nms.Add("cmdty", "http://www.gnucash.org/XML/cmdty");
            nms.Add("gnc", "http://www.gnucash.org/XML/gnc");

            XmlSerializer s = new XmlSerializer(typeof(Commodity));

            Commodity c = new Commodity(null, "US Dollar", "CURRENCY", "USD", "ISO$###", 100);
            StringWriter w = new StringWriter();

            s.Serialize(w, c, nms);
            string str = w.ToString();

            System.Diagnostics.Debug.WriteLine(str);

            //c.ReadXml(reader);
            Assert.AreEqual("ISO4217", c.Namespace, "check namespace");
            Assert.AreEqual("US Dollar", c.FullName, "check name");
            Assert.AreEqual(10000, c.Fraction, "check fraction");
        }
    }
}
#endif
