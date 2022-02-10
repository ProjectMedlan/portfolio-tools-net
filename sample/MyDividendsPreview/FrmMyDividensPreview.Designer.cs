namespace MyDividendsPreview
{
    partial class FrmMyDividensPreview
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtFilename = new System.Windows.Forms.TextBox();
            this.lblFilename = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.lvwDividends = new System.Windows.Forms.ListView();
            this.chName = new System.Windows.Forms.ColumnHeader();
            this.chAmount = new System.Windows.Forms.ColumnHeader();
            this.chDividend = new System.Windows.Forms.ColumnHeader();
            this.chDividendTotal = new System.Windows.Forms.ColumnHeader();
            this.chCurrency = new System.Windows.Forms.ColumnHeader();
            this.chExDate = new System.Windows.Forms.ColumnHeader();
            this.chPayDate = new System.Windows.Forms.ColumnHeader();
            this.lblStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtFilename
            // 
            this.txtFilename.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtFilename.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
            this.txtFilename.Location = new System.Drawing.Point(96, 8);
            this.txtFilename.Name = "txtFilename";
            this.txtFilename.Size = new System.Drawing.Size(259, 23);
            this.txtFilename.TabIndex = 1;
            // 
            // lblFilename
            // 
            this.lblFilename.AutoSize = true;
            this.lblFilename.Location = new System.Drawing.Point(12, 12);
            this.lblFilename.Name = "lblFilename";
            this.lblFilename.Size = new System.Drawing.Size(67, 15);
            this.lblFilename.TabIndex = 0;
            this.lblFilename.Text = "Dateiname:";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(361, 8);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(112, 23);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // lvwDividends
            // 
            this.lvwDividends.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwDividends.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chName,
            this.chAmount,
            this.chDividend,
            this.chDividendTotal,
            this.chCurrency,
            this.chExDate,
            this.chPayDate});
            this.lvwDividends.FullRowSelect = true;
            this.lvwDividends.GridLines = true;
            this.lvwDividends.HideSelection = true;
            this.lvwDividends.Location = new System.Drawing.Point(12, 37);
            this.lvwDividends.MultiSelect = false;
            this.lvwDividends.Name = "lvwDividends";
            this.lvwDividends.Size = new System.Drawing.Size(776, 401);
            this.lvwDividends.TabIndex = 4;
            this.lvwDividends.UseCompatibleStateImageBehavior = false;
            this.lvwDividends.View = System.Windows.Forms.View.Details;
            // 
            // chName
            // 
            this.chName.Text = "Name";
            this.chName.Width = 180;
            // 
            // chAmount
            // 
            this.chAmount.Text = "Anzahl";
            this.chAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // chDividend
            // 
            this.chDividend.Text = "Dividende";
            this.chDividend.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.chDividend.Width = 80;
            // 
            // chDividendTotal
            // 
            this.chDividendTotal.Text = "Gesamtdividende";
            this.chDividendTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.chDividendTotal.Width = 120;
            // 
            // chCurrency
            // 
            this.chCurrency.Text = "Währung";
            this.chCurrency.Width = 80;
            // 
            // chExDate
            // 
            this.chExDate.Text = "Ex-Date";
            this.chExDate.Width = 80;
            // 
            // chPayDate
            // 
            this.chPayDate.Text = "Zahltag";
            this.chPayDate.Width = 80;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(496, 16);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(42, 15);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "Status:";
            // 
            // FrmMyDividensPreview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lvwDividends);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.lblFilename);
            this.Controls.Add(this.txtFilename);
            this.Name = "FrmMyDividensPreview";
            this.Text = "Meine Dividendenvorschau";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox txtFilename;
        private Label lblFilename;
        private Button btnStart;
        private ListView lvwDividends;
        private ColumnHeader chName;
        private ColumnHeader chAmount;
        private ColumnHeader chDividend;
        private ColumnHeader chDividendTotal;
        private ColumnHeader chCurrency;
        private ColumnHeader chExDate;
        private ColumnHeader chPayDate;
        private Label lblStatus;
    }
}