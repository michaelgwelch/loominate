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
using System;

namespace Loominate.Engine
{
    
    public class Commodity
    {
        private string fullName;
        private string nameSpace;
        private string mnemonic;
        private string cusip;
        private string uniqueName;
        private int fraction;
        
        public Commodity(Object book, string fullName, string nameSpace,
            string mnemonic, string cusip, int fraction)
        {
            this.fullName = fullName;
            this.nameSpace = nameSpace;
            this.mnemonic = mnemonic;
            this.cusip = cusip;
            this.uniqueName = nameSpace + "::" + mnemonic;
            this.fraction = fraction;
        }
        
        public string FullName
        {
            get
            {
                return fullName;
            }
            set
            {
                fullName = value;
            }
        }
        
        public string Namespace
        {
            get
            {
                return nameSpace;
            }
            set
            {
                nameSpace = value;
            }
        }
        
        public string Mnemonic
        {
            get
            {
                return mnemonic;
            }
            set
            {
                mnemonic = value;
            }
        }
        
        public string Cusip
        {
            get
            {
                return cusip;
            }
            set
            {
                cusip = value;
            }
        }
        
        public string UniqueName
        {
            get
            {
                return uniqueName;
            }
        }
        
        public int Fraction
        {
            get
            {
                return fraction;
            }
            set
            {
                fraction = value;
            }
        }
        
        public override bool Equals(object other)
        {
            Commodity c = other as Commodity;           
            if (c == null) return false;
            
            return (this.nameSpace == c.nameSpace
                       && this.mnemonic == c.mnemonic);
        }
        
        public override int GetHashCode()
        {
            return uniqueName.GetHashCode();
        }
            
        
    }
    
}
