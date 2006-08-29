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
    using System.Collections.Generic;
    using System.Xml;
    using NUnit.Framework;

    [TestFixture]
    public class TestTransaction
    {
        string xml=@"<gnc:transaction version='2.0.0'>
              <trn:id type='guid'>5eed80e716398d839ab58d47aa2e2939</trn:id>
              <trn:currency>
                <cmdty:space>ISO4217</cmdty:space>
                <cmdty:id>USD</cmdty:id>
              </trn:currency>
              <trn:date-posted>
                <ts:date>2006-07-28 00:00:00 -0500</ts:date>
              </trn:date-posted>
              <trn:date-entered>
                <ts:date>2006-07-28 22:17:39 -0500</ts:date>
              </trn:date-entered>
              <trn:description>opening balance</trn:description>
              <trn:splits>
                <trn:split>
                  <split:id type='guid'>9e13d3013f49da698a791279cf874c20</split:id>
                  <split:reconciled-state>n</split:reconciled-state>
                  <split:value>30000/100</split:value>
                  <split:quantity>30000/100</split:quantity>
                  <split:account type='guid'>4c49b481451e5ac3e14882af8053fffd</split:account>
                </trn:split>
                <trn:split>
                  <split:id type='guid'>2b0b9e0f6cb87fe58abba15534a36a32</split:id>
                  <split:reconciled-state>n</split:reconciled-state>
                  <split:value>-30000/100</split:value>
                  <split:quantity>-30000/100</split:quantity>
                  <split:account type='guid'>9664c37fe7ead82287f1e918837ee0f0</split:account>
                </trn:split>
              </trn:splits>
            </gnc:transaction>";

        [Test]
        public void TestOne()
        {
            XmlGnuCashReader reader = XmlReaderFactory.CreateReader(xml);
            Dictionary<string, Commodity> commodities = null;

            Commodity c = new Commodity("US Dollar", "ISO4217", "USD", "840", 100, String.Empty, "currency", String.Empty);
            commodities[c.UniqueName] = c;
            Transaction t = Transaction.ReadXml(reader, commodities);

            Assert.IsNotNull(t, "check transaction");
            Assert.AreEqual(new Guid("5eed80e716398d839ab58d47aa2e2939"), t.Id, "check id");
            Assert.IsNotNull(c, "check a");
            Assert.IsNotNull(t, "check b");
            Assert.IsNotNull(t.Commodity, "check c");
            Assert.AreEqual(c, t.Commodity, "check commodity");
        }

    }
} 

#endif

