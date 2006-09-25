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
    /* 
     * typedef struct
{
  short num_rows;
  short num_cols;

  short start_col;
  short stop_col;

  char *cursor_name;

  GPtrArray *cells; 
} CellBlock;
*/
    public class CellBlock
    {
        short numRows;
        short numCols;

        string cursorName;
        BasicCell[] cells;

        public CellBlock(short rows, short cols, string cursorName)
        {
            if (rows <= 0) throw new ArgumentOutOfRangeException("rows");
            if (cols <= 0) throw new ArgumentOutOfRangeException("cols");
            if (cursorName == null) throw new ArgumentNullException("cursorName");

            this.Init(rows, cols);
            this.cursorName = cursorName;
        }

        private void Init(short rows, short cols)
        {
            this.numRows = rows;
            this.numCols = cols;

            this.cells = new BasicCell[rows * cols];
        }

        public BasicCell this[short row, short col]
        {
            get
            {
                AssertArrayBounds(row, col);
                return this.cells[CalculateIndex(row, col)];
            }
            set
            {
                AssertArrayBounds(row, col);
                this.cells[CalculateIndex(row, col)] = value;
            }
        }

        public int GetNumberOfChangedCells(bool includeConditional)
        {
            short changed = 0;

            foreach (BasicCell cell in this.cells)
            {

                if (cell == null) continue;
                if (cell.Changed) changed++;
                if (includeConditional && cell.ConditionallyChanged) changed++;
            }

            return changed;
        }

        public void ClearChanges()
        {
            foreach (BasicCell cell in this.cells)
            {
                cell.Changed = false;
            }
        }

        private int CalculateIndex(short row, short col)
        {
            return (row * this.numCols) + col;
        }

        private void AssertArrayBounds(short row, short col)
        {
            if (row < 0 || row >= this.numRows) throw new ArgumentOutOfRangeException("row");
            if (col < 0 || col >= this.numCols) throw new ArgumentOutOfRangeException("col");
        }

    }
}
