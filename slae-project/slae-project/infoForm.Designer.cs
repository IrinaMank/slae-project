﻿namespace slae_project
{
    partial class infoForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(infoForm));
            this.infoTextBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // infoTextBox
            // 
            this.infoTextBox.AcceptsTab = true;
            this.infoTextBox.BackColor = System.Drawing.Color.White;
            this.infoTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.infoTextBox.ForeColor = System.Drawing.SystemColors.InfoText;
            this.infoTextBox.Location = new System.Drawing.Point(-2, -1);
            this.infoTextBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.infoTextBox.Name = "infoTextBox";
            this.infoTextBox.ReadOnly = true;
            this.infoTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.infoTextBox.Size = new System.Drawing.Size(559, 454);
            this.infoTextBox.TabIndex = 0;
            this.infoTextBox.Text = resources.GetString("infoTextBox.Text");
            this.infoTextBox.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // infoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::slae_project.Properties.Resources.kuchka;
            this.ClientSize = new System.Drawing.Size(555, 453);
            this.Controls.Add(this.infoTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "infoForm";
            this.Text = "Справка";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox infoTextBox;
    }
}