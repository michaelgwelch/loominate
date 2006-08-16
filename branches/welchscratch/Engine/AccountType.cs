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
    public enum AccountType
    {
        BadType     = -1,
        NoType      = -1,
        Bank        = 0,    // Savings or checking account, often interest bearing   
        Cash        = 1,    // Cash on hand
        Asset       = 2,    // Generic generalized account for assets (other than bank and cash)
        Credit      = 3,    // Credit (e.g. amex) and debit accounts (e.g. visa, mastercard)
        Liability   = 4,    // Generic generalized account for liabilities
        Stock       = 5,    // Stocks, generally shown with three columns: price, shares, value
        Mutual      = 6,    // Mutual fund, three columns like stocks
// deprecated        Currency    = 7,    // Like a stock account. used for trading currency. 
        Income      = 8,    
        Expense     = 9,
        Equity      = 10,   // Used to balance the balance sheet
        Receivable  = 11,   // Accounts receivable
        Payable     = 12,   // Accounts payable
    }
}
