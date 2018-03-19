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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.matrixDataGrid = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.vectorDataGrid = new System.Windows.Forms.DataGridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.matrixDataGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vectorDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.matrixDataGrid);
            this.groupBox1.Location = new System.Drawing.Point(27, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(452, 414);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Построение матрицы:";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // matrixDataGrid
            // 
            this.matrixDataGrid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.matrixDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.matrixDataGrid.Location = new System.Drawing.Point(17, 47);
            this.matrixDataGrid.Name = "matrixDataGrid";
            this.matrixDataGrid.RowTemplate.Height = 24;
            this.matrixDataGrid.Size = new System.Drawing.Size(404, 332);
            this.matrixDataGrid.TabIndex = 0;
            this.matrixDataGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView2_CellContentClick);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(690, 59);
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
            this.button2.Location = new System.Drawing.Point(690, 104);
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
            this.vectorDataGrid.Location = new System.Drawing.Point(556, 59);
            this.vectorDataGrid.Name = "vectorDataGrid";
            this.vectorDataGrid.RowTemplate.Height = 24;
            this.vectorDataGrid.Size = new System.Drawing.Size(73, 332);
            this.vectorDataGrid.TabIndex = 2;
            // 
            // groupBox2
            // 
            this.groupBox2.Location = new System.Drawing.Point(514, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(156, 414);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Построение вектора";
            // 
            // matrixForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(855, 473);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.vectorDataGrid);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.Name = "matrixForm";
            this.Text = "Form3";
            this.Load += new System.EventHandler(this.matrixFormLoad);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.matrixDataGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vectorDataGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridView matrixDataGrid;
        private System.Windows.Forms.DataGridView vectorDataGrid;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}