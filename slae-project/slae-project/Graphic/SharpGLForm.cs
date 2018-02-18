using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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
        GraphicData GD;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SharpGLForm"/> class.
        /// </summary>
        public SharpGLForm(bool Type)
        {
            InitializeComponent();

            //Облегчим себе жизнь. Передадим в главную логическую сразу.
            GD = new GraphicData(openGLControl);


            //Manual Рендеринг, мы же не делаем игру, так что смысла в RealTime FPS нету.
            //Для повторной отрисовки вызовите функцию openGLControl.Refresh();
            openGLControl.RenderTrigger = RenderTrigger.Manual;
            openGLControl.DoRender();
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
        void Wrapped_Resized()
        {
            //  Get the OpenGL object.
            OpenGL gl = openGLControl.OpenGL;

            //  Set the projection matrix.
            gl.MatrixMode(OpenGL.GL_PROJECTION);

            //  Load the identity.
            gl.LoadIdentity();

            //  Create a perspective transformation.
            //gl.Perspective(60.0f, (double)Width / (double)Height, 0.01, 1.0);

            gl.Ortho2D(0, openGLControl.Width, 0, openGLControl.Height);

            //  Use the 'look at' helper function to position and aim the camera.
            //gl.LookAt(0, 0, 1000, 0, 0, 0, 0, 1, 1);

            //  Set the modelview matrix.
            gl.MatrixMode(OpenGL.GL_MODELVIEW);

            openGLControl.Refresh();
        }

        /// <summary>
        /// Во время изменения размеров окна затемнить его и выключить надписи.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SharpGLForm_ResizeBegin(object sender, EventArgs e)
        {
            GD.TextDisable();
            tableLayoutPanel1.Visible = false;
            OpenGL gl = openGLControl.OpenGL;
            gl.ClearColor((float)240/255, (float)240 / 255, (float)240 / 255, 1.0f);
            openGLControl.Refresh();
        }

        /// <summary>
        /// Когда окно перескали таскать, вернуть всё обратно.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SharpGLForm_ResizeEnd(object sender, EventArgs e)
        {
            GD.TextEnable();
            tableLayoutPanel1.Visible = true;
            OpenGL gl = openGLControl.OpenGL;
            gl.ClearColor(1.0f, 1.0f, 1.0f, 1.0f);
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

        private void openGLControl_MouseMove(object sender, MouseEventArgs e)
        {
            //Тут высчитывается насколько сместился курсор мышки нажатой
            GD.mouse.setMouseData(MouseButtons.ToString(), MousePosition.X, MousePosition.Y);

            

            //Меняем курсор мышки на разные в зависимости нажата левая кнопка мышки или нет.
            if (MouseButtons.ToString() != "Left")
            {
                Cursor.Current = Cursors.Hand;
            }
            else 
            {
                Cursor.Current = Cursors.NoMove2D;
                GD.mouse.isPressed = true;
            }

            //Обновили экран
            openGLControl.Refresh();

            Application.DoEvents();
        }

        private void openGLControl_MouseDown(object sender, MouseEventArgs e)
        {
            //Меняем курсор мышки на четырехстрелочковый.
            Cursor.Current = Cursors.NoMove2D;
            //Мышка нажата
            GD.mouse.isPressed = true;

            Application.DoEvents();
        }

        private void openGLControl_MouseUp(object sender, MouseEventArgs e)
        {
            //Мышка отжата.            
            GD.mouse.isPressed = false;
            GD.mouse.isPressedBefore = false;
        }

        private void button_refresh_Click(object sender, EventArgs e)
        {
            GD.MoveToEndCursor();
            openGLControl.Refresh();
        }
    }
}
