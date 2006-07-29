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
        
namespace MyCash.Engine
{
    using System.Collections.Generic;
    
    public class Account
    {
        // QofInstance inst;
        string accountName;
        string accountCode;
        string description;
        AccountType type;
        Commodity commodity;
        int commodityScu;
        bool nonStandardScu;

        AccountGroup parent;
        AccountGroup children;

        decimal startingBalance;
        decimal startingClearedBalance;
        decimal startingReconciledBalance;
        
        decimal balance;
        decimal clearedBalance;
        decimal reconciledBalance;
        
        int version;
        uint versionCheck;
        
        List<Split> splits;
        List<Lot> lots;
        
        //Policy policy;
        
        bool isBalanceDirty;
        bool isSortDirty;
        
        short mark;

    }
}
    
