﻿namespace slae_project
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
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // format_matrix
            // 
            this.format_matrix.FormattingEnabled = true;
            this.format_matrix.Location = new System.Drawing.Point(401, 94);
            this.format_matrix.Name = "format_matrix";
            this.format_matrix.Size = new System.Drawing.Size(325, 24);
            this.format_matrix.TabIndex = 0;
            // 
            // solver
            // 
            this.solver.FormattingEnabled = true;
            this.solver.Location = new System.Drawing.Point(401, 134);
            this.solver.Name = "solver";
            this.solver.Size = new System.Drawing.Size(325, 24);
            this.solver.TabIndex = 1;
            // 
            // precond
            // 
            this.precond.FormattingEnabled = true;
            this.precond.Location = new System.Drawing.Point(401, 175);
            this.precond.Name = "precond";
            this.precond.Size = new System.Drawing.Size(325, 24);
            this.precond.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(128, 94);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Формат матрицы:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(128, 134);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Решатель:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(128, 175);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(154, 17);
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
            this.groupBox1.Location = new System.Drawing.Point(131, 229);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(296, 152);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Матрица:";
            // 
            // property_matrix
            // 
            this.property_matrix.FormattingEnabled = true;
            this.property_matrix.Location = new System.Drawing.Point(18, 26);
            this.property_matrix.Name = "property_matrix";
            this.property_matrix.Size = new System.Drawing.Size(155, 72);
            this.property_matrix.TabIndex = 7;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(840, 393);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.precond);
            this.Controls.Add(this.solver);
            this.Controls.Add(this.format_matrix);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
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
    }
}

