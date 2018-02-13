using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SharpGL;

namespace slae_project
{
    public class GraphicData
    {
        public class GraphicObject
        {
            public string Name;

            public List<List<double>> Matrix = new List<List<double>>();

            public GraphicObject(string _Name, List<List<double>> _Matrix)
            {
                this.Name = _Name; Matrix = _Matrix;
            }
            public GraphicObject(string _Name, List<double> _Vector)
            {
                this.Name = _Name; Matrix.Add(new List<double>(_Vector));
            }
            public GraphicObject(string _Name, double _Value)
            {
                this.Name = _Name; Matrix.Add(new List<double>()); Matrix[0].Add(_Value);
            }
        }
        public List<GraphicObject> List_Of_Objects = new List<GraphicObject>();

        public void ItisATest()
        {
            double[] vector4ik = new double[] { 1, 2, 3, 4, 5 };
            List_Of_Objects.Add(new GraphicObject("FirstVector", vector4ik.ToList()));
            List_Of_Objects.Add(new GraphicObject("SecondVector", vector4ik.ToList()));
            List_Of_Objects.Add(new GraphicObject("ThirdVector", 5));
            List_Of_Objects.Add(new GraphicObject("FourthVector", vector4ik.ToList()));
        }

    }
    public class PointDouble
    {
        public double x, y;
        public PointDouble(double _x, double _y)
        {
            x = _x; y = _y;
        }
    }
    public class Net
    {
        public PointDouble initP = new PointDouble(5.0, 5.0);
        public PointDouble cursorP;
        public double xCellSize = 25, yCellSize = 25;

        public Net()
        {
            cursorP = new PointDouble(initP.x, initP.y);
        }
        public void X_move()
        {
            cursorP.x += xCellSize;
        }
        public void X_nullificate()
        {
            cursorP.x = initP.x;
        }

        public void Y_move()
        {
            cursorP.y -= yCellSize;
        }
        public void Y_nullificate()
        {
            cursorP.y = initP.y;
        }
    }
    /// <summary>
    /// The main form class.
    /// </summary>
    public partial class SharpGLForm : Form
    {
        GraphicData GD = new GraphicData();
        Net Grid = new Net();
        /// <summary>
        /// Initializes a new instance of the <see cref="SharpGLForm"/> class.
        /// </summary>
        public SharpGLForm(bool Type)
        {
            InitializeComponent();

            GD.ItisATest();
        }

        /// <summary>
        /// Handles the OpenGLDraw event of the openGLControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RenderEventArgs"/> instance containing the event data.</param>
        private void openGLControl_OpenGLDraw(object sender, RenderEventArgs e)
        {
            //  Get the OpenGL object.
            OpenGL gl = openGLControl.OpenGL;

            //  Clear the color and depth buffer.
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            //  Load the identity matrix.
            gl.LoadIdentity();
            
            Grid.initP.y = openGLControl.Height - Grid.yCellSize;
            foreach(var obj in GD.List_Of_Objects)
            {
                gl.DrawText((int)Grid.cursorP.x, (int)Grid.cursorP.y, 0.0f, 0.0f, 0.0f, "", 14.0f, obj.Name);
                Grid.Y_move();

                foreach (var vect in obj.Matrix)
                {
                    foreach (var value in vect)
                    {
                        gl.DrawText((int)Grid.cursorP.x, (int)Grid.cursorP.y, 0.0f, 0.0f, 0.0f, "", 14.0f, value.ToString());
                        Grid.X_move();
                    }

                    Grid.X_nullificate(); Grid.Y_move();
                }
            }
            Grid.Y_nullificate();
            //  Rotate around the Y axis.
            //gl.Rotate(rotation, 0.0f, 1.0f, 0.0f);

            //  Draw a coloured pyramid.
            /*gl.Begin(OpenGL.GL_TRIANGLES);
            gl.Color(1.0f, 0.0f, 0.0f);
            gl.Vertex(0.0f, 1.0f, 0.0f);
            gl.Color(0.0f, 1.0f, 0.0f);
            gl.Vertex(-1.0f, -1.0f, 1.0f);
            gl.Color(0.0f, 0.0f, 1.0f);
            gl.Vertex(1.0f, -1.0f, 1.0f);
            gl.Color(1.0f, 0.0f, 0.0f);
            gl.Vertex(0.0f, 1.0f, 0.0f);
            gl.Color(0.0f, 0.0f, 1.0f);
            gl.Vertex(1.0f, -1.0f, 1.0f);
            gl.Color(0.0f, 1.0f, 0.0f);
            gl.Vertex(1.0f, -1.0f, -1.0f);
            gl.Color(1.0f, 0.0f, 0.0f);
            gl.Vertex(0.0f, 1.0f, 0.0f);
            gl.Color(0.0f, 1.0f, 0.0f);
            gl.Vertex(1.0f, -1.0f, -1.0f);
            gl.Color(0.0f, 0.0f, 1.0f);
            gl.Vertex(-1.0f, -1.0f, -1.0f);
            gl.Color(1.0f, 0.0f, 0.0f);
            gl.Vertex(0.0f, 1.0f, 0.0f);
            gl.Color(0.0f, 0.0f, 1.0f);
            gl.Vertex(-1.0f, -1.0f, -1.0f);
            gl.Color(0.0f, 1.0f, 0.0f);
            gl.Vertex(-1.0f, -1.0f, 1.0f);
            gl.End();

            //  Nudge the rotation.
            rotation += 3.0f;*/
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
            //  TODO: Set the projection matrix here.

            //  Get the OpenGL object.
            OpenGL gl = openGLControl.OpenGL;

            //  Set the projection matrix.
            gl.MatrixMode(OpenGL.GL_PROJECTION);

            //  Load the identity.
            gl.LoadIdentity();

            //  Create a perspective transformation.
            gl.Perspective(60.0f, (double)Width / (double)Height, 0.01, 100.0);

            //  Use the 'look at' helper function to position and aim the camera.
            gl.LookAt(-5, 5, -5, 0, 0, 0, 0, 1, 0);

            //  Set the modelview matrix.
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
        }

        /// <summary>
        /// The current rotation.
        /// </summary>
        private float rotation = 0.0f;

    }
}
