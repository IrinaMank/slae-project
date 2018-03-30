namespace slae_project
{
    partial class matrixForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.matrixDataGrid = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.vectorDataGrid = new System.Windows.Forms.DataGridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.x0DataGrid = new System.Windows.Forms.DataGridView();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.matrixDataGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vectorDataGrid)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.x0DataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.matrixDataGrid);
            this.groupBox1.Location = new System.Drawing.Point(27, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(508, 452);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Матрица СЛАУ";
            // 
            // matrixDataGrid
            // 
            this.matrixDataGrid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.matrixDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.matrixDataGrid.Location = new System.Drawing.Point(17, 47);
            this.matrixDataGrid.Name = "matrixDataGrid";
            this.matrixDataGrid.RowTemplate.Height = 24;
            this.matrixDataGrid.Size = new System.Drawing.Size(485, 399);
            this.matrixDataGrid.TabIndex = 0;
            this.matrixDataGrid.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.matrixDataGrid_CellBeginEdit);
            this.matrixDataGrid.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.matrixDataGrid_CellEndEdit);
            this.matrixDataGrid.MouseDown += new System.Windows.Forms.MouseEventHandler(this.matrixDataGrid_MouseDown);
            this.matrixDataGrid.MouseMove += new System.Windows.Forms.MouseEventHandler(this.matrixDataGrid_MouseMove);
            this.matrixDataGrid.MouseUp += new System.Windows.Forms.MouseEventHandler(this.matrixDataGrid_MouseUp);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(909, 59);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(153, 39);
            this.button1.TabIndex = 0;
            this.button1.Text = "Очистить матрицу";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(909, 104);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(153, 37);
            this.button2.TabIndex = 1;
            this.button2.Text = "Готово";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // vectorDataGrid
            // 
            this.vectorDataGrid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.vectorDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.vectorDataGrid.Location = new System.Drawing.Point(42, 47);
            this.vectorDataGrid.Name = "vectorDataGrid";
            this.vectorDataGrid.RowTemplate.Height = 24;
            this.vectorDataGrid.Size = new System.Drawing.Size(73, 399);
            this.vectorDataGrid.TabIndex = 2;
            this.vectorDataGrid.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.vector_CellEdit);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.groupBox2.Controls.Add(this.vectorDataGrid);
            this.groupBox2.Location = new System.Drawing.Point(571, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(156, 452);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Вектор правой части";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.Transparent;
            this.groupBox3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.groupBox3.Controls.Add(this.x0DataGrid);
            this.groupBox3.Location = new System.Drawing.Point(747, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(156, 452);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Начальное приближение";
            // 
            // x0DataGrid
            // 
            this.x0DataGrid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.x0DataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.x0DataGrid.Location = new System.Drawing.Point(41, 47);
            this.x0DataGrid.Name = "x0DataGrid";
            this.x0DataGrid.RowTemplate.Height = 24;
            this.x0DataGrid.Size = new System.Drawing.Size(73, 399);
            this.x0DataGrid.TabIndex = 3;
            this.x0DataGrid.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.vector_CellEdit);
            // 
            // matrixForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::slae_project.Properties.Resources.kuchka;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1074, 537);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "matrixForm";
            this.Text = "Manual input";
            this.Load += new System.EventHandler(this.matrixFormLoad);
            this.VisibleChanged += new System.EventHandler(this.matrixForm_VisibleChanged);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.matrixDataGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vectorDataGrid)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.x0DataGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridView matrixDataGrid;
        private System.Windows.Forms.DataGridView vectorDataGrid;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView x0DataGrid;
    }
}