/*******************************************************************************
    Copyright 2006 Josh Edler
    
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

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Loominate.Gooey.Controls.Register
{
    /// <summary>
    /// Description of RegisterControl.
    /// </summary>
    public partial class RegisterControl : UserControl
    {
        internal const byte HEADERS = 7;
        internal const byte DATE_HEADER = 0;
        internal const byte NUM_HEADER = 1;
        internal const byte PAYEE_CATEGORY_MEMO_HEADER = 2;
        internal const byte PAYMENT_HEADER = 3;
        internal const byte CLR_HEADER = 4;
        internal const byte DEPOSIT_HEADER = 5;
        internal const byte BALANCE_HEADER = 6;

        private HeaderInfo[] headers = new HeaderInfo[HEADERS];

        public RegisterControl()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();

            this.Dock = DockStyle.Fill;

            for (int i = 0; i < HEADERS; i++)
                headers[i] = new HeaderInfo();

            this.SizeChanged += new EventHandler(RegisterControl_SizeChanged);
            contentList.DrawItem += new DrawItemEventHandler(contentList_DrawItem);
            contentList.MeasureItem += new MeasureItemEventHandler(contentList_MeasureItem);
        }

        void contentList_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = contentList.ItemHeight;
            e.ItemWidth = contentList.Width;

            e.ItemHeight = 0;
            e.ItemWidth = 0;
        }

        void contentList_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index > -1)
            {
                RegisterEntry entry = (RegisterEntry)contentList.Items[e.Index];

                /*
                System.Diagnostics.Trace.WriteLine(
                    String.Format("{0,4} : X:{1,04} Y:{2,04} W:{3,04} H:{4,04} {5}", 
                    e.Index, 
                    e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height, 
                    e.State)
                    //selected ? "SELECTED" : String.Empty)
                );
                */

                entry.Draw(e.Bounds, e.Graphics, e.State);
            }
        }

        void RegisterControl_SizeChanged(object sender, EventArgs e)
        {
            lock (headers.SyncRoot)
            {
                SetHeaderByControl(headers[DATE_HEADER], lblDate);
                SetHeaderByControl(headers[NUM_HEADER], lblNum);
                SetHeaderByControl(headers[PAYEE_CATEGORY_MEMO_HEADER], lblPayeeCategoryMemo);
                SetHeaderByControl(headers[PAYMENT_HEADER], lblPayment);
                SetHeaderByControl(headers[CLR_HEADER], lblClr);
                SetHeaderByControl(headers[DEPOSIT_HEADER], lblDeposit);
                SetHeaderByControl(headers[BALANCE_HEADER], lblBalance);
            }
        }

        protected override void InitLayout()
        {
            base.InitLayout();

            // stress test:
            for (int i = 0; i < 100; i++)
                this.contentList.Items.Add(new RegisterEntry(this, headers));
        }

        private void SetHeaderByControl(HeaderInfo h, Control c)
        {
            h.x = c.Location.X;
            h.y = c.Location.Y;

            h.width = c.Width;
            h.height = c.Height;
        }

    }
}
