namespace Loominate.Gooey.Controls.Register
{
    partial class RegisterControl : System.Windows.Forms.UserControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.headerPanel = new System.Windows.Forms.Panel();
            this.lblEnd = new System.Windows.Forms.Label();
            this.lblDeposit = new System.Windows.Forms.Label();
            this.lblClr = new System.Windows.Forms.Label();
            this.lblPayment = new System.Windows.Forms.Label();
            this.lblPayeeCategoryMemo = new System.Windows.Forms.Label();
            this.lblNum = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.contentList = new System.Windows.Forms.ListBox();
            this.lblBalance = new System.Windows.Forms.Label();
            this.headerPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // headerPanel
            // 
            this.headerPanel.BackColor = System.Drawing.Color.WhiteSmoke;
            this.headerPanel.Controls.Add(this.lblEnd);
            this.headerPanel.Controls.Add(this.lblDeposit);
            this.headerPanel.Controls.Add(this.lblClr);
            this.headerPanel.Controls.Add(this.lblPayment);
            this.headerPanel.Controls.Add(this.lblPayeeCategoryMemo);
            this.headerPanel.Controls.Add(this.lblNum);
            this.headerPanel.Controls.Add(this.lblDate);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.Location = new System.Drawing.Point(0, 0);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(797, 15);
            this.headerPanel.TabIndex = 0;
            // 
            // lblEnd
            // 
            this.lblEnd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblEnd.BackColor = System.Drawing.Color.Silver;
            this.lblEnd.Location = new System.Drawing.Point(777, 0);
            this.lblEnd.Name = "lblEnd";
            this.lblEnd.Size = new System.Drawing.Size(20, 15);
            this.lblEnd.TabIndex = 6;
            // 
            // lblDeposit
            // 
            this.lblDeposit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDeposit.BackColor = System.Drawing.Color.Silver;
            this.lblDeposit.Location = new System.Drawing.Point(635, 0);
            this.lblDeposit.Name = "lblDeposit";
            this.lblDeposit.Size = new System.Drawing.Size(70, 15);
            this.lblDeposit.TabIndex = 5;
            this.lblDeposit.Text = "Deposit";
            // 
            // lblClr
            // 
            this.lblClr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblClr.BackColor = System.Drawing.Color.Silver;
            this.lblClr.Location = new System.Drawing.Point(614, 0);
            this.lblClr.Name = "lblClr";
            this.lblClr.Size = new System.Drawing.Size(20, 15);
            this.lblClr.TabIndex = 4;
            this.lblClr.Text = "Clr";
            // 
            // lblPayment
            // 
            this.lblPayment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPayment.BackColor = System.Drawing.Color.Silver;
            this.lblPayment.Location = new System.Drawing.Point(543, 0);
            this.lblPayment.Name = "lblPayment";
            this.lblPayment.Size = new System.Drawing.Size(70, 15);
            this.lblPayment.TabIndex = 3;
            this.lblPayment.Text = "Payment";
            // 
            // lblPayeeCategoryMemo
            // 
            this.lblPayeeCategoryMemo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPayeeCategoryMemo.BackColor = System.Drawing.Color.Silver;
            this.lblPayeeCategoryMemo.Location = new System.Drawing.Point(142, 0);
            this.lblPayeeCategoryMemo.Name = "lblPayeeCategoryMemo";
            this.lblPayeeCategoryMemo.Size = new System.Drawing.Size(400, 15);
            this.lblPayeeCategoryMemo.TabIndex = 2;
            this.lblPayeeCategoryMemo.Text = "Payee/Category/Memo";
            // 
            // lblNum
            // 
            this.lblNum.BackColor = System.Drawing.Color.Silver;
            this.lblNum.Location = new System.Drawing.Point(71, 0);
            this.lblNum.Name = "lblNum";
            this.lblNum.Size = new System.Drawing.Size(70, 15);
            this.lblNum.TabIndex = 1;
            this.lblNum.Text = "Num";
            // 
            // lblDate
            // 
            this.lblDate.BackColor = System.Drawing.Color.Silver;
            this.lblDate.Location = new System.Drawing.Point(0, 0);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(70, 15);
            this.lblDate.TabIndex = 0;
            this.lblDate.Text = "Date";
            // 
            // contentList
            // 
            this.contentList.BackColor = System.Drawing.Color.WhiteSmoke;
            this.contentList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.contentList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.contentList.FormattingEnabled = true;
            this.contentList.ItemHeight = 30;
            this.contentList.Location = new System.Drawing.Point(0, 15);
            this.contentList.Name = "contentList";
            this.contentList.ScrollAlwaysVisible = true;
            this.contentList.Size = new System.Drawing.Size(797, 120);
            this.contentList.TabIndex = 1;
            // 
            // lblBalance
            // 
            this.lblBalance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBalance.BackColor = System.Drawing.Color.Silver;
            this.lblBalance.Location = new System.Drawing.Point(706, 0);
            this.lblBalance.Name = "lblBalance";
            this.lblBalance.Size = new System.Drawing.Size(70, 15);
            this.lblBalance.TabIndex = 6;
            this.lblBalance.Text = "Balance";
            // 
            // RegisterControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblBalance);
            this.Controls.Add(this.contentList);
            this.Controls.Add(this.headerPanel);
            this.Name = "RegisterControl";
            this.Size = new System.Drawing.Size(797, 150);
            this.headerPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.ListBox contentList;
        private System.Windows.Forms.Label lblPayeeCategoryMemo;
        private System.Windows.Forms.Label lblNum;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblDeposit;
        private System.Windows.Forms.Label lblClr;
        private System.Windows.Forms.Label lblPayment;
        private System.Windows.Forms.Label lblBalance;
        private System.Windows.Forms.Label lblEnd;
    }
}
