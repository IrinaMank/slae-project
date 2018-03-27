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
        public int property;

        public matrixForm()
        {
            InitializeComponent();
        }
        
        private void matrixFormLoad(object sender, EventArgs e)
        {
            sizeRead();
            size = 2;
            var column1 = new DataGridViewColumn();
            column1.Width = 30; //ширина колонки
            column1.ReadOnly = false; //значение в этой колонке нельзя править
            column1.Name = "column"; //текстовое имя колонки, его можно использовать вместо обращений по индексу
            column1.Frozen = true; //флаг, что данная колонка всегда отображается на своем месте
            column1.CellTemplate = new DataGridViewTextBoxCell(); //тип нашей колонки
            vectorDataGrid.Columns.Add(column1);
            vectorDataGrid.BringToFront();

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


                matrixDataGrid.Columns.Add(column);
                matrixDataGrid.Rows.Add(row);
                vectorDataGrid.Rows.Add(row1);

            }

            matrixDataGrid.AllowUserToResizeRows = false; // запрещаем менять размер строчек
            matrixDataGrid.AllowUserToResizeColumns = false; // запрещаем менять размер столбцов
            matrixDataGrid.RowHeadersVisible = false; // делаем невидимыми заголовки строк
            matrixDataGrid.ColumnHeadersVisible = false; // делаем невидимыми заголовки столбцов
            matrixDataGrid.Height = 30 * size; // высота рамки
            matrixDataGrid.Width = 30 * size; // ширина рамки

            vectorDataGrid.AllowUserToResizeRows = false; // запрещаем менять размер строчек
            vectorDataGrid.AllowUserToResizeColumns = false; // запрещаем менять размер столбцов
            vectorDataGrid.RowHeadersVisible = false; // делаем невидимыми заголовки строк
            vectorDataGrid.ColumnHeadersVisible = false; // делаем невидимыми заголовки столбцов
            vectorDataGrid.Height = 30 * size; // высота рамки
            vectorDataGrid.Width = 30; // ширина рамки


            groupBox1.Height = matrixDataGrid.Height + 50; // высота групбокса относительно размера матрицы
            groupBox1.Width = matrixDataGrid.Width + 30; // ширина групбокса относительно размера матрицы
            groupBox2.Height = vectorDataGrid.Height + 50; // высота групбокса относительно размера матрицы
            groupBox2.Width = vectorDataGrid.Width + 60; // ширина групбокса относительно размера матрицы
        }


        public void textToOnlyNumbers(ref string text)
        {
            try{
                text = Convert.ToDouble(text).ToString();
            }
            catch {
                text = "";
            }
        }

        private void doMatrixNull()
        {
            for (int i = 0; i < size - 1; i++)
            {
                for (int j = 0; j < size - 1; j++)
                    if (matrixDataGrid[j, i].Value == null)
                        matrixDataGrid[j, i].Value = 0;

                if (vectorDataGrid[0, i].Value == null)
                    vectorDataGrid[0, i].Value = 0;
            }
            
        }
        private void sizeRead() // чтение размерности матрицы из файла
        {
            using (StreamReader reader = File.OpenText("mymatrixSet.txt"))
            {
                size = Convert.ToInt32(reader.ReadLine());
                property = Convert.ToInt32(reader.ReadLine());
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            doMatrixNull();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            doMatrixNull();
            size--;
            Form1.str_format_matrix = "Плотный";
            Factory.CreateMatrix(Form1.str_format_matrix);
            List<string> arrays = Factory.name_arr;
            int count_arr = arrays.Count();

            string name = "myMatrix.txt";
            using (StreamWriter writer = File.CreateText(name))
            {
                string line;
                writer.WriteLine(size.ToString());
                for (int j = 0; j < size; j++)
                {
                    for (int i = 0; i < size; i++)
                    {
                        line = matrixDataGrid[i,j].Value.ToString();
                        writer.Write(line + " ");
                    }
                    writer.Write("\r\n");
                }
            }
            Form2.filenames_format.Clear();
            Form2.filenames_format.Add(arrays[0].ToString(), name);

                Form2.F = new SimpleVector(size);
                string line1;
                for (int i = 0; i < size; i++)
                {
                    line1 = vectorDataGrid[0,i].Value.ToString();
                    Form2.F[i] = Convert.ToDouble(line1);                 
                }
            Form2.X0 = new SimpleVector(size);
            for (int i = 0; i < size; i++)
                {
                    Form2.X0[i] = 1.0;
                }

            this.Close();

        }

        private void matrixDataGrid_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == size-1 || e.RowIndex == size-1)
            {
                for (int i=0; i<size;i++)
                    for (int j = 0; j < size; j++)
                    {
                        matrixDataGrid.Rows[j].Cells[i].Style.BackColor = Color.White;
                        vectorDataGrid.Rows[j].Cells[0].Style.BackColor = Color.White;
                    }

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

                size++;
                matrixDataGrid.Columns.Add(column);
                matrixDataGrid.Rows.Add(row);
                vectorDataGrid.Rows.Add(row1);
                matrixDataGrid.Height = 30 * size; // высота рамки
                matrixDataGrid.Width = 30 * size; // ширина рамки
                vectorDataGrid.Height = 30 * size; // высота рамки

                groupBox1.Height = matrixDataGrid.Height + 50; // высота групбокса относительно размера матрицы
                groupBox1.Width = matrixDataGrid.Width + 30; // ширина групбокса относительно размера матрицы
                groupBox2.Height = vectorDataGrid.Height + 50; // высота групбокса относительно размера матрицы
                groupBox2.Width = vectorDataGrid.Width + 60; // ширина групбокса относительно размера матрицы
                
            }
        }

        private void matrixDataGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (matrixDataGrid[e.ColumnIndex, e.RowIndex].Value != null)
            {
                var buf = matrixDataGrid[e.ColumnIndex, e.RowIndex].Value.ToString();
                textToOnlyNumbers(ref buf);
                if (property == 1)
                    matrixDataGrid[e.RowIndex, e.ColumnIndex].Value = matrixDataGrid[e.ColumnIndex, e.RowIndex].Value;
                matrixDataGrid.UpdateCellValue(e.RowIndex, e.ColumnIndex);
            }
            else
            {
                if (property == 1)
                    matrixDataGrid[e.RowIndex, e.ColumnIndex].Value = "";

                if (e.ColumnIndex == size - 2 || e.RowIndex == size - 2)
                {
                    bool notEmptyRow = false;
                    for (int i = 0; i < size; i++)
                    {
                        if (matrixDataGrid[e.ColumnIndex, e.RowIndex].Value != null || matrixDataGrid[e.RowIndex, e.ColumnIndex].Value != null)
                            notEmptyRow = true;
                    }
                    if (!notEmptyRow)
                    {
                        size--;
                        matrixDataGrid.Rows.RemoveAt(e.ColumnIndex);
                        matrixDataGrid.Columns.RemoveAt(e.ColumnIndex);
                        vectorDataGrid.Rows.RemoveAt(e.ColumnIndex);
                        matrixDataGrid.Height = 30 * size;
                        matrixDataGrid.Width = 30 * size; 
                        vectorDataGrid.Height = 30 * size;
                        groupBox1.Height = matrixDataGrid.Height + 50;
                        groupBox1.Width = matrixDataGrid.Width + 30;
                        groupBox2.Height = vectorDataGrid.Height + 50;                      
                    }
                }
            }
        }
    }

    
}
