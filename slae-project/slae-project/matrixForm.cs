using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using slae_project.Vector;

namespace slae_project
{
    public partial class matrixForm : Form
    {
        const int widthCol = 30;
        const int heightRow = 30;
        const int maxSize = 11;
        public int size;
        public bool property;

        public matrixForm()
        {
            InitializeComponent();
        }
        static Double Eval(String expression)
        {
            System.Data.DataTable table = new System.Data.DataTable();
            return Convert.ToDouble(table.Compute(expression, String.Empty));
        }
        private void matrixFormLoad(object sender, EventArgs e)
        {
            groupBox1.MouseMove += new MouseEventHandler(this.groupBox1_MouseMove);
            groupBox1.MouseUp += new MouseEventHandler(this.matrixDataGrid_MouseUp);
            groupBox1.MouseLeave += new EventHandler(this.groupBox1_MouseLeave);

            property = Form1.property_matr;
            size = 2;
            var column1 = new DataGridViewColumn();
            column1.Width = widthCol; //ширина колонки
            column1.ReadOnly = false; //значение в этой колонке нельзя править
            column1.Name = "column"; //текстовое имя колонки, его можно использовать вместо обращений по индексу
            column1.Frozen = true; //флаг, что данная колонка всегда отображается на своем месте
            column1.CellTemplate = new DataGridViewTextBoxCell(); //тип нашей колонки
            var column2 = new DataGridViewColumn();
            column2.Width = widthCol; //ширина колонки
            column2.ReadOnly = false; //значение в этой колонке нельзя править
            column2.Name = "column"; //текстовое имя колонки, его можно использовать вместо обращений по индексу
            column2.Frozen = true; //флаг, что данная колонка всегда отображается на своем месте
            column2.CellTemplate = new DataGridViewTextBoxCell(); //тип нашей колонки
            vectorDataGrid.Columns.Add(column1);
            x0DataGrid.Columns.Add(column2);
            vectorDataGrid.BringToFront();
            x0DataGrid.BringToFront();

            for (int i = 0; i < size; i++)
            {
                var column = new DataGridViewColumn();
                column.Width = widthCol; //ширина колонки
                column.ReadOnly = false; //значение в этой колонке нельзя править
                column.Name = "column" + Convert.ToString(i); //текстовое имя колонки, его можно использовать вместо обращений по индексу
                column.Frozen = true; //флаг, что данная колонка всегда отображается на своем месте
                column.CellTemplate = new DataGridViewTextBoxCell(); //тип нашей колонки

                var row = new DataGridViewRow();
                row.Height = heightRow; //ширина колонки
                row.ReadOnly = false; //значение в этой колонке нельзя править
                row.Frozen = true; //флаг, что данная колонка всегда отображается на своем месте

                var row1 = new DataGridViewRow();
                row1.Height = heightRow; //ширина колонки
                row1.ReadOnly = false; //значение в этой колонке нельзя править
                row1.Frozen = true; //флаг, что данная колонка всегда отображается на своем месте

                var row2 = new DataGridViewRow();
                row2.Height = heightRow; //ширина колонки
                row2.ReadOnly = false; //значение в этой колонке нельзя править
                row2.Frozen = true; //флаг, что данная колонка всегда отображается на своем месте


                matrixDataGrid.Columns.Add(column);
                matrixDataGrid.Rows.Add(row);
                vectorDataGrid.Rows.Add(row1);
                x0DataGrid.Rows.Add(row2);

            }

            matrixDataGrid.AllowUserToResizeRows = false; // запрещаем менять размер строчек
            matrixDataGrid.AllowUserToResizeColumns = false; // запрещаем менять размер столбцов
            matrixDataGrid.RowHeadersVisible = false; // делаем невидимыми заголовки строк
            matrixDataGrid.ColumnHeadersVisible = false; // делаем невидимыми заголовки столбцов

            vectorDataGrid.AllowUserToResizeRows = false; // запрещаем менять размер строчек
            vectorDataGrid.AllowUserToResizeColumns = false; // запрещаем менять размер столбцов
            vectorDataGrid.RowHeadersVisible = false; // делаем невидимыми заголовки строк
            vectorDataGrid.ColumnHeadersVisible = false; // делаем невидимыми заголовки столбцов
            x0DataGrid.AllowUserToResizeRows = false; // запрещаем менять размер строчек
            x0DataGrid.AllowUserToResizeColumns = false; // запрещаем менять размер столбцов
            x0DataGrid.RowHeadersVisible = false; // делаем невидимыми заголовки строк
            x0DataGrid.ColumnHeadersVisible = false; // делаем невидимыми заголовки столбцов

            vectorDataGrid.Width = widthCol;
            x0DataGrid.Width = widthCol;

            groupBox2.Width = vectorDataGrid.Width + widthCol*2;
            groupBox3.Width = vectorDataGrid.Width + widthCol*2;
            AddGrey();
            size = 2;
            sizeWrap();
        }

