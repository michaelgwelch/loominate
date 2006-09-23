

namespace Loominate.Register
{
	using System;

	
	public class NumberCell : BasicCell
	{
	    long nextNum;
	    bool nextNumSet;
	    
		public NumberCell()
		{
		}
		
		public override void ModifyVerify(string change, string newValue,
		                                    ref int cursorPos, int startSel, int endSel)
		{
		    bool accel = false, isNum;
		    long number;
		    char uc;
		    
		    // we are deleting or entering > 1 char then just accept change
		    if (change == null || change == String.Empty || change.Length > 1) 
		    {
		        base.Value = newValue;
		        return;
		    }

		    
		    isNum = long.TryParse(this.Value, out number);
		    if (isNum && (number < 0)) isNum = false;
		    
		    uc = change[0];
            switch (uc)
            {
                case '+':
                case '=':
                  number++;
                  accel = true;
                  break;

                case '_':
                case '-':
                  number--;
                  accel = true;
                  break;

                case '}':
                case ']':
                  number += 10;
                  accel = true;
                  break;

                case '{':
                case '[':
                  number -= 10;
                  accel = true;
                  break;
            }
            
            if (number < 0) number = 0;
		    
		    //  /* If there is already a non-number there, don't accelerate. */
		    if (accel && !isNum && this.Value != String.Empty) accel = false;
		    if (accel)
		    {
		        char[] buff = new char[128];
                if (!isNum) number = this.nextNum;
		        
		        if (number.ToString() == string.Empty)
		            return;
		        
		        base.Value = number.ToString();

                cursorPos = -1;

                return;
		    }
		    
            base.Value = newValue;
		}
		
	}
}
