namespace slae_project
{
    partial class aboutProgramForm
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
            this.aboutProgramTextBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // aboutProgramTextBox
            // 
            this.aboutProgramTextBox.AcceptsTab = true;
            this.aboutProgramTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.15F, System.Drawing.FontStyle.Strikeout, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.aboutProgramTextBox.Location = new System.Drawing.Point(0, 1);
            this.aboutProgramTextBox.Name = "aboutProgramTextBox";
            this.aboutProgramTextBox.ReadOnly = true;
            this.aboutProgramTextBox.Size = new System.Drawing.Size(648, 332);
            this.aboutProgramTextBox.TabIndex = 0;
            this.aboutProgramTextBox.Text = "Здесь будет информация о разработчиках и программе.\n";
            // 
            // aboutProgramForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(647, 332);
            this.Controls.Add(this.aboutProgramTextBox);
            this.Name = "aboutProgramForm";
            this.Text = "О программе";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox aboutProgramTextBox;
    }
}