        /// <summary>
        /// Подгонка размеров элементов окна под текущую размерность матрицы. Вызывается каждый раз при измененении размеров матрицы.
        /// </summary>
        private void sizeWrap()
        {
            int gsize = size + 1;
            matrixDataGrid.Height = heightRow * gsize;
            matrixDataGrid.Width = widthCol * gsize;
            vectorDataGrid.Height = heightRow * size;
            x0DataGrid.Height = heightRow * size;
            groupBox1.Height = matrixDataGrid.Height + 50;
            groupBox1.Width = matrixDataGrid.Width + 30;
            groupBox2.Height = vectorDataGrid.Height + 50;
            groupBox3.Height = vectorDataGrid.Height + 50;
        }

        public void textToOnlyNumbers(int col, int row, int mat)
        {
            try
            {
                switch (mat)
                {
                    case 1:
                        matrixDataGrid[col, row].Value = Eval(matrixDataGrid[col, row].Value.ToString().Replace(",", "."));
                        break;
                    case 2:
                        vectorDataGrid[col, row].Value = Eval(vectorDataGrid[col, row].Value.ToString().Replace(",", "."));
                        break;
                    case 3:
                        x0DataGrid[col, row].Value = Eval(x0DataGrid[col, row].Value.ToString().Replace(",", "."));
                        break;
                }
            }
            catch
            {
                switch (mat)
                {
                    case 1:
                        matrixDataGrid[col, row].Value = null;
                        break;
                    case 2:
                        vectorDataGrid[col, row].Value = null;
                        break;
                    case 3:
                        x0DataGrid[col, row].Value = null;
                        break;
                }
            }
        }

        private void clearMatrix()
        {
            while (size > 2)
            {
                size--;
                matrixDataGrid.Rows.RemoveAt(size);
                matrixDataGrid.Columns.RemoveAt(size);
                vectorDataGrid.Rows.RemoveAt(size);
                x0DataGrid.Rows.RemoveAt(size);
            }

            for (int j = 0; j < size; j++)
            {
                for (int i = 0; i < size; i++)
                    matrixDataGrid[i, j].Value = null;
                vectorDataGrid[0, j].Value = null;
                x0DataGrid[0, j].Value = null;
            }

            sizeWrap();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            clearMatrix();
        }

