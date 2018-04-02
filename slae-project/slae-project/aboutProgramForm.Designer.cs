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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(aboutProgramForm));
            this.aboutProgramTextBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // aboutProgramTextBox
            // 
            this.aboutProgramTextBox.AcceptsTab = true;
            this.aboutProgramTextBox.BackColor = System.Drawing.Color.White;
            this.aboutProgramTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.aboutProgramTextBox.Location = new System.Drawing.Point(0, 1);
            this.aboutProgramTextBox.Name = "aboutProgramTextBox";
            this.aboutProgramTextBox.ReadOnly = true;
            this.aboutProgramTextBox.Size = new System.Drawing.Size(456, 666);
            this.aboutProgramTextBox.TabIndex = 0;
            this.aboutProgramTextBox.Text = resources.GetString("aboutProgramTextBox.Text");
            // 
            // aboutProgramForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(451, 664);
            this.Controls.Add(this.aboutProgramTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "aboutProgramForm";
            this.Text = "О программе";
            this.Load += new System.EventHandler(this.aboutProgramForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox aboutProgramTextBox;
    }
}