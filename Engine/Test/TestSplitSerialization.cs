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
    using System.Xml;
    using NUnit.Framework;

    [TestFixture]
    public class TestSplitSerialization
    {
        private string xml = @"<trn:split>
            <split:id type='guid'>9e13d3013f49da698a791279cf874c20</split:id>
            <split:reconciled-state>n</split:reconciled-state>
            <split:value>30000/100</split:value>
            <split:quantity>30000/100</split:quantity>
            <split:account type='guid'>4c49b481451e5ac3e14882af8053fffd</split:account>
        </trn:split>";

        [Test]
        public void TestOne()
        {
            XmlReader reader = XmlReaderFactory.CreateReader(xml);
            Split s = Split.ReadXml(reader);

            Assert.IsNotNull(s);
            Assert.AreEqual(new Guid("9e13d3013f49da698a791279cf874c20"), s.Id, "check Id");
            Assert.AreEqual(ReconcileState.NotReconciled, s.ReconcileState, "check reconcile state");
            Assert.AreEqual(new decimal(300), s.Value, "check value");
            Assert.AreEqual(new decimal(300), s.Quantity, "check quantity");
            Assert.AreEqual(new Guid("4c49b481451e5ac3e14882af8053fffd"), s.AccountId, "check account id");
        }

    }
}
#endif