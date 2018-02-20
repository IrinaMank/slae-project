using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;

using SharpGL;

namespace slae_project
{
    
    /// <summary>
    /// The main form class.
    /// </summary>
    public partial class SharpGLForm : Form
    {
        /// <summary>
        /// Вся логика лежит тут и находится в GraphicalLogic.cs
        /// Главное что это структура хранения объектов(числа,векторов,матриц в нашем формате)
        /// </summary>
        public GraphicData GD;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SharpGLForm"/> class.
        /// </summary>
        public SharpGLForm(bool Type)
        {
            InitializeComponent();
            //SharpGLWrappedThread ThreadController = new SharpGLWrappedThread();

            //Облегчим себе жизнь. Передадим в главную логическую сразу.
            GD = new GraphicData(openGLControl);
            
            //Manual Рендеринг, мы же не делаем игру, так что смысла в RealTime FPS нету.
            //Для повторной отрисовки вызовите функцию openGLControl.Refresh();
            openGLControl.RenderTrigger = RenderTrigger.Manual;
            openGLControl.DoRender();

            //установить границы скруллбаров и сбросить мышки-местоположение в лево-нижний угол
            Refresh_Window();
        }
        public class SharpGLWrappedThread
        {
            Thread my_thread;
            public SharpGLWrappedThread()
            {
                my_thread = new Thread(Controller);
                my_thread.Start();

            }
            void Controller()
            {

                while (true) { }
                /*while (true)
                {
                    if (SharpForm != null)
                        if (!SharpForm.Created)
                            Application.Exit();
                } */
            }
        }
        /// <summary>
        /// Handles the OpenGLDraw event of the openGLControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RenderEventArgs"/> instance containing the event data.</param>
        private void openGLControl_OpenGLDraw(object sender, RenderEventArgs e)
        {
            GD.RealDraw(); 
        }

        /// <summary>
        /// Handles the OpenGLInitialized event of the openGLControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void openGLControl_OpenGLInitialized(object sender, EventArgs e)
        {
            //  TODO: Initialise OpenGL here.

            //  Get the OpenGL object.
            OpenGL gl = openGLControl.OpenGL;

            //  Set the clear color.
            gl.ClearColor(1.0f, 1.0f, 1.0f, 1.0f);
        }

        /// <summary>
        /// Handles the Resized event of the openGLControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void openGLControl_Resized(object sender, EventArgs e)
        {
            Wrapped_Resized();
            //gl.ClearColor(1.0f, 1.0f, 1.0f, 1.0f);
        }

        /// <summary>
        /// Очевидно эта функция при изменении окна. В 2D инициализацию OpenGL
        /// </summary>
        void Wrapped_Resized()
        {
            //  Get the OpenGL object.
            OpenGL gl = openGLControl.OpenGL;

            //  Set the projection matrix.
            gl.MatrixMode(OpenGL.GL_PROJECTION);

            //  Load the identity.
            gl.LoadIdentity();

            //Мы двумерны.
            gl.Ortho2D(0, openGLControl.Width, 0, openGLControl.Height);
            gl.Viewport(0, 0, openGLControl.Width, openGLControl.Height);
            
            //  Set the modelview matrix.
            gl.MatrixMode(OpenGL.GL_MODELVIEW);

            openGLControl.Refresh();
        }

