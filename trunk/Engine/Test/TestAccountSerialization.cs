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
    using System.Text;
    using NUnit.Framework;

    [TestFixture()]
    class TestAccountSerialization
    {
        const string sampleXml="<gnc:account version=\"2.0.0\">" +
            "<act:name>Assets</act:name>" +
            "<act:id type=\"guid\">0739c8984c8d9a604cf8784907d5232d</act:id>" +
            "<act:type>ASSET</act:type>" +
            "<act:commodity>" +
                "<cmdty:space>ISO4217</cmdty:space>" +
                "<cmdty:id>USD</cmdty:id>" +
            "</act:commodity>" +
            "<act:commodity-scu>100</act:commodity-scu>" +
            "<act:description>Assets</act:description>" +
            "<act:slots>" +
                "<slot>" +
                    "<slot:key>placeholder</slot:key>" +
                    "<slot:value type=\"string\">true</slot:value>" +
                "</slot>" +
            "</act:slots>" +
            "</gnc:account>";
        
        public void TestOne()
        {

        }

    }
}

#endif