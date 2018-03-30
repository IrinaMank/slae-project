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
            property = Form1.property_matr;
            size = 2;
            var column1 = new DataGridViewColumn();
            column1.Width = 30; //ширина колонки
            column1.ReadOnly = false; //значение в этой колонке нельзя править
            column1.Name = "column"; //текстовое имя колонки, его можно использовать вместо обращений по индексу
            column1.Frozen = true; //флаг, что данная колонка всегда отображается на своем месте
            column1.CellTemplate = new DataGridViewTextBoxCell(); //тип нашей колонки
            var column2 = new DataGridViewColumn();
            column2.Width = 30; //ширина колонки
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
                column.Width = 30; //ширина колонки
                column.ReadOnly = false; //значение в этой колонке нельзя править
                column.Name = "column" + Convert.ToString(i); //текстовое имя колонки, его можно использовать вместо обращений по индексу
                column.Frozen = true; //флаг, что данная колонка всегда отображается на своем месте
                column.CellTemplate = new DataGridViewTextBoxCell(); //тип нашей колонки

                var row = new DataGridViewRow();
                row.Height = 30; //ширина колонки
                row.ReadOnly = false; //значение в этой колонке нельзя править
                row.Frozen = true; //флаг, что данная колонка всегда отображается на своем месте

                var row1 = new DataGridViewRow();
                row1.Height = 30; //ширина колонки
                row1.ReadOnly = false; //значение в этой колонке нельзя править
                row1.Frozen = true; //флаг, что данная колонка всегда отображается на своем месте

                var row2 = new DataGridViewRow();
                row2.Height = 30; //ширина колонки
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

            vectorDataGrid.Width = 30;
            x0DataGrid.Width = 30;

            groupBox2.Width = vectorDataGrid.Width + 60;
            groupBox3.Width = vectorDataGrid.Width + 60;
            sizeWrap();
        }

        /// <summary>
        /// Подгонка размеров элементов окна под текущую размерность матрицы. Вызывается каждый раз при измененении размеров матрицы.
        /// </summary>
        private void sizeWrap()
        {
            matrixDataGrid.Height = 30 * size;
            matrixDataGrid.Width = 30 * size;
            vectorDataGrid.Height = 30 * size;
            x0DataGrid.Height = 30 * size;
            groupBox1.Height = matrixDataGrid.Height + 50;
            groupBox1.Width = matrixDataGrid.Width + 30;
            groupBox2.Height = vectorDataGrid.Height + 50;
            groupBox3.Height = vectorDataGrid.Height + 50;
        }

        public void textToOnlyNumbers(int col, int row)
        {
            try
            {
                matrixDataGrid[col, row].Value = Eval(matrixDataGrid[col, row].Value.ToString().Replace(",", "."));
            }
            catch
            {
                matrixDataGrid[col, row].Value = null;
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
                writer.WriteLine((size-1).ToString());
                for (int j = 0; j < size-1; j++)
                {
                    for (int i = 0; i < size-1; i++)
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

            Factory.RightVector = new SimpleVector(size - 1);
            //FileLoadForm.F = new SimpleVector(size-1);

            object line1;
            for (int i = 0; i < size-1; i++)
            {
                line1 = vectorDataGrid[0, i].Value;
                if (line1 != null)
                    Factory.RightVector[i] = Convert.ToDouble(line1);
                else
                    Factory.RightVector[i] = 0.0;
            }
            Factory.X0 = new SimpleVector(size-1);
            for (int i = 0; i < size-1; i++)
            {
                line1 = x0DataGrid[0, i].Value;
                if (line1 != null)
                    Factory.X0[i] = Convert.ToDouble(line1);
                else
                    Factory.X0[i] = 0.0;
            }
            this.Visible = false;
        }

        private void matrixDataGrid_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == size - 1 || e.RowIndex == size - 1)
            {
                for (int i = 0; i < size; i++)
                    for (int j = 0; j < size; j++)
                    {
                        matrixDataGrid.Rows[j].Cells[i].Style.BackColor = Color.White;
                        vectorDataGrid.Rows[j].Cells[0].Style.BackColor = Color.White;
                        x0DataGrid.Rows[j].Cells[0].Style.BackColor = Color.White;
                    }
                if (size < 10)
                {
                    var column = new DataGridViewColumn();
                    column.Width = 30; //ширина колонки
                    column.ReadOnly = false; //значение в этой колонке нельзя править
                    column.Name = "column" + Convert.ToString(size); //текстовое имя колонки, его можно использовать вместо обращений по индексу
                    column.Frozen = true; //флаг, что данная колонка всегда отображается на своем месте
                    column.CellTemplate = new DataGridViewTextBoxCell(); //тип нашей колонки
                    column.DefaultCellStyle.BackColor = Color.Gray;

                    var row = new DataGridViewRow();
                    row.Height = 30; //ширина колонки
                    row.ReadOnly = false; //значение в этой колонке нельзя править
                    row.Frozen = true; //флаг, что данная колонка всегда отображается на своем месте
                    row.DefaultCellStyle.BackColor = Color.Gray;

                    var row1 = new DataGridViewRow();
                    row1.Height = 30; //ширина колонки
                    row1.ReadOnly = false; //значение в этой колонке нельзя править
                    row1.Frozen = true; //флаг, что данная колонка всегда отображается на своем месте
                    row1.DefaultCellStyle.BackColor = Color.Gray;

                    var row2 = new DataGridViewRow();
                    row2.Height = 30; //ширина колонки
                    row2.ReadOnly = false; //значение в этой колонке нельзя править
                    row2.Frozen = true; //флаг, что данная колонка всегда отображается на своем месте
                    row2.DefaultCellStyle.BackColor = Color.Gray;

                    size++;
                    matrixDataGrid.Columns.Add(column);
                    matrixDataGrid.Rows.Add(row);
                    vectorDataGrid.Rows.Add(row1);
                    x0DataGrid.Rows.Add(row2);


                    sizeWrap();
                }
            }
        }

        void RecClear()
        {
            bool notEmptyRow = false;
            for (int i = 0; i < size; i++)
            {

                if (matrixDataGrid[size - 2, i].Value != null || matrixDataGrid[i, size - 2].Value != null)
                    notEmptyRow = true;
            }
            if (!notEmptyRow && size > 3)
            {
                try
                {
                    size--;
                    matrixDataGrid.Rows.RemoveAt(size - 1);
                    matrixDataGrid.Columns.RemoveAt(size - 1);
                    vectorDataGrid.Rows.RemoveAt(size - 1);

                }
                catch
                {

                }
                for (int i = 0; i < size; i++)
                {
                    matrixDataGrid.Rows[i].Cells[size - 1].Style.BackColor = Color.Gray;
                    matrixDataGrid.Rows[size - 1].Cells[i].Style.BackColor = Color.Gray;
                }
                vectorDataGrid.Rows[size - 1].Cells[0].Style.BackColor = Color.Gray;
                x0DataGrid.Rows[size - 1].Cells[0].Style.BackColor = Color.Gray;
                sizeWrap();
                RecClear();
            }
        }

        private void matrixDataGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (matrixDataGrid[e.ColumnIndex, e.RowIndex].Value != null)
            {
                textToOnlyNumbers(e.ColumnIndex, e.RowIndex);
                if (property == true)
                    matrixDataGrid[e.RowIndex, e.ColumnIndex].Value = matrixDataGrid[e.ColumnIndex, e.RowIndex].Value;
                matrixDataGrid.UpdateCellValue(e.RowIndex, e.ColumnIndex);
            }
            else
            {
                if (property == true)
                    matrixDataGrid[e.RowIndex, e.ColumnIndex].Value = "";

                if ((e.ColumnIndex == size - 2 || e.RowIndex == size - 2) && size > 1)
                {
                    RecClear();
                }
            }
        }

        private void matrixForm_VisibleChanged(object sender, EventArgs e)
        {
            Form1.justDoIt.Enabled = true;
            Form1.loadFiles.Enabled = true;
            if (this.Visible == false)
                Form1.next.Enabled = true;
        }
    }
}
