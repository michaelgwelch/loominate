namespace Loominate.Register
{
    /* typedef void (*CellModifyVerifyFunc) (BasicCell *cell,
                                      const char *add_str,
                                      int add_str_len,
                                      const char *new_value,
                                      int new_value_len,
                                      int *cursor_position,
                                      int *start_selection,
                                      int *end_selection);
    */
    
    public class ModifyVerifyArgs : BasicCellEventArgs
    {
        string addStr;
        string newValue;

        public ModifyVerifyArgs(string addStr, string newValue,
                int curPos, int startSel, int endSel) :
            base(curPos, startSel, endSel)
        {
            this.addStr = addStr;
            this.newValue = newValue;
        }
    }
}
