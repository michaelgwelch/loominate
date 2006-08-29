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
    internal class NameSpace
    {
        private const string gnuCash="http://www.gnucash.org/XML/gnc";
        private const string account = "http://www.gnucash.org/XML/act";
        private const string book = "http://www.gnucash.org/XML/book";
        private const string countData = "http://www.gnucash.org/XML/cd";
        private const string commodity = "http://www.gnucash.org/XML/cmdty";
        private const string price = "http://www.gnucash.org/XML/price";
        private const string slot = "http://www.gnucash.org/XML/slot";
        private const string split = "http://www.gnucash.org/XML/split";
        private const string scheduledTransaction = "http://www.gnucash.org/XML/sx";
        private const string transaction = "http://www.gnucash.org/XML/trn";
        private const string timestamp = "http://www.gnucash.org/XML/ts";

        public static NameSpace GnuCash = new NameSpace(gnuCash);
        public static NameSpace Account = new NameSpace(account);
        public static NameSpace Book = new NameSpace(book);
        public static NameSpace CountData = new NameSpace(countData);
        public static NameSpace Commodity = new NameSpace(commodity);
        public static NameSpace Price = new NameSpace(price);
        public static NameSpace Slot = new NameSpace(slot);
        public static NameSpace Split = new NameSpace(split);
        public static NameSpace ScheduledTransaction = new NameSpace(scheduledTransaction);
        public static NameSpace Transaction = new NameSpace(transaction);
        public static NameSpace Timestamp = new NameSpace(timestamp);

        private string val;

        private NameSpace(string ns)
        {
            val = ns;
        }

        public static implicit operator string(NameSpace ns)
        {
            return ns.val;
        }

        public override string ToString()
        {
            return val;
        }


/* 
 *      xmlns:gnc="http://www.gnucash.org/XML/gnc"
     xmlns:act="http://www.gnucash.org/XML/act"
     xmlns:book="http://www.gnucash.org/XML/book"
     xmlns:cd="http://www.gnucash.org/XML/cd"
     xmlns:cmdty="http://www.gnucash.org/XML/cmdty"
     xmlns:price="http://www.gnucash.org/XML/price"
     xmlns:slot="http://www.gnucash.org/XML/slot"
     xmlns:split="http://www.gnucash.org/XML/split"
     xmlns:sx="http://www.gnucash.org/XML/sx"
     xmlns:trn="http://www.gnucash.org/XML/trn"
     xmlns:ts="http://www.gnucash.org/XML/ts"
     xmlns:fs="http://www.gnucash.org/XML/fs"
     xmlns:bgt="http://www.gnucash.org/XML/bgt"
     xmlns:recurrence="http://www.gnucash.org/XML/recurrence"
     xmlns:lot="http://www.gnucash.org/XML/lot"
     xmlns:cust="http://www.gnucash.org/XML/cust"
     xmlns:job="http://www.gnucash.org/XML/job"
     xmlns:addr="http://www.gnucash.org/XML/addr"
     xmlns:owner="http://www.gnucash.org/XML/owner"
     xmlns:taxtable="http://www.gnucash.org/XML/taxtable"
     xmlns:tte="http://www.gnucash.org/XML/tte"
     xmlns:employee="http://www.gnucash.org/XML/employee"
     xmlns:order="http://www.gnucash.org/XML/order"
     xmlns:billterm="http://www.gnucash.org/XML/billterm"
     xmlns:bt-days="http://www.gnucash.org/XML/bt-days"
     xmlns:bt-prox="http://www.gnucash.org/XML/bt-prox"
     xmlns:invoice="http://www.gnucash.org/XML/invoice"
     xmlns:entry="http://www.gnucash.org/XML/entry"
     xmlns:vendor="http://www.gnucash.org/XML/vendor">
 **/
    }
}
