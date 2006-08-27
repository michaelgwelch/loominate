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
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;

    public class GnuCashFile
    {
        private const string ElementName = "gnc-v2";

        private Book[] books;

        public GnuCashFile(Book[] books)
        {
            this.books = books;
        }

        public void WriteXmlStream(Stream stream)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.Encoding = System.Text.Encoding.UTF8;
            XmlWriter writer = XmlWriter.Create(stream, settings);

            writer.WriteStartElement(ElementName);
            GnuCashXml.WriteNamespaces(writer);
            GnuCashXml.WriteCountData(writer, Namespaces.GnuCash,
                CountDataType.Book, books.Length);
            this.books[0].WriteXml(writer);

            writer.WriteEndElement();
            writer.Flush();

        }

        /// <summary>
        /// Creates a new instance of File from the specified stream.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static GnuCashFile ReadXmlStream(Stream stream)
        {
            XmlReader reader = new XmlTextReader(stream);
            reader.MoveToContent();
            reader.ReadStartElement(ElementName);

            int numOfBooks = GnuCashXml.ReadCountData(reader, CountDataType.Book);
            Book[] books = new Book[numOfBooks];

            for (int i = 0; i < numOfBooks; i++)
            {
                books[i] = Book.ReadXml(reader);
            }

            return new GnuCashFile(books);
        }
    }
}
