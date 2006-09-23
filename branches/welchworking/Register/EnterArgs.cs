namespace Loominate.Register
{
    /// <summary>
    /// </summary>
    public class  EnterArgs : BasicCellEventArgs
    {
        // An event handler should set this if the cell should allow
        // direct editing by the user.
        bool allowDirectEditing;    
        bool changeCursor;          // set to true to indicate that cursor and selection values are changed
        
        public EnterArgs(int curPos, int startSel,
                int endSel) : base(curPos, startSel, endSel)
        {
        }
                      
        public bool AllowDirectEditing 
        {
            get { return allowDirectEditing; }
            set { allowDirectEditing = value; }
        }
        
        public bool ChangeCursor
        {
            get { return this.changeCursor; }
            set { this.changeCursor = value; }
        }
        
    }
}
