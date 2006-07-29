/*******************************************************************************
    Copyright 2006 Michael Welch
    
    This file is part of MyCash.

    MyCash is free software; you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation; either version 2 of the License, or
    (at your option) any later version.

    MyCash is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with MyCash; if not, write to the Free Software
    Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
*******************************************************************************/
#if NUNIT
using System;
using NUnit.Framework;

namespace MyCash.Engine
{
    // This test class is based on code from the gnucash project.
    // You can refer to src/engine/test/test-commodities.c in that project.
    [TestFixture]
    public class TestCommodity
    {
        private Random rand = new Random();
        
        [Test]
        public void TestConstructor()
        {
            Object book = null;
            String fullName = "US Dollar";
            String nameSpace = "Currency";
            String mnemonic = "USD";
            String cusip = "USD";
            int fraction = 100;
            Commodity c = new Commodity(book, fullName, nameSpace, mnemonic,
                cusip, fraction);
                
            String expected = nameSpace + "::" + mnemonic;
            String actual = c.UniqueName;
            Assert.AreEqual(expected, actual, "UniqueName is not correct");
            
            Assert.AreEqual(fullName, c.FullName);
            Assert.AreEqual(nameSpace, c.Namespace);
            Assert.AreEqual(mnemonic, c.Mnemonic);
            Assert.AreEqual(cusip, c.Cusip);
            Assert.AreEqual(fraction, c.Fraction);
            
            fullName = "US Dollars";
          //  c.FullName = fullName;
        //    Assert.AreEqual(fullName, c.FullName);
            
        }
        
        [Test]
        public void TestCommodityFromGnuCash()
        {
            string fullName = GetRandom.String();
            string nameSpace = GetRandom.CommodityNamespace();
            string mnemonic = GetRandom.String();
            string cusip = GetRandom.String();
            int fraction = rand.Next(0, 100001);
            
            Commodity c = new Commodity(null, fullName, nameSpace,
                                              mnemonic, cusip, fraction);
            Assert.AreEqual(fullName, c.FullName, "fullname equal test");
            Assert.AreEqual(nameSpace, c.Namespace, "namespace equal test");
            Assert.AreEqual(mnemonic, c.Mnemonic, "mnemonic equal test");
            Assert.AreEqual(cusip, c.Cusip, "cusip equal test");
            Assert.AreEqual(fraction, c.Fraction, "fractioin equal test");
            
            fullName = GetRandom.String();
            c.FullName = fullName;
            Assert.AreEqual(fullName, c.FullName, "reset fullname equal test");
            
            nameSpace = GetRandom.CommodityNamespace();
            c.Namespace = nameSpace;
            Assert.AreEqual(nameSpace, c.Namespace, "reset namespace equal test");
            
            mnemonic = GetRandom.String();
            c.Mnemonic = mnemonic;
            Assert.AreEqual(mnemonic, c.Mnemonic, "reset mnemonic equal test");
            
            cusip = GetRandom.String();
            c.Cusip = cusip;
            Assert.AreEqual(cusip, c.Cusip, "reset cusip equal test");
            
            fraction = rand.Next(0, 10001);
            c.Fraction = fraction;
            Assert.AreEqual(fraction, c.Fraction, "reset fraction equal test");
       
            
        }
        
            
        
        [Test]
        public void TestEquality()
        {
            Commodity c1 = new Commodity(null, "US Dollar", "Currency",
                                            "USD", "cusip1", 100);
            Commodity c2 = new Commodity(null, "US Doll", "Currency",
                                            "USD", "cusip2", 105);
            
            Assert.AreEqual(c1, c2, "Commodities are equal if same" +
                               " namespace and mnemonic");

        }
    }
    
    
}
#endif
