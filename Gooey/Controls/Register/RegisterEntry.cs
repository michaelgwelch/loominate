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

// Enable DRAW_BOUNDING_BOX to get different-colored boxes to be drawn around
// each value box.  Mostly useful for bounds checking purposes.
// #define DRAW_BOUNDING_BOX

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Loominate.Gooey.Controls.Register
{
    /// <summary>
    /// Description of RegisterEntry.
    /// </summary>
    internal class RegisterEntry
    {
        private HeaderInfo[] headers;

        private int fullHeight;
        private int halfHeight;
        private Font entry;

        internal RegisterEntry(Control parent, HeaderInfo[] headers)
        {
            this.headers = headers;

            entry = new Font(parent.Font, FontStyle.Italic);
        }

        internal void Draw(Rectangle bounds, Graphics g, DrawItemState item)
        {
            Rectangle myRect = bounds;

            fullHeight = bounds.Height;
            halfHeight = bounds.Height / 2;

            g.FillRectangle(new SolidBrush(Color.LightGray), myRect);

            myRect.Height = halfHeight;
            myRect.Y += halfHeight;
                        
            g.FillRectangle(new SolidBrush(Color.Gainsboro), myRect);

            myRect = bounds;

            lock (headers.SyncRoot)
            {
                DrawEntryDate(g, myRect, item);
                DrawEntryNum(g, myRect, item);
                DrawEntryPayeeCategoryMemo(g, myRect, item);
                DrawEntryPayment(g, myRect, item);
                DrawEntryClr(g, myRect, item);
                DrawEntryDeposit(g, myRect, item);
                DrawEntryBalance(g, myRect, item);
            }

            if ((item & DrawItemState.Selected) == DrawItemState.Selected)
            {
                Rectangle box = new Rectangle(
                    myRect.X + 1,
                    myRect.Y + 1,
                    myRect.Width - 2,
                    myRect.Height - 2
                    );

                g.DrawRectangle(new Pen(Color.Blue, 2.0f), box);
            }
        }

        #region DrawEntry methods (useful for the selected row)
        private void DrawEntryDate(Graphics g, Rectangle bounds, DrawItemState item)
        {
            Rectangle rect = new Rectangle(
                headers[RegisterControl.DATE_HEADER].x,
                bounds.Y,
                headers[RegisterControl.DATE_HEADER].width,
                halfHeight
                );

            DrawValueBox(g, rect, item, DrawEdgeState.FullEdge, "Date");
        }

        private void DrawEntryNum(Graphics g, Rectangle bounds, DrawItemState item)
        {
            Rectangle rect = new Rectangle(
                headers[RegisterControl.NUM_HEADER].x,
                bounds.Y,
                headers[RegisterControl.NUM_HEADER].width,
                halfHeight
                );

            DrawValueBox(g, rect, item, DrawEdgeState.FullEdge, "Num");
        }

        private void DrawEntryPayeeCategoryMemo(Graphics g, Rectangle bounds, DrawItemState item)
        {
            int width = headers[RegisterControl.PAYEE_CATEGORY_MEMO_HEADER].width;
            int leftWidth = width / 2 - 1; 
            int rightWidth = width - leftWidth - 1;

            Rectangle payeeRect = new Rectangle(
                headers[RegisterControl.PAYEE_CATEGORY_MEMO_HEADER].x,
                bounds.Y,
                headers[RegisterControl.PAYEE_CATEGORY_MEMO_HEADER].width,
                halfHeight
                );

            Rectangle categoryRect = new Rectangle(
                headers[RegisterControl.PAYEE_CATEGORY_MEMO_HEADER].x,
                bounds.Y + halfHeight,
                leftWidth,
                halfHeight
                );

            Rectangle memoRect = new Rectangle(
                headers[RegisterControl.PAYEE_CATEGORY_MEMO_HEADER].x + leftWidth + 1, 
                bounds.Y + halfHeight,
                rightWidth,
                halfHeight
                );

            DrawValueBox(g, payeeRect, item, DrawEdgeState.FullEdge, "Payee");
            DrawValueBox(g, categoryRect, item, DrawEdgeState.HalfEdgeIfSelected, "Category");
            DrawValueBox(g, memoRect, item, DrawEdgeState.NoEdge, "Memo");
        }

        private void DrawEntryPayment(Graphics g, Rectangle bounds, DrawItemState item)
        {
            Rectangle rect = new Rectangle(
                headers[RegisterControl.PAYMENT_HEADER].x,
                bounds.Y,
                headers[RegisterControl.PAYMENT_HEADER].width,
                halfHeight
                );

            DrawValueCurrency(g, rect, item, "Payment", String.Empty);
        }

        private void DrawEntryClr(Graphics g, Rectangle bounds, DrawItemState item)
        {
            Rectangle rect = new Rectangle(
                headers[RegisterControl.CLR_HEADER].x,
                bounds.Y,
                headers[RegisterControl.CLR_HEADER].width,
                halfHeight
                );

            DrawValueBox(g, rect, item, DrawEdgeState.FullEdge, String.Empty);
        }

        private void DrawEntryDeposit(Graphics g, Rectangle bounds, DrawItemState item)
        {
            Rectangle rect = new Rectangle(
                headers[RegisterControl.DEPOSIT_HEADER].x,
                bounds.Y,
                headers[RegisterControl.DEPOSIT_HEADER].width,
                halfHeight
                );

            DrawValueCurrency(g, rect, item, "Deposit", String.Empty);
        }

        private void DrawEntryBalance(Graphics g, Rectangle bounds, DrawItemState item)
        {
            Rectangle rect = new Rectangle(
                headers[RegisterControl.BALANCE_HEADER].x,
                bounds.Y,
                headers[RegisterControl.BALANCE_HEADER].width,
                halfHeight
                );

            DrawValueCurrency(g, rect, item, String.Empty, String.Empty);
        }
        #endregion

        #region Base DrawValueBox method
        private void DrawValueBox(Graphics g, Rectangle bounds, DrawItemState item, DrawEdgeState edge, string value)
        {
            Rectangle box = new Rectangle(bounds.X, bounds.Y, bounds.Width - 1, bounds.Height);

#if DRAW_BOUNDING_BOX
            Color c;

            int x = bounds.X;

            if (x >= headers[RegisterControl.DATE_HEADER].x && x < headers[RegisterControl.NUM_HEADER].x)
                c = Color.Orange;
            else if (x >= headers[RegisterControl.NUM_HEADER].x && x < headers[RegisterControl.PAYEE_CATEGORY_MEMO_HEADER].x)
                c = Color.Red;
            else if (x >= headers[RegisterControl.PAYEE_CATEGORY_MEMO_HEADER].x && x < headers[RegisterControl.PAYMENT_HEADER].x)
                c = Color.DarkViolet;
            else if (x >= headers[RegisterControl.PAYMENT_HEADER].x && x < headers[RegisterControl.CLR_HEADER].x)
                c = Color.LimeGreen;
            else if (x >= headers[RegisterControl.CLR_HEADER].x && x < headers[RegisterControl.DEPOSIT_HEADER].x)
                c = Color.DarkGoldenrod;
            else if (x >= headers[RegisterControl.DEPOSIT_HEADER].x && x < headers[RegisterControl.BALANCE_HEADER].x)
                c = Color.Cyan;
            else
                c = Color.Yellow;

            g.DrawRectangle(new Pen(c), box);
#endif

            if (edge == DrawEdgeState.FullEdge)
            {
                // looks best with only the right edge drawn
                g.DrawLine(
                           new Pen(Color.WhiteSmoke),
                           bounds.X + bounds.Width,
                           bounds.Y,
                           bounds.X + bounds.Width,
                           bounds.Y + fullHeight - 1
                          );
            }
            else if (edge == DrawEdgeState.HalfEdgeIfSelected && (item & DrawItemState.Selected) == DrawItemState.Selected)
            {
                // looks best with only the right edge drawn
                g.DrawLine(
                           new Pen(Color.WhiteSmoke),
                           bounds.X + bounds.Width,
                           bounds.Y,
                           bounds.X + bounds.Width,
                           bounds.Y + halfHeight - 1
                          );
            }

            g.DrawString(
                         value,
                         entry,
                         new SolidBrush(Color.Gray),
                         new PointF(bounds.X, bounds.Y)
                        );
        }
#endregion

        #region DrawValue methos (useful when actual values are available)
        private void DrawValueCurrency(Graphics g, Rectangle bounds, DrawItemState state, string wholeVal, string changeVal)
        {
            int changeWidth = 20;
            int wholeWidth = bounds.Width - changeWidth - 1;

            Rectangle wholeRect = new Rectangle(bounds.X, bounds.Y, wholeWidth, bounds.Height);
            Rectangle changeRect = new Rectangle(bounds.X + wholeWidth + 1, bounds.Y, changeWidth, bounds.Height);

            DrawValueBox(g, wholeRect, state, DrawEdgeState.FullEdge, wholeVal);
            DrawValueBox(g, changeRect, state, DrawEdgeState.FullEdge, changeVal);
        }
        #endregion
    }
}
