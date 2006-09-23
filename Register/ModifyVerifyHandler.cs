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
    public delegate void ModifyVerifyHandler(object sender,
            ModifyVerifyArgs args);
}
