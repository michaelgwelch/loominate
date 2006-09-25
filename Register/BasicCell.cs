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


namespace Loominate.Register
{
    using System;
    public abstract class BasicCell
    {
       
        string name;
        string value;
        
        bool changed; // true if value modified
        bool conditionallyChanged;  // true if value modified conditionally

        public virtual string Value 
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
            
        }

        public virtual bool EnterCell(ref int cursorPos, 
            ref int startSel, ref int endSel)
        {
            return false;
        }

        public virtual void LeaveCell() { }

        public virtual void ModifyVerify(string change, string newValue,
                                         ref int cursorPos, int startSel, int endSel)
        {
        }

//        public event EventHandler DirectUpdate;
//        public event EventHandler LeaveCell;
//
//        private event EventHandler GuiRealize;
//        private event EventHandler GuiMove;
//        private event EventHandler GuiDestroy;

        private string sampleText; // sample text for sizing purposes
        private Alignment alignment; // horizontal alignment in column
        bool expandable;                 // can file with extra spaces
        bool span;                       // can span multiple columns
        //bool isPopup;                    // is a popup widget
        
        
        protected BasicCell()
        {
            this.value = string.Empty;        
        }
        
        private void Clear()
        {
            /*
              g_free (cell->cell_name);
  cell->cell_name = NULL;

  cell->changed = FALSE;
  cell->conditionally_changed = FALSE;

  cell->value = NULL;
  cell->value_chars = 0;

  cell->set_value = NULL;
  cell->enter_cell = NULL;
  cell->modify_verify = NULL;
  cell->direct_update = NULL;
  cell->leave_cell = NULL;
  cell->gui_realize = NULL;
  cell->gui_move = NULL;
  cell->gui_destroy = NULL;
            */
            this.name = null;
            this.changed = false;
            this.conditionallyChanged = false;
            this.value = null;
            
            
        }
        
        
        public string Name
        {
            set
            {
                this.name = value;
            }
        }
        
        public bool HasName(string name)
        {
            if (this.name == null) return false;
            return (this.name == name);
        }
        
        public string SetSampleText
        {
            set
            {
                this.sampleText = value;
            }
        }
        
        public Alignment Alignment
        {
            set
            {
                this.alignment = value;
            }
        }
        
        public bool Expandable
        {
            set
            {
                this.expandable = value;
            }
        }
        
        public bool Span
        {
            set
            {
                this.span = value;
            }
        }
        
        public bool Changed
        {
            get
            {
                return this.changed;
            }
            set
            {
                this.changed = value;
            }
        }
        
        public bool ConditionallyChanged
        {
            get
            {
                return this.conditionallyChanged;
            }
            set
            {
                this.conditionallyChanged = value;
            }
        }
        
             
        
        
    }

}
