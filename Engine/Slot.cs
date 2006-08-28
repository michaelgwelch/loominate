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
    using System.Xml.Serialization;

    public class Slot : IComparable<Slot>
    {
        string key;
        object value;
        string type;

        public Slot(string key, object value, string type)
        {
            this.key = key;
            this.type = type;
            this.value = value;
        }

        public string Key
        {
            get { return this.key; }
        }

        public string Type
        {
            get { return this.type; }
        }

        public object Value
        {
            get { return this.value; }
        }


        #region IComparable<Slot> Members

        public int CompareTo(Slot other)
        {
            return this.key.CompareTo(other.key);
        }

        #endregion
    }
}
