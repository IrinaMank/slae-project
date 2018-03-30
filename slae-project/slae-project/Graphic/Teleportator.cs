using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace slae_project
{
    public partial class Teleporter : Form
    {
        public Teleporter(SharpGLForm sharp)
        {
            InitializeComponent();
            Show();
            refered_sharp = sharp;
            Refresher();
        }
        SharpGLForm refered_sharp = null;

        private bool CheckNumberFromString(string str, ref int place, int Limit, bool Messages = false)
        {
            if (int.TryParse(str, out place) == false)
            {
                
                if (Messages) MessageBox.Show("Это не число");
                return false;
            }
            else
            {
                if (place < 0 || place > Limit)
                {
                    if (Messages) MessageBox.Show("Число не в диапазоне");
                    return false;
                }
            }
            return true;
        }

        //Кнопка телепорта
        private void button1_Click(object sender, EventArgs e)
        {
            int rem_x = 0, rem_y = 0;
            int N_Matrix = 0, N_Row = 0, N_Column = 0;
            if (CheckNumberFromString(textBox1_NumberMatrix.Text, ref N_Matrix, refered_sharp.GD.List_Of_Objects.Count() - 1, true))
                if (CheckNumberFromString(textBox2_NumberRow.Text, ref N_Row, refered_sharp.GD.List_Of_Objects[N_Matrix].yCellCount - 1, true))
                    if (CheckNumberFromString(TextBox_NumberColumn.Text, ref N_Column, refered_sharp.GD.List_Of_Objects[N_Matrix].xCellCount - 1, true))
                    {

                        int hnew = (int)((N_Column + refered_sharp.GD.LeftTopCellOfEachMatrix[N_Matrix].X + 0.5) * refered_sharp.GD.Grid.xCellSize - refered_sharp.openGLControl.Width / 2);
                        int vnew = (int)((N_Row + refered_sharp.GD.LeftTopCellOfEachMatrix[N_Matrix].Y + 2) * refered_sharp.GD.Grid.yCellSize - refered_sharp.openGLControl.Height / 2);
                        rem_x = hnew % refered_sharp.openGLControl.Width;
                        rem_y = vnew % refered_sharp.openGLControl.Height;

                        //if (hnew < refered_sharp.hScrollBar1.Minimum) hnew = refered_sharp.hScrollBar1.Minimum;
                        //if (hnew > refered_sharp.hScrollBar1.Maximum) hnew = refered_sharp.hScrollBar1.Maximum;
                        //if (vnew < refered_sharp.vScrollBar1.Minimum) vnew = refered_sharp.vScrollBar1.Minimum;
                        //if (vnew > refered_sharp.vScrollBar1.Maximum) vnew = refered_sharp.vScrollBar1.Maximum;
                        if (hnew < refered_sharp.hScrollBar1.Minimum) refered_sharp.hScrollBar1.Minimum = hnew;
                        if (hnew > refered_sharp.hScrollBar1.Maximum) refered_sharp.hScrollBar1.Maximum = hnew;
                        if (vnew < refered_sharp.vScrollBar1.Minimum) refered_sharp.vScrollBar1.Minimum = vnew;
                        if (vnew > refered_sharp.vScrollBar1.Maximum) refered_sharp.vScrollBar1.Maximum = vnew;

                        refered_sharp.hScrollBar1.Value = hnew;
                        refered_sharp.vScrollBar1.Value = vnew;


                    }

            refered_sharp.GD.mouse.true_x = refered_sharp.openGLControl.Width/2;
            refered_sharp.GD.mouse.true_y = refered_sharp.openGLControl.Height/2;
            refered_sharp.Refresh_Window(false);
            //refered_sharp.GD.LaserCrossroad(refered_sharp.openGLControl.Width/2, refered_sharp.openGLControl.Height / 2);
            Refresher();

        }


        private void Refresher()
        {
            Quantity_of_matrix_refresher();

            int NumberMatr = 0;
            if (CheckNumberFromString(textBox1_NumberMatrix.Text, ref NumberMatr, refered_sharp.GD.List_Of_Objects.Count() - 1))
            {
                groupBox2_NumberRow.Text = "Номер строки от 0 до " + (refered_sharp.GD.List_Of_Objects[NumberMatr].yCellCount - 1).ToString();
                groupBox1_NumberColumn.Text = "Номер столбца от 0 до " + (refered_sharp.GD.List_Of_Objects[NumberMatr].xCellCount - 1).ToString();
                groupBox2_NumberRow.Visible = true;
                groupBox1_NumberColumn.Visible = true;
            }
            else
            {
                groupBox2_NumberRow.Text = "Номер строки";
                groupBox1_NumberColumn.Text = "Номер столбца";
                groupBox2_NumberRow.Visible = false;
                groupBox1_NumberColumn.Visible = false;
            }
        }


        private void button2_Exit_Click(object sender, EventArgs e)
        {
            Close();
        }
        public void Quantity_of_matrix_refresher()
        {
            groupBox1_NumberMatrix.Text = "Номер матрицы: ";

            if (refered_sharp.GD.List_Of_Objects.Count() > 0)
                groupBox1_NumberMatrix.Text += "от 0 до " + (refered_sharp.GD.List_Of_Objects.Count() - 1).ToString();
        }
        private void textBox1_NumberMatrix_TextChanged(object sender, EventArgs e)
        {
            Refresher();
        }
        private void textBox2_NumberRow_TextChanged(object sender, EventArgs e)
        {

        }
        private void TextBox_NumberColumn_TextChanged(object sender, EventArgs e)
        {

        }


    }
}
