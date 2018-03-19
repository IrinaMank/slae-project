namespace slae_project
{
    partial class Teleporter
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
            this.groupBox2_NumberRow = new System.Windows.Forms.GroupBox();
            this.textBox2_NumberRow = new System.Windows.Forms.TextBox();
            this.groupBox1_NumberMatrix = new System.Windows.Forms.GroupBox();
            this.textBox1_NumberMatrix = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.button1_Click_Teleport = new System.Windows.Forms.Button();
            this.groupBox1_NumberColumn = new System.Windows.Forms.GroupBox();
            this.TextBox_NumberColumn = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.button2_Exit = new System.Windows.Forms.Button();
            this.groupBox2_NumberRow.SuspendLayout();
            this.groupBox1_NumberMatrix.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1_NumberColumn.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2_NumberRow
            // 
            this.groupBox2_NumberRow.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2_NumberRow.Controls.Add(this.textBox2_NumberRow);
            this.groupBox2_NumberRow.Location = new System.Drawing.Point(3, 72);
            this.groupBox2_NumberRow.Name = "groupBox2_NumberRow";
            this.groupBox2_NumberRow.Size = new System.Drawing.Size(330, 49);
            this.groupBox2_NumberRow.TabIndex = 3;
            this.groupBox2_NumberRow.TabStop = false;
            this.groupBox2_NumberRow.Text = "Номер строки:";
            // 
            // textBox2_NumberRow
            // 
            this.textBox2_NumberRow.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox2_NumberRow.Location = new System.Drawing.Point(50, 22);
            this.textBox2_NumberRow.Name = "textBox2_NumberRow";
            this.textBox2_NumberRow.Size = new System.Drawing.Size(210, 20);
            this.textBox2_NumberRow.TabIndex = 0;
            this.textBox2_NumberRow.TextChanged += new System.EventHandler(this.textBox2_NumberRow_TextChanged);
            // 
            // groupBox1_NumberMatrix
            // 
            this.groupBox1_NumberMatrix.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1_NumberMatrix.Controls.Add(this.textBox1_NumberMatrix);
            this.groupBox1_NumberMatrix.Location = new System.Drawing.Point(0, 19);
            this.groupBox1_NumberMatrix.Name = "groupBox1_NumberMatrix";
            this.groupBox1_NumberMatrix.Size = new System.Drawing.Size(330, 49);
            this.groupBox1_NumberMatrix.TabIndex = 2;
            this.groupBox1_NumberMatrix.TabStop = false;
            this.groupBox1_NumberMatrix.Text = "Номер матрицы";
            // 
            // textBox1_NumberMatrix
            // 
            this.textBox1_NumberMatrix.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1_NumberMatrix.Location = new System.Drawing.Point(50, 19);
            this.textBox1_NumberMatrix.Name = "textBox1_NumberMatrix";
            this.textBox1_NumberMatrix.Size = new System.Drawing.Size(210, 20);
            this.textBox1_NumberMatrix.TabIndex = 0;
            this.textBox1_NumberMatrix.TextChanged += new System.EventHandler(this.textBox1_NumberMatrix_TextChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.button1_Click_Teleport, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1_NumberColumn, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.groupBox3, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2_NumberRow, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 23.80952F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 19.04762F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 19.04762F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 19.04762F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 19.04762F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(336, 290);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // button1_Click_Teleport
            // 
            this.button1_Click_Teleport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button1_Click_Teleport.AutoSize = true;
            this.button1_Click_Teleport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button1_Click_Teleport.Location = new System.Drawing.Point(3, 182);
            this.button1_Click_Teleport.Name = "button1_Click_Teleport";
            this.button1_Click_Teleport.Size = new System.Drawing.Size(330, 49);
            this.button1_Click_Teleport.TabIndex = 4;
            this.button1_Click_Teleport.Text = "Телепортироваться";
            this.button1_Click_Teleport.UseVisualStyleBackColor = true;
            this.button1_Click_Teleport.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1_NumberColumn
            // 
            this.groupBox1_NumberColumn.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1_NumberColumn.Controls.Add(this.TextBox_NumberColumn);
            this.groupBox1_NumberColumn.Location = new System.Drawing.Point(3, 127);
            this.groupBox1_NumberColumn.Name = "groupBox1_NumberColumn";
            this.groupBox1_NumberColumn.Size = new System.Drawing.Size(330, 49);
            this.groupBox1_NumberColumn.TabIndex = 5;
            this.groupBox1_NumberColumn.TabStop = false;
            this.groupBox1_NumberColumn.Text = "Номер столбца:";
            // 
            // TextBox_NumberColumn
            // 
            this.TextBox_NumberColumn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBox_NumberColumn.Location = new System.Drawing.Point(50, 22);
            this.TextBox_NumberColumn.Name = "TextBox_NumberColumn";
            this.TextBox_NumberColumn.Size = new System.Drawing.Size(210, 20);
            this.TextBox_NumberColumn.TabIndex = 0;
            this.TextBox_NumberColumn.TextChanged += new System.EventHandler(this.TextBox_NumberColumn_TextChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.groupBox1_NumberMatrix);
            this.groupBox3.Location = new System.Drawing.Point(3, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(330, 63);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Введите адрес телепортации";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Controls.Add(this.button2_Exit, 0, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 237);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(330, 50);
            this.tableLayoutPanel2.TabIndex = 6;
            // 
            // button2_Exit
            // 
            this.button2_Exit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button2_Exit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button2_Exit.Location = new System.Drawing.Point(3, 3);
            this.button2_Exit.Name = "button2_Exit";
            this.button2_Exit.Size = new System.Drawing.Size(324, 44);
            this.button2_Exit.TabIndex = 5;
            this.button2_Exit.Text = "Выход";
            this.button2_Exit.UseVisualStyleBackColor = true;
            this.button2_Exit.Click += new System.EventHandler(this.button2_Exit_Click);
            // 
            // Teleporter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(336, 290);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Teleporter";
            this.Text = "Телепортатор к ячейке";
            this.groupBox2_NumberRow.ResumeLayout(false);
            this.groupBox2_NumberRow.PerformLayout();
            this.groupBox1_NumberMatrix.ResumeLayout(false);
            this.groupBox1_NumberMatrix.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox1_NumberColumn.ResumeLayout(false);
            this.groupBox1_NumberColumn.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox2_NumberRow;
        private System.Windows.Forms.TextBox textBox2_NumberRow;
        private System.Windows.Forms.GroupBox groupBox1_NumberMatrix;
        private System.Windows.Forms.TextBox textBox1_NumberMatrix;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button button1_Click_Teleport;
        private System.Windows.Forms.Button button2_Exit;
        private System.Windows.Forms.GroupBox groupBox1_NumberColumn;
        private System.Windows.Forms.TextBox TextBox_NumberColumn;
    }
}