        private void button2_Click(object sender, EventArgs e)
        {
           // size--;
            Form1.str_format_matrix = "Плотный";
            Factory.CreateMatrix(Form1.str_format_matrix);
            List<string> arrays = Factory.name_arr;
            int count_arr = arrays.Count();

            string name = "myMatrix.txt";
            using (StreamWriter writer = File.CreateText(name))
            {
                writer.WriteLine((size).ToString());
                for (int j = 0; j < size; j++)
                {
                    for (int i = 0; i < size; i++)
                    {
                        var line = matrixDataGrid[i, j].Value;
                        if (line != null)
                            writer.Write(line + " ");
                        else
                            writer.Write(0.0 + " ");
                    }
                    writer.Write("\r\n");
                }
                writer.Close();
            }
            FileLoadForm.filenames_format.Clear();
            FileLoadForm.filenames_format.Add(arrays[0].ToString(), name);

            FileLoadForm.F = new SimpleVector(size);

            object line1;
            for (int i = 0; i < size; i++)
            {
                line1 = vectorDataGrid[0, i].Value;
                if (line1 != null)
                    Factory.RightVector[i] = Convert.ToDouble(line1);
                else
                    Factory.RightVector[i] = 0.0;
            }
            FileLoadForm.X0 = new SimpleVector(size);
            for (int i = 0; i < size; i++)
            {
                line1 = x0DataGrid[0, i].Value;
                if (line1 != null)
                    Factory.X0[i] = Convert.ToDouble(line1);
                else
                    Factory.X0[i] = 0.0;
            }
            this.Visible = false;
            Form1.format.Enabled = true;
        }
        void AddGrey()
        {
            if (size < maxSize)
            {
                var column = new DataGridViewColumn();
                column.Width = widthCol; //ширина колонки
                column.ReadOnly = false; //значение в этой колонке нельзя править
                column.Name = "column" + Convert.ToString(size); //текстовое имя колонки, его можно использовать вместо обращений по индексу
                column.Frozen = true; //флаг, что данная колонка всегда отображается на своем месте
                column.CellTemplate = new DataGridViewTextBoxCell(); //тип нашей колонки
                column.DefaultCellStyle.BackColor = Color.Gray;

                var row = new DataGridViewRow();
                row.Height = heightRow; //ширина колонки
                row.ReadOnly = false; //значение в этой колонке нельзя править
                row.Frozen = true; //флаг, что данная колонка всегда отображается на своем месте
                row.DefaultCellStyle.BackColor = Color.Gray;

                var row1 = new DataGridViewRow();
                row1.Height = heightRow; //ширина колонки
                row1.ReadOnly = false; //значение в этой колонке нельзя править
                row1.Frozen = true; //флаг, что данная колонка всегда отображается на своем месте

                var row2 = new DataGridViewRow();
                row2.Height = heightRow; //ширина колонки
                row2.ReadOnly = false; //значение в этой колонке нельзя править
                row2.Frozen = true; //флаг, что данная колонка всегда отображается на своем месте

                size++;
                matrixDataGrid.Columns.Add(column);
                matrixDataGrid.Rows.Add(row);
                vectorDataGrid.Rows.Add(row1);
                x0DataGrid.Rows.Add(row2);
                sizeWrap();
            }
        }
        void MakeWhite()
        {
            for (int i = 0; i < size+1; i++)
                for (int j = 0; j < size+1; j++)
                {
                    matrixDataGrid.Rows[j].Cells[i].Style.BackColor = Color.White;
                }
        }
        private void matrixDataGrid_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == size || e.RowIndex == size)
            {
                e.Cancel = true;
            }
        }

        void ReduceMatrix()
        {
            try
            {
                size--;
                matrixDataGrid.Rows.RemoveAt(size);
                matrixDataGrid.Columns.RemoveAt(size);
                vectorDataGrid.Rows.RemoveAt(size);
                x0DataGrid.Rows.RemoveAt(size);
                sizeWrap();
            }
            catch
            {
               
            }
        }

        private void matrixDataGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (matrixDataGrid[e.ColumnIndex, e.RowIndex].Value != null)
            {
                textToOnlyNumbers(e.ColumnIndex, e.RowIndex, 1);
                if (property == true)
                    matrixDataGrid[e.RowIndex, e.ColumnIndex].Value = matrixDataGrid[e.ColumnIndex, e.RowIndex].Value;
                matrixDataGrid.UpdateCellValue(e.RowIndex, e.ColumnIndex);
            }
            else
            {
                if (property == true)
                    matrixDataGrid[e.RowIndex, e.ColumnIndex].Value = "";
            }
        }

        private void matrixForm_VisibleChanged(object sender, EventArgs e)
        {
            Form1.justDoIt.Enabled = true;
            Form1.loadFiles.Enabled = true;
            if (this.Visible == false)
                Form1.next.Enabled = true;
        }

        private (int row,int col) GetMouseRowCol(int X, int Y)
        {
            int col = Math.Max(2, X / widthCol);
            int row = Math.Max(2, Y / heightRow);
            return (row+1, col+1);
        }
        private bool IsMouseAtGrey(int X, int Y)
        {
            (int row, int col) coor = GetMouseRowCol(X, Y);

            int gsize = size + 1;
            if ((coor.col == gsize || coor.row == gsize)
                && (coor.col <= gsize && coor.row <= gsize))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void UpdateMouse(int X, int Y)
        {
            if(mouseSpandClutch && size<=maxSize)
            {
                (int row, int col) coor = GetMouseRowCol(X, Y);
                matrixDataGrid.ClearSelection();
                int gsize = size + 1;
                if ((coor.col > gsize || coor.row > gsize) && (X > lastUpdate.x || Y> lastUpdate.y ) && gsize < maxSize)
                {
                    MakeWhite();
                    AddGrey();
                    lastUpdate = (X, Y);
                }
                if ((coor.col < gsize && coor.row < gsize) && (X < lastUpdate.x || Y < lastUpdate.y))
                {
                    ReduceMatrix();
                    lastUpdate = (X, Y);
                }
            }
        }
        private void groupBox1_MouseMove(object sender, MouseEventArgs e)
        {
            UpdateMouse(e.X - matrixDataGrid.Location.X, e.Y- matrixDataGrid.Location.Y);
        }

        private void matrixDataGrid_MouseMove(object sender, MouseEventArgs e)
        {
            UpdateMouse(e.X, e.Y);
        }
        (int x, int y) lastUpdate = (0, 0);
        bool mouseSpandClutch = false;
        private void matrixDataGrid_MouseDown(object sender, MouseEventArgs e)
        {
            if(IsMouseAtGrey(e.X, e.Y))
            {
                mouseSpandClutch = true;
                lastUpdate = (e.X, e.Y);
            }
        }

        private void matrixDataGrid_MouseUp(object sender, MouseEventArgs e)
        {
            mouseSpandClutch = false;
            lastUpdate = (0,0);
        }

        private void groupBox1_MouseLeave(object sender, EventArgs e)
        {
            mouseSpandClutch = false;
            lastUpdate = (0, 0);
        }

        private void vector_CellEdit(object sender, DataGridViewCellEventArgs e)
        {
            int col = e.ColumnIndex;
            int row = e.RowIndex;
            try
            {
                ((DataGridView)sender)[col, row].Value = Eval(((DataGridView)sender)[col, row].Value.ToString().Replace(",", "."));
            }
            catch
            {
                ((DataGridView)sender)[col, row].Value = null;
            }
        }
    }
}
