namespace slae_project
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.format_matrix = new System.Windows.Forms.ComboBox();
            this.solver = new System.Windows.Forms.ComboBox();
            this.precond = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.property_matrix = new System.Windows.Forms.CheckedListBox();
            this.start = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.менюToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.информацияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.выходToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button1 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.maskedTextBox1_size = new System.Windows.Forms.MaskedTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.maskedTextBox1_accuracy = new System.Windows.Forms.MaskedTextBox();
            this.groupBox1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // format_matrix
            // 
            this.format_matrix.FormattingEnabled = true;
            this.format_matrix.Location = new System.Drawing.Point(301, 76);
            this.format_matrix.Margin = new System.Windows.Forms.Padding(2);
            this.format_matrix.Name = "format_matrix";
            this.format_matrix.Size = new System.Drawing.Size(245, 21);
            this.format_matrix.TabIndex = 0;
            // 
            // solver
            // 
            this.solver.FormattingEnabled = true;
            this.solver.Location = new System.Drawing.Point(301, 109);
            this.solver.Margin = new System.Windows.Forms.Padding(2);
            this.solver.Name = "solver";
            this.solver.Size = new System.Drawing.Size(245, 21);
            this.solver.TabIndex = 1;
            // 
            // precond
            // 
            this.precond.FormattingEnabled = true;
            this.precond.Location = new System.Drawing.Point(301, 142);
            this.precond.Margin = new System.Windows.Forms.Padding(2);
            this.precond.Name = "precond";
            this.precond.Size = new System.Drawing.Size(245, 21);
            this.precond.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(96, 76);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Формат матрицы:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(96, 109);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Решатель:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(96, 142);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(119, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Предобусловливание:";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.property_matrix);
            this.groupBox1.Location = new System.Drawing.Point(99, 262);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(148, 95);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Матрица:";
            // 
            // property_matrix
            // 
            this.property_matrix.FormattingEnabled = true;
            this.property_matrix.Location = new System.Drawing.Point(14, 21);
            this.property_matrix.Margin = new System.Windows.Forms.Padding(2);
            this.property_matrix.Name = "property_matrix";
            this.property_matrix.Size = new System.Drawing.Size(117, 49);
            this.property_matrix.TabIndex = 7;
            this.property_matrix.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.property_matrix_ItemCheck);
            this.property_matrix.Validated += new System.EventHandler(this.property_matrix_Validated);
            // 
            // start
            // 
            this.start.Enabled = false;
            this.start.Location = new System.Drawing.Point(315, 333);
            this.start.Margin = new System.Windows.Forms.Padding(2);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(123, 24);
            this.start.TabIndex = 7;
            this.start.Text = "Загрузить данные";
            this.start.UseVisualStyleBackColor = true;
            this.start.Click += new System.EventHandler(this.start_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.менюToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(630, 24);
            this.menuStrip1.TabIndex = 8;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // менюToolStripMenuItem
            // 
            this.менюToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.информацияToolStripMenuItem,
            this.выходToolStripMenuItem});
            this.менюToolStripMenuItem.Name = "менюToolStripMenuItem";
            this.менюToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.менюToolStripMenuItem.Text = "Меню";
            // 
            // информацияToolStripMenuItem
            // 
            this.информацияToolStripMenuItem.Name = "информацияToolStripMenuItem";
            this.информацияToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.информацияToolStripMenuItem.Text = "Информация";
            // 
            // выходToolStripMenuItem
            // 
            this.выходToolStripMenuItem.Name = "выходToolStripMenuItem";
            this.выходToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.выходToolStripMenuItem.Text = "Выход";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(459, 333);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(118, 23);
            this.button1.TabIndex = 9;
            this.button1.Text = "Получить решение";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(96, 185);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Размер матрицы";
            // 
            // maskedTextBox1_size
            // 
            this.maskedTextBox1_size.Location = new System.Drawing.Point(301, 182);
            this.maskedTextBox1_size.Name = "maskedTextBox1_size";
            this.maskedTextBox1_size.Size = new System.Drawing.Size(100, 20);
            this.maskedTextBox1_size.TabIndex = 11;
            this.maskedTextBox1_size.TextAlignChanged += new System.EventHandler(this.maskedTextBox1_size_TextAlignChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(96, 226);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Точность решения";
            // 
            // maskedTextBox1_accuracy
            // 
            this.maskedTextBox1_accuracy.Location = new System.Drawing.Point(301, 218);
            this.maskedTextBox1_accuracy.Name = "maskedTextBox1_accuracy";
            this.maskedTextBox1_accuracy.Size = new System.Drawing.Size(100, 20);
            this.maskedTextBox1_accuracy.TabIndex = 13;
            this.maskedTextBox1_accuracy.TextChanged += new System.EventHandler(this.maskedTextBox1_accuracy_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(630, 392);
            this.Controls.Add(this.maskedTextBox1_accuracy);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.maskedTextBox1_size);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.start);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.precond);
            this.Controls.Add(this.solver);
            this.Controls.Add(this.format_matrix);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox format_matrix;
        private System.Windows.Forms.ComboBox solver;
        private System.Windows.Forms.ComboBox precond;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckedListBox property_matrix;
        private System.Windows.Forms.Button start;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem менюToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem информацияToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem выходToolStripMenuItem;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.MaskedTextBox maskedTextBox1_size;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.MaskedTextBox maskedTextBox1_accuracy;
    }
}