        /// <summary>
        /// Выход из графического окна.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Происходит при движении мышкой над окном OpenGL
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openGLControl_MouseMove(object sender, MouseEventArgs e)
        {
            GD.mouse.setMouseData(MouseButtons.ToString(), Cursor.Position.X, Cursor.Position.Y, Cursor.Position.X - Location.X - openGLControl.Location.X - 8, -Cursor.Position.Y + Location.Y + Size.Height + openGLControl.Location.Y - 30);
            //Меняем курсор мышки на разные в зависимости нажата левая кнопка мышки или нет.
            if (MouseButtons.ToString() == "Left")
            {
                //Тут высчитывается насколько сместился курсор мышки нажатой
                

                Cursor.Current = Cursors.NoMove2D;
                GD.mouse.isPressed = true;

                //Местоположение экрана смещенное мышкой присваивает слайдерам(скруллбарам)
                try
                { hScrollBar1.Value = GD.mouse.ShiftedPosition.x; }
                catch (Exception error) { }
                try { vScrollBar1.Value = GD.mouse.ShiftedPosition.y; }
                catch (Exception error) { }
                //Обновили экран
                
            }
            else 
            {
                Cursor.Current = Cursors.Hand;
            }



            openGLControl.Refresh();

            //Эту штуку приходится вызывать когда чтото с мышкой поделал.
            Application.DoEvents();
        }
        /// <summary>
        /// Кнопку мыши опустили вниз(по идеи левую)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openGLControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (MouseButtons.ToString() == "Left")
            {
                //Меняем курсор мышки на четырехстрелочковый.
                Cursor.Current = Cursors.NoMove2D;
                //Мышка нажата
                GD.mouse.isPressed = true;
            }

            Application.DoEvents();
        }

        /// <summary>
        /// Кнопку мышки левую отжали.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openGLControl_MouseUp(object sender, MouseEventArgs e)
        {
                //Мышка отжата.            
            GD.mouse.isPressed = false;
            GD.mouse.isPressedBefore = false;

            //Обновили экран
            openGLControl.Refresh();
        }


        /// <summary>
        /// Справа в менюшке есть кнопка "Обновить", это она.
        /// Просто обновляет изображение и ничего больше.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_refresh_Click(object sender, EventArgs e)
        {
            openGLControl.Refresh();
        }

        /// <summary>
        /// Сбрасывает все настройки по умолчанию в менюшке справа
        /// И заодно заново отрисовывает и сбрасывает ваше местоположение
        /// в лево нижний угол.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_reset_Click(object sender, EventArgs e)
        {
            trackBar_QuantityAfterPoint.Value = GD.FontQuanitityAfterPoint = 3;
            trackBar_FontSize.Value = 14; GD.FontSize = 14;
            
            radioButton1_General.Checked = true;
            radioButton2_Double.Checked = false;
            radioButton3_Exponential.Checked = false;
            GD.font_format = 0;
            Refresh_Window();
        }

        /// <summary>
        /// Эту функцию я подарил юзерам, вызывать после добавления или удаления объектов
        /// Она обновляет изображение, настраивает максимумы скруллбаров(ибо оно зависит от границ матриц)
        /// И сбрасывает местоположение в лево-нижний угол
        /// </summary>
        public void Refresh_Window()
        {
            openGLControl.Refresh();
            SetScrollBars();
            GD.RealDraw_Try_To_Initialize = true;
            openGLControl.Refresh();
        }

        /// <summary>
        /// Функция реагирующая на изменение ползунка размера шрифта
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trackBar_FontSize_ValueChanged(object sender, EventArgs e)
        {
            GD.FontSize = trackBar_FontSize.Value;
            setAutoCell();
           // openGLControl.Refresh();
        }

        /// <summary>
        /// Функция реагирующая на изменение размеров окна
        /// Своеобразно говоря дубль, ибо openGL....Resized тоже самое
        /// Хотя эта вроде бы чаще реагирует, еще даже пока ты не отпустил мышку
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SharpGLForm_Resize(object sender, EventArgs e)
        {
            openGLControl.Refresh();
            GD.MoveToEndCursor();

            SetScrollBars();
        }

