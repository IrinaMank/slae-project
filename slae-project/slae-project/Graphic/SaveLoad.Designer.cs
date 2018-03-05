namespace slae_project
{
    partial class SaveLoad
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
            this.groupBox2_NumberMatrix = new System.Windows.Forms.GroupBox();
            this.textBox2_NumberMatrix = new System.Windows.Forms.TextBox();
            this.groupBox1_NameMatrix = new System.Windows.Forms.GroupBox();
            this.textBox1_NameMatrix = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.radioButton1_Save = new System.Windows.Forms.RadioButton();
            this.radioButton2_Load = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.button1 = new System.Windows.Forms.Button();
            this.button2_Exit = new System.Windows.Forms.Button();
            this.groupBox2_NumberMatrix.SuspendLayout();
            this.groupBox1_NameMatrix.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2_NumberMatrix
            // 
            this.groupBox2_NumberMatrix.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2_NumberMatrix.Controls.Add(this.textBox2_NumberMatrix);
            this.groupBox2_NumberMatrix.Location = new System.Drawing.Point(3, 127);
            this.groupBox2_NumberMatrix.Name = "groupBox2_NumberMatrix";
            this.groupBox2_NumberMatrix.Size = new System.Drawing.Size(330, 49);
            this.groupBox2_NumberMatrix.TabIndex = 3;
            this.groupBox2_NumberMatrix.TabStop = false;
            this.groupBox2_NumberMatrix.Text = "Номер матрицы:";
            // 
            // textBox2_NumberMatrix
            // 
            this.textBox2_NumberMatrix.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox2_NumberMatrix.Location = new System.Drawing.Point(50, 22);
            this.textBox2_NumberMatrix.Name = "textBox2_NumberMatrix";
            this.textBox2_NumberMatrix.Size = new System.Drawing.Size(210, 20);
            this.textBox2_NumberMatrix.TabIndex = 0;
            this.textBox2_NumberMatrix.TextChanged += new System.EventHandler(this.textBox2_NumberMatrix_TextChanged);
            // 
            // groupBox1_NameMatrix
            // 
            this.groupBox1_NameMatrix.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1_NameMatrix.Controls.Add(this.textBox1_NameMatrix);
            this.groupBox1_NameMatrix.Location = new System.Drawing.Point(3, 72);
            this.groupBox1_NameMatrix.Name = "groupBox1_NameMatrix";
            this.groupBox1_NameMatrix.Size = new System.Drawing.Size(330, 49);
            this.groupBox1_NameMatrix.TabIndex = 2;
            this.groupBox1_NameMatrix.TabStop = false;
            this.groupBox1_NameMatrix.Text = "Имя файла (GraphicData_?.txt)";
            // 
            // textBox1_NameMatrix
            // 
            this.textBox1_NameMatrix.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1_NameMatrix.Location = new System.Drawing.Point(50, 19);
            this.textBox1_NameMatrix.Name = "textBox1_NameMatrix";
            this.textBox1_NameMatrix.Size = new System.Drawing.Size(210, 20);
            this.textBox1_NameMatrix.TabIndex = 0;
            this.textBox1_NameMatrix.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.checkBox1, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.groupBox3, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1_NameMatrix, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2_NumberMatrix, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 23.80952F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 19.04762F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 19.04762F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 19.04762F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 19.04762F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(336, 290);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(89, 198);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(157, 17);
            this.checkBox1.TabIndex = 5;
            this.checkBox1.Text = "Добавить в конец списка";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.tableLayoutPanel3);
            this.groupBox3.Location = new System.Drawing.Point(3, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(330, 63);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Режим работы окна";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.radioButton1_Save, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.radioButton2_Load, 1, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 19);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(333, 44);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // radioButton1_Save
            // 
            this.radioButton1_Save.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButton1_Save.AutoSize = true;
            this.radioButton1_Save.Checked = true;
            this.radioButton1_Save.Location = new System.Drawing.Point(3, 3);
            this.radioButton1_Save.Name = "radioButton1_Save";
            this.radioButton1_Save.Size = new System.Drawing.Size(160, 38);
            this.radioButton1_Save.TabIndex = 0;
            this.radioButton1_Save.TabStop = true;
            this.radioButton1_Save.Text = "Режим сохранения";
            this.radioButton1_Save.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButton1_Save.UseVisualStyleBackColor = true;
            this.radioButton1_Save.CheckedChanged += new System.EventHandler(this.radioButton1_Save_CheckedChanged);
            // 
            // radioButton2_Load
            // 
            this.radioButton2_Load.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButton2_Load.AutoSize = true;
            this.radioButton2_Load.Location = new System.Drawing.Point(169, 3);
            this.radioButton2_Load.Name = "radioButton2_Load";
            this.radioButton2_Load.Size = new System.Drawing.Size(161, 38);
            this.radioButton2_Load.TabIndex = 1;
            this.radioButton2_Load.Text = "Режим загрузки";
            this.radioButton2_Load.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButton2_Load.UseVisualStyleBackColor = true;
            this.radioButton2_Load.CheckedChanged += new System.EventHandler(this.radioButton2_Load_CheckedChanged);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.button1, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.button2_Exit, 0, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 237);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(330, 50);
            this.tableLayoutPanel2.TabIndex = 6;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.AutoSize = true;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button1.Location = new System.Drawing.Point(168, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(159, 44);
            this.button1.TabIndex = 4;
            this.button1.Text = "commit";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2_Exit
            // 
            this.button2_Exit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button2_Exit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button2_Exit.Location = new System.Drawing.Point(3, 3);
            this.button2_Exit.Name = "button2_Exit";
            this.button2_Exit.Size = new System.Drawing.Size(159, 44);
            this.button2_Exit.TabIndex = 5;
            this.button2_Exit.Text = "Выход";
            this.button2_Exit.UseVisualStyleBackColor = true;
            this.button2_Exit.Click += new System.EventHandler(this.button2_Exit_Click);
            // 
            // SaveLoad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(336, 290);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "SaveLoad";
            this.Text = "SaveLoad";
            this.groupBox2_NumberMatrix.ResumeLayout(false);
            this.groupBox2_NumberMatrix.PerformLayout();
            this.groupBox1_NameMatrix.ResumeLayout(false);
            this.groupBox1_NameMatrix.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox2_NumberMatrix;
        private System.Windows.Forms.TextBox textBox2_NumberMatrix;
        private System.Windows.Forms.GroupBox groupBox1_NameMatrix;
        private System.Windows.Forms.TextBox textBox1_NameMatrix;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.RadioButton radioButton1_Save;
        private System.Windows.Forms.RadioButton radioButton2_Load;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2_Exit;
    }
}