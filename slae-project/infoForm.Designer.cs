namespace slae_project
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
            this.infoTextBox.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.infoTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.infoTextBox.ForeColor = System.Drawing.SystemColors.InfoText;
            this.infoTextBox.Location = new System.Drawing.Point(-3, -1);
            this.infoTextBox.Name = "infoTextBox";
            this.infoTextBox.ReadOnly = true;
            this.infoTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.infoTextBox.Size = new System.Drawing.Size(746, 306);
            this.infoTextBox.TabIndex = 0;
            this.infoTextBox.Text = resources.GetString("infoTextBox.Text");
            this.infoTextBox.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // infoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(742, 303);
            this.Controls.Add(this.infoTextBox);
            this.Name = "infoForm";
            this.Text = "Информация";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox infoTextBox;
    }
}