        /// <summary>
        /// Высчитывает границы скруллбаров максимумов и 
        /// местоположение мышки-обзора в левый нижний угол возвращает
        /// </summary>
        void SetScrollBars()
        {
            GD.mouse.BorderEndRecalculate();
            hScrollBar1.Minimum = GD.mouse.BorderBegin.x; hScrollBar1.Maximum = Math.Abs(GD.mouse.BorderEnd.x);
            vScrollBar1.Minimum = GD.mouse.BorderBegin.y; vScrollBar1.Maximum = Math.Abs(GD.mouse.BorderEnd.y);
            vScrollBar1.Value = Math.Abs(GD.mouse.BorderEnd.y);
            hScrollBar1.Value = Math.Abs(GD.mouse.BorderBegin.x);
        }
        /// <summary>
        /// Функция реагирующая на изменения ползунка за колво знаков после запятой.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trackBar_QuantityAfterPoint_ValueChanged(object sender, EventArgs e)
        {
            GD.FontQuanitityAfterPoint = trackBar_QuantityAfterPoint.Value;
            setAutoCell();
            Refresh_Window();
        }

        /// <summary>
        /// Функция реагирующая на изменение галочки на "Основной" формат записи чисел
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton1_General_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1_General.Checked == true)
            {
                GD.font_format = GraphicData.FontFormat.G;
                radioButton2_Double.Checked = false;
                radioButton3_Exponential.Checked = false;
            }
            setAutoCell();
            Refresh_Window();
        }

        /// <summary>
        /// Функция реагирующая на изменение галочки на "Дробный" формат записи чисел
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton2_Double_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2_Double.Checked == true)
            {
                GD.font_format = GraphicData.FontFormat.F;
                radioButton1_General.Checked = false;
                radioButton3_Exponential.Checked = false;
            }
            setAutoCell();
            Refresh_Window();
        }

        /// <summary>
        /// Функция реагирующая на изменение галочки на "Экспоненциальный" формат записи чисел
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton3_Exponential_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3_Exponential.Checked == true)
            {
                GD.font_format = GraphicData.FontFormat.E;
                radioButton2_Double.Checked = false;
                radioButton1_General.Checked = false;
            }
            setAutoCell();
            Refresh_Window();
        }

        /// <summary>
        /// Функция реагирующая на изменение ползунка скруллбара вертикального
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void vScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            GD.mouse.ShiftedPosition.y = vScrollBar1.Value;
            openGLControl.Refresh();
        }

        /// <summary>
        /// Функция реагирующая на изменение ползунка скруллбара горизонтального
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            GD.mouse.ShiftedPosition.x = hScrollBar1.Value;
            openGLControl.Refresh();
        }

        /// <summary>
        /// Когда мышка парит над вопросиком в левом нижнем углу экрана.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void label6_FAQ_MouseHover(object sender, EventArgs e)
        {
            label7_FAQ_move_phrase.Visible = true;
        }

        /// <summary>
        /// Когда мышка прекращает парить над вопросиком в левом нижнем углу экрана.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void label6_FAQ_MouseLeave(object sender, EventArgs e)
        {
            label7_FAQ_move_phrase.Visible = false;
        }

        private int e_Delta_old = 0;
        private void openGLControl_MouseScroller(object sender, MouseEventArgs e)
        {
            try
            {
                
                vScrollBar1.Value -= (int)e.Delta/5;
                e_Delta_old = e.Delta;
            }
            catch (Exception error) { }


            //Обновили 
            Application.DoEvents();
        }

        private void radioButton1_Number_enabled_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1_Number_enabled.Checked)
            {
                radioButton1_Number_disabled.Checked = false;
                GD.TargetNumber = true;
            }
        }

        private void radioButton1_Number_disabled_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1_Number_disabled.Checked)
            {
                radioButton1_Number_enabled.Checked = false;
                GD.TargetNumber = false;
            }
        }

        private void radioButton2_TargetPlus_Enabled_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2_TargetPlus_Enabled.Checked)
            {
                radioButton1_TargetPlus_Disabled.Checked = false;
                GD.TargetPlus = true;
            }
        }

        private void radioButton1_TargetPlus_Disabled_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1_TargetPlus_Disabled.Checked)
            {
                radioButton2_TargetPlus_Enabled.Checked = false;
                GD.TargetPlus = false;
            }
        }
    }
}
