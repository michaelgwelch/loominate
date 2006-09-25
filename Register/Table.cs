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
    using System.Collections.Generic;

    /// <summary>
    /// Represents a two-dimensional table
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Table<T>
    {
        List<T> array;
        int rows;
        int cols;

        public Table()
        {
            array = new List<T>();
        }

        public T this[int row, int col]
        {
            get
            {
                if ((row < 0) || (col < 0)) throw new ArgumentOutOfRangeException();
                if ((row >= this.rows) || (col >= this.cols)) throw new ArgumentOutOfRangeException();

                int index = ((row * this.cols) + col);

                return this.array[index];
            }

            set
            {
                if ((row < 0) || (col < 0)) throw new ArgumentOutOfRangeException();
                if ((row >= this.rows) || (col >= this.cols)) throw new ArgumentOutOfRangeException();

                int index = ((row * this.cols) + col);

                this.array[index] = value;
            }
        }

        public void Resize(int rows, int cols)
        {
            if ((rows < 0) || (cols < 0)) throw new ArgumentNullException();

            int oldLength = this.array.Count;
            int newLength = rows * cols;
            if (oldLength == newLength) return;

            // if shrinking get rid of "old" cells so capacity can be 
            // reduced
            if (newLength < oldLength)
            {
                this.array.RemoveRange(newLength, oldLength - newLength);
            }

            // Set capacity
            this.array.Capacity = newLength;

            this.rows = rows;
            this.cols = cols;
        }

        public int RowCount { get { return this.rows; } }
        public int ColumnCount { get { return this.cols; } }
    }
}
