namespace Loominate.Register
{
    /// <summary>
    /// The base class of the EventArg classes used in BasicCell events
    /// </summary>
    public abstract class BasicCellEventArgs : System.EventArgs
    {
        int cursorPosition;
        int startSelection;
        int endSelection;

        public int CursorPosition 
        {
            set
            {
                cursorPosition = value;
            }
            get
            {
                return cursorPosition;
            }
        }
        
        public int StartSelection
        {
            set
            {
                startSelection = value;
            }
            
            get
            {
                return startSelection;
            }
        }
        
        public int EndSelection
        {
            set
            {
                startSelection = value;
            }
            
            get
            {
                return EndSelection;
            }
        }

        protected BasicCellEventArgs(int curPos, int startSel, 
                int endSel)
        {
            this.cursorPosition = curPos;
            this.startSelection = startSel;
            this.endSelection = endSel;
        }
                      
    }
}
