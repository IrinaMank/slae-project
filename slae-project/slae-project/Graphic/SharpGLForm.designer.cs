namespace slae_project
{
    partial class SharpGLForm
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
            this.openGLControl = new SharpGL.OpenGLControl();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.button_reset = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.trackBar_FontSize = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.trackBar_CellHeight = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.trackBar_CellWidth = new System.Windows.Forms.TrackBar();
            this.label4 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.radioButton3_Exponential = new System.Windows.Forms.RadioButton();
            this.radioButton2_Double = new System.Windows.Forms.RadioButton();
            this.radioButton1_General = new System.Windows.Forms.RadioButton();
            this.button_refresh = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.trackBar_QuantityAfterPoint = new System.Windows.Forms.TrackBar();
            this.button_exit = new System.Windows.Forms.Button();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.label6_FAQ = new System.Windows.Forms.Label();
            this.label7_FAQ_move_phrase = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.openGLControl)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_FontSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_CellHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_CellWidth)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_QuantityAfterPoint)).BeginInit();
            this.SuspendLayout();
            // 
            // openGLControl
            // 
            this.openGLControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.openGLControl.DrawFPS = true;
            this.openGLControl.Location = new System.Drawing.Point(21, 0);
            this.openGLControl.Name = "openGLControl";
            this.openGLControl.OpenGLVersion = SharpGL.Version.OpenGLVersion.OpenGL2_1;
            this.openGLControl.RenderContextType = SharpGL.RenderContextType.FBO;
            this.openGLControl.RenderTrigger = SharpGL.RenderTrigger.TimerBased;
            this.openGLControl.Size = new System.Drawing.Size(544, 498);
            this.openGLControl.TabIndex = 0;
            this.openGLControl.OpenGLInitialized += new System.EventHandler(this.openGLControl_OpenGLInitialized);
            this.openGLControl.OpenGLDraw += new SharpGL.RenderEventHandler(this.openGLControl_OpenGLDraw);
            this.openGLControl.Resized += new System.EventHandler(this.openGLControl_Resized);
            this.openGLControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.openGLControl_MouseDown);
            this.openGLControl.MouseUp += new System.Windows.Forms.MouseEventHandler(this.openGLControl_MouseUp);
            this.openGLControl.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.openGLControl_MouseScroller);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.button_reset, 0, 11);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.trackBar_FontSize, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.trackBar_CellHeight, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.trackBar_CellWidth, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.button_refresh, 0, 10);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.trackBar_QuantityAfterPoint, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.button_exit, 0, 12);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(565, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 13;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.999701F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.999402F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.999701F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.999402F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.999701F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.999402F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.999701F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 23.99821F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4.002901F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.999402F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.999402F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.999402F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.00368F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(181, 521);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // button_reset
            // 
            this.button_reset.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_reset.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button_reset.Location = new System.Drawing.Point(4, 436);
            this.button_reset.Name = "button_reset";
            this.button_reset.Size = new System.Drawing.Size(173, 30);
            this.button_reset.TabIndex = 3;
            this.button_reset.Text = "Сбросить";
            this.button_reset.UseVisualStyleBackColor = true;
            this.button_reset.Click += new System.EventHandler(this.button_reset_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label1.Location = new System.Drawing.Point(4, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(173, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Размер шрифта";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trackBar_FontSize
            // 
            this.trackBar_FontSize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBar_FontSize.Location = new System.Drawing.Point(4, 25);
            this.trackBar_FontSize.Maximum = 24;
            this.trackBar_FontSize.Minimum = 4;
            this.trackBar_FontSize.Name = "trackBar_FontSize";
            this.trackBar_FontSize.Size = new System.Drawing.Size(173, 34);
            this.trackBar_FontSize.TabIndex = 3;
            this.trackBar_FontSize.Value = 14;
            this.trackBar_FontSize.ValueChanged += new System.EventHandler(this.trackBar_FontSize_ValueChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label2.Location = new System.Drawing.Point(4, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(173, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Высота ячейки";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trackBar_CellHeight
            // 
            this.trackBar_CellHeight.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBar_CellHeight.Location = new System.Drawing.Point(4, 87);
            this.trackBar_CellHeight.Maximum = 100;
            this.trackBar_CellHeight.Minimum = 20;
            this.trackBar_CellHeight.Name = "trackBar_CellHeight";
            this.trackBar_CellHeight.Size = new System.Drawing.Size(173, 34);
            this.trackBar_CellHeight.TabIndex = 5;
            this.trackBar_CellHeight.Value = 30;
            this.trackBar_CellHeight.ValueChanged += new System.EventHandler(this.trackBar_CellHeight_ValueChanged);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label3.Location = new System.Drawing.Point(4, 128);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(173, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "Ширина ячейки";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trackBar_CellWidth
            // 
            this.trackBar_CellWidth.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBar_CellWidth.Location = new System.Drawing.Point(4, 149);
            this.trackBar_CellWidth.Maximum = 300;
            this.trackBar_CellWidth.Minimum = 20;
            this.trackBar_CellWidth.Name = "trackBar_CellWidth";
            this.trackBar_CellWidth.Size = new System.Drawing.Size(173, 34);
            this.trackBar_CellWidth.TabIndex = 7;
            this.trackBar_CellWidth.Value = 75;
            this.trackBar_CellWidth.ValueChanged += new System.EventHandler(this.trackBar_CellWidth_ValueChanged);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label4.Location = new System.Drawing.Point(4, 190);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(173, 17);
            this.label4.TabIndex = 8;
            this.label4.Text = "Формат записи чисел";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Controls.Add(this.radioButton3_Exponential, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.radioButton2_Double, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.radioButton1_General, 0, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(4, 211);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(173, 115);
            this.tableLayoutPanel2.TabIndex = 9;
            // 
            // radioButton3_Exponential
            // 
            this.radioButton3_Exponential.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButton3_Exponential.AutoSize = true;
            this.radioButton3_Exponential.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.radioButton3_Exponential.Location = new System.Drawing.Point(3, 79);
            this.radioButton3_Exponential.Name = "radioButton3_Exponential";
            this.radioButton3_Exponential.Size = new System.Drawing.Size(167, 33);
            this.radioButton3_Exponential.TabIndex = 2;
            this.radioButton3_Exponential.Text = "Экспоненциальный";
            this.radioButton3_Exponential.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButton3_Exponential.UseVisualStyleBackColor = true;
            this.radioButton3_Exponential.CheckedChanged += new System.EventHandler(this.radioButton3_Exponential_CheckedChanged);
            // 
            // radioButton2_Double
            // 
            this.radioButton2_Double.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButton2_Double.AutoSize = true;
            this.radioButton2_Double.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.radioButton2_Double.Location = new System.Drawing.Point(3, 41);
            this.radioButton2_Double.Name = "radioButton2_Double";
            this.radioButton2_Double.Size = new System.Drawing.Size(167, 32);
            this.radioButton2_Double.TabIndex = 1;
            this.radioButton2_Double.Text = "Дробный";
            this.radioButton2_Double.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButton2_Double.UseVisualStyleBackColor = true;
            this.radioButton2_Double.CheckedChanged += new System.EventHandler(this.radioButton2_Double_CheckedChanged);
            // 
            // radioButton1_General
            // 
            this.radioButton1_General.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButton1_General.AutoSize = true;
            this.radioButton1_General.Checked = true;
            this.radioButton1_General.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.radioButton1_General.Location = new System.Drawing.Point(3, 3);
            this.radioButton1_General.Name = "radioButton1_General";
            this.radioButton1_General.Size = new System.Drawing.Size(167, 32);
            this.radioButton1_General.TabIndex = 0;
            this.radioButton1_General.TabStop = true;
            this.radioButton1_General.Text = "Основной";
            this.radioButton1_General.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButton1_General.UseVisualStyleBackColor = true;
            this.radioButton1_General.CheckedChanged += new System.EventHandler(this.radioButton1_General_CheckedChanged);
            // 
            // button_refresh
            // 
            this.button_refresh.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_refresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button_refresh.Location = new System.Drawing.Point(4, 395);
            this.button_refresh.Name = "button_refresh";
            this.button_refresh.Size = new System.Drawing.Size(173, 30);
            this.button_refresh.TabIndex = 2;
            this.button_refresh.Text = "Обновить";
            this.button_refresh.UseVisualStyleBackColor = true;
            this.button_refresh.Click += new System.EventHandler(this.button_refresh_Click);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label5.Location = new System.Drawing.Point(4, 333);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(173, 17);
            this.label5.TabIndex = 10;
            this.label5.Text = "Кол-во знаков";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trackBar_QuantityAfterPoint
            // 
            this.trackBar_QuantityAfterPoint.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBar_QuantityAfterPoint.Location = new System.Drawing.Point(4, 354);
            this.trackBar_QuantityAfterPoint.Maximum = 15;
            this.trackBar_QuantityAfterPoint.Name = "trackBar_QuantityAfterPoint";
            this.trackBar_QuantityAfterPoint.Size = new System.Drawing.Size(173, 34);
            this.trackBar_QuantityAfterPoint.TabIndex = 11;
            this.trackBar_QuantityAfterPoint.Value = 3;
            this.trackBar_QuantityAfterPoint.ValueChanged += new System.EventHandler(this.trackBar_QuantityAfterPoint_ValueChanged);
            // 
            // button_exit
            // 
            this.button_exit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_exit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button_exit.Location = new System.Drawing.Point(4, 477);
            this.button_exit.Name = "button_exit";
            this.button_exit.Size = new System.Drawing.Size(173, 30);
            this.button_exit.TabIndex = 0;
            this.button_exit.Text = "Выход";
            this.button_exit.UseVisualStyleBackColor = true;
            this.button_exit.Click += new System.EventHandler(this.button_exit_Click);
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.vScrollBar1.Location = new System.Drawing.Point(-2, 0);
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(20, 498);
            this.vScrollBar1.TabIndex = 2;
            this.vScrollBar1.ValueChanged += new System.EventHandler(this.vScrollBar1_ValueChanged);
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hScrollBar1.Location = new System.Drawing.Point(22, 501);
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(543, 20);
            this.hScrollBar1.TabIndex = 3;
            this.hScrollBar1.ValueChanged += new System.EventHandler(this.hScrollBar1_ValueChanged);
            // 
            // label6_FAQ
            // 
            this.label6_FAQ.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6_FAQ.AutoSize = true;
            this.label6_FAQ.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label6_FAQ.Location = new System.Drawing.Point(2, 501);
            this.label6_FAQ.Name = "label6_FAQ";
            this.label6_FAQ.Size = new System.Drawing.Size(16, 17);
            this.label6_FAQ.TabIndex = 4;
            this.label6_FAQ.Text = "?";
            this.label6_FAQ.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label6_FAQ.MouseLeave += new System.EventHandler(this.label6_FAQ_MouseLeave);
            this.label6_FAQ.MouseHover += new System.EventHandler(this.label6_FAQ_MouseHover);
            // 
            // label7_FAQ_move_phrase
            // 
            this.label7_FAQ_move_phrase.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label7_FAQ_move_phrase.AutoSize = true;
            this.label7_FAQ_move_phrase.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label7_FAQ_move_phrase.Location = new System.Drawing.Point(21, 481);
            this.label7_FAQ_move_phrase.Name = "label7_FAQ_move_phrase";
            this.label7_FAQ_move_phrase.Size = new System.Drawing.Size(489, 17);
            this.label7_FAQ_move_phrase.TabIndex = 5;
            this.label7_FAQ_move_phrase.Text = "Перемещаться по полю можно с помощью зажатой левой кнопкой мыши";
            this.label7_FAQ_move_phrase.Visible = false;
            // 
            // SharpGLForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(746, 521);
            this.Controls.Add(this.label7_FAQ_move_phrase);
            this.Controls.Add(this.label6_FAQ);
            this.Controls.Add(this.hScrollBar1);
            this.Controls.Add(this.vScrollBar1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.openGLControl);
            this.Name = "SharpGLForm";
            this.Text = "Графическое отображение";
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SharpGLForm_MouseMove);
            this.Resize += new System.EventHandler(this.SharpGLForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.openGLControl)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_FontSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_CellHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_CellWidth)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_QuantityAfterPoint)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SharpGL.OpenGLControl openGLControl;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button button_exit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_refresh;
        private System.Windows.Forms.TrackBar trackBar_FontSize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar trackBar_CellHeight;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TrackBar trackBar_CellWidth;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.RadioButton radioButton3_Exponential;
        private System.Windows.Forms.RadioButton radioButton2_Double;
        private System.Windows.Forms.RadioButton radioButton1_General;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TrackBar trackBar_QuantityAfterPoint;
        private System.Windows.Forms.Button button_reset;
        private System.Windows.Forms.VScrollBar vScrollBar1;
        private System.Windows.Forms.HScrollBar hScrollBar1;
        private System.Windows.Forms.Label label6_FAQ;
        private System.Windows.Forms.Label label7_FAQ_move_phrase;
    }
}

